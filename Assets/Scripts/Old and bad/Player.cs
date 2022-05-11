using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector3 movementVector;
    private Animator animator;
    public float speed;
    private Rigidbody body;
    

    // Start is called before the first frame update
    void Start()
    {
        //animator = transform.GetChild(0).GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        speed = 1.5f; 
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), 2.5f);
        }
    }

    void CalculateMovement()
    {
        movementVector = new Vector3( Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        body.velocity = new Vector3(
            movementVector.x * speed,
            movementVector.y,
            movementVector.z * speed
        );
    }

}
