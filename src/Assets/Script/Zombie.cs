using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy
{
    public MeshCollider leftArmCollider, rightArmCollider;
    public SkinnedMeshRenderer leftArmMeshRenderer, rightArmMeshRenderer;
    void Start()
    {
        /*DontDestroyOnLoad(this);
        if (GameObject.Find(name).gameObject != this.gameObject)
            Destroy(this.gameObject);*/

        speed = 0.8f;
        curHP = 500;
        maxHP = 500;
        stayTime = 0;

        animatior = GetComponent<Animator>();
        rigid = transform.GetComponent<Rigidbody>();
        movement = new Movement();
        //movement.Initialized(transform, speed);

        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        inBiuTime -= Time.deltaTime;
        if (beBiu && inBiuTime > 0)
        {
            //Debug.Log($"Be Biu={inBiuTime}");
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
            if(target!=null)
                agent.SetDestination(target.transform.position);
            else
            {
                agent.SetDestination(new Vector3(Random.Range(-29, 29)
                    , Random.Range(-29, 29), Random.Range(-29, 29)));
            }
        }
        else if (state == "attack")
        {
            animatior.SetInteger("state", 2);
            agent.isStopped = true;
            Attack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Mesh leftArmMesh = new Mesh();
        leftArmMeshRenderer.BakeMesh(leftArmMesh);
        leftArmCollider.sharedMesh = null;
        leftArmCollider.sharedMesh = leftArmMesh;

        Mesh rightArmMesh = new Mesh();
        leftArmMeshRenderer.BakeMesh(rightArmMesh);
        rightArmCollider.sharedMesh = null;
        rightArmCollider.sharedMesh = rightArmMesh;
    }
    protected override void Attack()
    {
        base.Attack();
    }
}
