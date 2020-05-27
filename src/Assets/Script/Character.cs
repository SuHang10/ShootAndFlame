using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Logic;
using WebManager;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public enum ChapterEvent
    {
        hp,
        clip,
    }

    /*AnamationController
     * state:
     * 0-->>死亡
     * 1-->>
     * 2-->>跳跃
     * 3-->>站立
     * 4-->>奔跑
     * shootMode：
     * 0-->>单发
     * 1-->>点射
     * 2-->>自动
     * shooting:
     * false-->>未射击
     * true -->>射击
     * reloading:
     * false-->>未装填
     * true -->>装填
     */

    public AudioClip onShootAudio;
    public Animator animatior;
    public int lavel = 1;
    public int exp = 0;
    public const int maxExp = 20;
    public float speed = 2.5f;
    public int maxHP = 2000;
    public int curHP = 2000;
    public int damage = 20;

    private PlayerWeapon weapon;
    private bool onLand = true;
    private Movement movement;
    private Rigidbody rigid;
    private float x, y;
    private bool alive = true;
    private int deadCount = 0;
    void Start()
    {
        /*DontDestroyOnLoad(this);
        if (GameObject.Find(name).gameObject != this.gameObject)
            Destroy(this.gameObject);*/

        rigid = GetComponent<Rigidbody>();
        movement = new Movement();
        movement.Initialized(transform, speed);
        weapon = GetComponent<PlayerWeapon>();

        GetInfo(LogicMain.chapter);
        //animatior = GetComponent<Animator>();
    }

    void Update()
    {
        if(alive)
        {
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
            transform.eulerAngles += new Vector3(0, x * 3, 0);
            /*if (!Input.GetKey(KeyCode.LeftAlt))
            {
                x = Input.GetAxis("Mouse X");
                y = Input.GetAxis("Mouse Y");
                transform.eulerAngles += new Vector3(0, x * 3, 0);
            }*/

            if (Input.GetKeyUp(KeyCode.Space))
            {
                animatior.SetInteger("state", 3);
                onLand = true;
            }
            if (Input.GetMouseButtonUp(0) || weapon.ShootInCD())
            {
                animatior.SetBool("shooting", false);
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                animatior.SetBool("reloading", false);
            }
            if (onLand)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animatior.SetInteger("state", 2);
                    onLand = false;
                }
                else if (Input.GetMouseButtonDown(0) && !weapon.ShootInCD() && weapon.GetClipNum() > 0)
                {
                    animatior.SetBool("shooting", true);
                    weapon.Shoot();
                }
                if (Input.GetKeyDown(KeyCode.R) && weapon.ammunition>0)
                {
                    animatior.SetBool("reloading", true);
                    weapon.ChangeClip();
                    //Act(ChapterEvent.clip, weapon.ammunition);
                }

                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                {
                    rigid.velocity = movement.LeftForward();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                {
                    rigid.velocity = movement.RighrForward();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                {
                    rigid.velocity = movement.LeftBack();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
                {
                    rigid.velocity = movement.RightBack();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rigid.velocity = movement.Left();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    rigid.velocity = movement.Right();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    rigid.velocity = movement.Forward();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    rigid.velocity = movement.Back();
                    if (onLand)
                        animatior.SetInteger("state", 4);
                }
                else if (onLand)
                    animatior.SetInteger("state", 3);
            }
        }
    }

    private void FixedUpdate()
    {
        if (curHP <= 0)
            alive = false;
        if (alive)
            UpdateUserInfo();
        else
        {
            deadCount++;
            if (deadCount < 5)
            {
                var userDeadMsg = MessageConverter.OnUserDeadMsg(LogicMain.chapter);
                Client.SendMsg(userDeadMsg);
            }
            else if (deadCount > 100)
                SceneManager.LoadScene("Login");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "HealthBox(Clone)")
        {
            AddHp();
            //Act(ChapterEvent.hp, curHP);
        }
        else if (collision.collider.name == "AmmoBox(Clone)")
        {
            AddAmmunition();
            //Act(ChapterEvent.clip, weapon.ammunition);
        }
        else if (collision.collider.name == "Small_Bullet(Clone)" && alive)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            curHP = curHP > 0 ? curHP - bullet.damage : 0;
            Debug.Log("角色HP=" + curHP.ToString());
        }
        else if (collision.collider.name.Contains("ArmPalm")&&alive)
        {
            curHP = curHP > 0 ? curHP - 1 : 0;
            Debug.Log("角色HP=" + curHP.ToString());
        }
        if (curHP <= 0)
        {
            alive = false;
            animatior.SetInteger("state", 0);
        }
    }

    private void Act(ChapterEvent chapterEvent, int i)
    {
        switch(chapterEvent)
        {
            case ChapterEvent.hp:
                var hpMsg = MessageConverter.OnHealthMsg(name, i.ToString()); 
                Client.SendMsg(hpMsg);
                break;
            case ChapterEvent.clip:
                var clipMsg = MessageConverter.OnClipMsg(name,i.ToString());
                Client.SendMsg(clipMsg);
                break;
            default:
                break;
        }
    }
    private void GetInfo(User user)
    {
        name = user.GetName();
        transform.position = new Vector3(user.GetPos_X(), 1, user.GetPos_Z());
        lavel = user.GetLevel();
        exp = user.GetEXP();
        curHP = user.GetHP();
        maxHP = 2000 + (lavel - 1) * 200;
        weapon.ammunition = user.GetClip();
        weapon.damage = 20 + (lavel - 1) * 5;

    }
    private void UpdateUserInfo()
    {
        exp = LogicMain.chapter.GetEXP();
        if(exp>100)
        {
            lavel += (exp / 100);
            exp %= 100;
            damage = 20 + (lavel - 1) * 5;
            maxHP = 2000 + (lavel - 1) * 200;
            curHP = maxHP;
        }

        deadCount = 0;
        LogicMain.chapter.SetPos_X(transform.position.x);
        LogicMain.chapter.SetPos_Y(transform.position.y);
        LogicMain.chapter.SetPos_Z(transform.position.z);
        LogicMain.chapter.SetHp(curHP);
        LogicMain.chapter.SetClip(weapon.ammunition);
        LogicMain.chapter.SetLevel(lavel);
        LogicMain.chapter.SetEXP(exp);

        var userMsg = MessageConverter.OnUserUpdateMsg(LogicMain.chapter);
        //Debug.Log(Turner.ToString(userMsg));
        Client.SendMsg(userMsg);
    }
    private void AddHp()
    {
        Debug.Log($"before hp={curHP}");
        if (curHP > 0)
            curHP += 10;
        if (curHP > maxHP)
            curHP = maxHP;
        Debug.Log($"after hp={curHP}");
        Debug.Log("吃血包");
    }
    private void AddAmmunition()
    {
        weapon.ammunition += 30;
        Debug.Log("吃子弹包");
    }
}
