using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using InGameEvent;
using WebManager;
using Controller;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public string state;

    public AudioSource audioSource;
    public AudioClip onShootAudio;
    public Slider hpUI;
    public Text damageUI;
    protected int damageShowTime = 0;
    public Animator animatior;
    public GameObject reward;
    protected Rigidbody rigid;
    protected Movement movement;
    protected NavMeshAgent agent;

    protected bool alive = true;
    public float speed = 4.0f;
    public int curHP = 100;
    public int maxHP = 100;
    protected int stayTime = 0;
    public const int rangeStayTime = 40;
    public const int rangeState = 20;
    public Vector3 position;

    protected bool beBiu = false;
    protected float inBiuTime = 0.0f;

    void Start()
    {
        if (position == null)
            position = transform.position;
    }

    virtual protected void Update()
    {
        position = transform.position;
        target = EnemyController.enemyDict[name].target;
        state = EnemyController.enemyDict[name].state;
    }

    virtual protected void FixedUpdate()
    {
        if(target!=null)
            transform.LookAt(target.transform);

        float _curHp = curHP;
        float _maxHp = maxHP;
        hpUI.value = _curHp / _maxHp;
        damageShowTime--;
        if (damageShowTime < 0)
        {
            damageUI.text = "0";
            damageUI.gameObject.SetActive(false);
        }

        var enemyMsg = MessageConverter.OnEnemyUpdateMsg(this);
        Client.SendMsg(enemyMsg);
    }

    virtual protected void Attack() { }

    protected void Dead()
    {
        if(alive)
        {
            animatior.SetInteger("state", 3);
            GameEvent.RegisterEvent(GameEventType.EnemyDead, this);
        }
        alive = false;
    }

    public string GetState() { return state; }
    public void SetState(string s) { state = s; }

    protected void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name == "BiuBullet(Clone)")
        {
            agent.isStopped = true;
            beBiu = true;
            inBiuTime = 3.0f;
        }


        if (collision.collider.name == "Small_Bullet(Clone)")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            curHP -= bullet.damage;
            damageUI.text = $"{int.Parse(damageUI.text) + bullet.damage}";
            damageUI.gameObject.SetActive(true);
            damageShowTime = 50;

            audioSource.clip = onShootAudio;
            audioSource.Play();
        }
        if (curHP <= 0 && alive)
        {
            Dead();
        }
    }
}
