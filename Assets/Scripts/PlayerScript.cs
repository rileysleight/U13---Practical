using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool rotateTowardsMouse;
    
    public Camera m_MainCamera;
    private Animator animator;
    public Collider lightCollider;
    private bool walkCheck;
    private bool idleCheck;

    enum States
    {
        Idle,
        Walk,
        Attack
    }

    States state;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    

    void Start()
    {
        animator = GetComponent<Animator>();
        

        state = States.Idle;
    }


    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);  

        if( state == States.Walk )
        {
            //DoWalk();
            animator.SetBool("Idle", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Walk", true);
        }

        if( state == States.Idle )
        {
            walkCheckCheckForWalk();
            animator.SetBool("Idle", true);
        }

        if( state == States.Attack )
        {
            DoAttack();

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
    }

    private void LightAttack()
    {
        lightCollider.enabled = !lightCollider.enabled;
        Debug.Log("Collider.enabled = " + lightCollider.enabled);
    }
    private void LightAttackEnd()
    {
        lightCollider.enabled = !lightCollider.enabled;
        Debug.Log("Collider.enabled = " + lightCollider.enabled);
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
        animator.SetBool("Walk", true);
    }


    void DoIdle()
    {
        animator.SetBool("Idle", true);
    }

    void DoAttack()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", true);
    }

    bool CheckForWalk(bool walkCheck)
    {
        if( (_input.InputVector.x != 0) || (_input.InputVector.y !=0) )
        {
            walkCheck = true;
        }
        return walkCheck;
    }

    void CheckForIdle(bool idleCheck)
    {
        if( (_input.InputVector.x == 0) || (_input.InputVector.y ==0) )
        {
            idleCheck = true;
        }
        return idleCheck;
    }

    //void CheckForAttack()
    //{

    //}










}
