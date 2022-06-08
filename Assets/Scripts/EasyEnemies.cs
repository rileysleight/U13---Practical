using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EasyEnemies : MonoBehaviour
{
    public Transform enemy;
    public Transform goal;
    private NavMeshAgent agent;
    public Collider lightCollider;
    private Animator animator;
    private Rigidbody rb;
    

    enum States
    {   
        Idle,
        //Walk,
        Attack,
        Die
    }

    States state;

    //S
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        

        agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position; 
        state = States.Idle;
    }

    //U
    void Update()
    {
        rb.velocity = new Vector3( rb.velocity.x * 0f, rb.velocity.y, rb.velocity.z * 0f);

        //agent.destination = goal.position;
        var dist = Vector3.Distance( transform.position, goal.transform.position);
        //Debug.Log(dist);

        if( state == States.Idle )
        {
            animator.SetBool("EnemyAttack", false);
            animator.SetBool("EnemyIdle", true);
            transform.LookAt(goal.transform);
            if (dist <= 3)
            {
                //state = States.Walk;
                state = States.Attack;
            }
        }
        /*if( state == States.Walk )
        {
            agent.destination = goal.position;
            animator.SetBool("EnemyAttack", false);
            animator.SetBool("EnemyIdle", false);
            animator.SetBool("EnemyWalk", true);
            if (dist <= 2)
            {
                state = States.Attack;
            }
            else if (dist > 6)
            {
                state = States.Idle;
            }
        }*/
        if( state == States.Attack )
        {
            //agent.destination = goal.position;
            animator.SetBool("EnemyIdle", false);
            animator.SetBool("EnemyAttack", true);
            if (dist > 3)
            {
                //state = States.Walk;
                state = States.Idle;
            }
        }
        if( state == States.Die)
        {
            //animator.SetBool("EnemyDeath", true);
            Destroy(gameObject);
        }
  


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            
            state = States.Die;
        }
    }

    void EnemyAttack()
    {
        lightCollider.enabled = !lightCollider.enabled;
        gameObject.tag = "EnemyAttack";
    }
    private void EnemyAttackEnd()
    {
        lightCollider.enabled = !lightCollider.enabled;
        gameObject.tag = "Enemy";
    }

    /*void DoWalk()
    {
        agent.destination = goal.position;
            animator.SetBool("EnemyAttack", false);
            animator.SetBool("EnemyIdle", false);
            animator.SetBool("EnemyWalk", true);
    }*/


    void DoIdle()
    {
        //animator.SetBool("WalkAttack", false);
        animator.SetBool("Attack", false);
        //animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
    }

    void DoAttack()
    {
        animator.SetBool("Idle", false);
        //animator.SetBool("Walk", false);
        //animator.SetBool("WalkAttack", false);
        animator.SetBool("Attack", true);
    }

}
