using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool rotateTowardsMouse;

    public Text enemiesText;
    public Text healthText;
    public Camera m_MainCamera;
    private Animator animator;
    public Collider lightCollider;
    public Collider defCollider;
    public int health = 20;
    public string healthString = "20";
    private bool walkCheck;
    private bool idleCheck;
    private bool attackCheck;
    private bool WAttackCheck;

    //public Text;


    private Rigidbody rb;

    enum States
    {
        Idle,
        Walk,
        Attack,
        WAttack,
        Defend,
        WDefend
    }

    States state;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        healthText.text = health.ToString();
        state = States.Idle;
    }


    void Update()
    {
        healthText.text = health.ToString();
    
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);  

        if( state == States.Walk )
        {
            //Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
            animator.SetBool("WalkAttack", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Walk", true);
            WAttackCheck = CheckForWAttack();
            idleCheck = CheckForIdle();
            if(WAttackCheck == true)
            {
                state = States.WAttack;
            }
            else if(idleCheck == true)
            {
                state = States.Idle;
            }
        }

        if( state == States.Idle )
        {
            
            //Debug.Log("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
            animator.SetBool("WalkAttack", false);
            animator.SetBool("Idle", true);
            walkCheck = CheckForWalk();
            attackCheck = CheckForIAttack(); 
            if(walkCheck == true)
            {
                state = States.Walk;
            }            
            else if(attackCheck == true)
            {
                state = States.Attack;
            }

            //print("xv=" + rb.velocity.x + "  zv=" + rb.velocity.z);

            // adding friction

            rb.velocity = new Vector3( rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);


            
        }

        if( state == States.Attack )
        {   
            //Debug.Log("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            DoAttack();
            walkCheck = CheckForWalk();
            if(walkCheck == true)
            {
                state = States.Walk;
            }
            else
            {
                state = States.Idle;
            }
            
            

        }

        if( state == States.WAttack )
        {   
            //Debug.Log("-----------------------------------------------------");
            DoWalkAttack();
            walkCheck = CheckForWalk();
            if(walkCheck == true)
            {
                state = States.Walk;
            }
            else
            {
                state = States.Idle;
            }
            
            

        }



        var movementVector = MoveTowardTarget(targetVector);  

        if(!rotateTowardsMouse)
        {
            RotateTowardMovementVector  (movementVector); 
        }
        else
        {
            RotateTowardMouseVector();
        }

        if(health < 1)
        {
            Destroy(gameObject);
        }
    }

    private void LightAttack()
    {
        lightCollider.enabled = !lightCollider.enabled;
        gameObject.tag = "PlayerAttack";
    }
    private void LightAttackEnd()
    {
        lightCollider.enabled = !lightCollider.enabled;
        gameObject.tag = "Player";
    }

    private void RotateTowardMouseVector()
    {
        Ray ray = m_MainCamera.ScreenPointToRay(_input.MousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        if(movementVector.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }   

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;

        return targetVector;
    }




    void DoWalk()
    {
        //agent.destination = goal.position;
        animator.SetBool("EnemyAttack", false);
        animator.SetBool("EnemyIdle", false);
        animator.SetBool("EnemyWalk", true);
    }


    void DoIdle()
    {
        animator.SetBool("WalkAttack", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
    }

    void DoAttack()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("WalkAttack", false);
        animator.SetBool("Attack", true);
    }

    void DoWalkAttack()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);
        animator.SetBool("WalkAttack", true);
    }

    bool CheckForWalk()
    {
        if( (_input.InputVector.x != 0) || (_input.InputVector.y !=0) )
        {
            walkCheck = true;
        }
        else
        {
            walkCheck = false;
        }
        return walkCheck;
    }

    bool CheckForIdle()
    {
        if( (_input.InputVector.x == 0) && (_input.InputVector.y ==0) )
        {
            if( (_input.InputVector.x == 0) || (_input.InputVector.y ==0) ){
                idleCheck = true;
            }
        }
        else
        {
            idleCheck = false;
        }
        return idleCheck;
    }

    bool CheckForIAttack()
    {
        if( (_input.InputVector.x == 0) && (_input.InputVector.y ==0) )
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                attackCheck = true;
            }
            else
            {
                attackCheck = false;
            }
        }
        else
        {
            attackCheck = false;
        }
        return attackCheck;
    }

    bool CheckForWAttack()
    {
        if( (_input.InputVector.x != 0) || (_input.InputVector.y !=0) )
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                WAttackCheck = true;
            }
            else
            {
                WAttackCheck = false;
            }
        }
        else
        {
            WAttackCheck = false;
        }
        return WAttackCheck;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            health = health -1;
            
        }
    }







}
