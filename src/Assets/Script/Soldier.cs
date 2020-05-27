using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Enemy
{
    /*士兵AI设计：
     * 随机数策略：
     * 有子弹则发射
     * 否则随机向前或者转随机角度
     */
    private Weapon weapon;
    void Start()
    {
        speed = 0.3f;
        curHP = 200;
        maxHP = 200;
        stayTime = 0;

        animatior = GetComponent<Animator>();
        rigid = transform.GetComponent<Rigidbody>();
        weapon = GetComponentInParent<SoldierWeapon>();
        weapon.shootCD = 1.0f;
        weapon.damage = 50;
        movement = new Movement();
        //movement.Initialized(transform, speed);

        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        inBiuTime -= Time.deltaTime;
        if (beBiu && inBiuTime > 0)
        {
            //Debug.Log($"Be Biu = {inBiuTime} ");
            return;
        }
        else
            beBiu = false;
        base.Update();
        if (state == "idle")
            animatior.SetInteger("state", 0);
        else if (state == "run")
        {
            animatior.SetInteger("state", 1);
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
        }
        else if (state == "attack")
        {
            agent.isStopped = true;
            animatior.SetInteger("state", 2);
            Attack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack()
    {
        base.Attack();
        if (!weapon.ShootInCD())
        {
            //animatior.SetInteger("state", 2);
            weapon.Shoot();
        }
    }

    /*void Update()
    {
        stayTime--;
        if (!weapon.ShootInCD())
        {
            animatior.SetInteger("state", 2);
            weapon.Shoot();
        }
        else
        {
            if(stayTime<0)
            {
                animatior.SetInteger("state", 1);
                float randomNum = Random.Range(0, rangeState);
                if (randomNum > 3)
                    rigidbody.velocity = movement.Forward();
                else
                {
                    transform.eulerAngles = new Vector3(0, randomNum * 90, 0);
                }
                stayTime= Random.Range(20, rangeStayTime);
            }
        }
    }*/

}
