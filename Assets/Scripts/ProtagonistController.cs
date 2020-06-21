using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistController : MonoBehaviour
{
    //public vars
    public float turnSpeed = 180f;
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;
    public float walkBackwardsSpeed = 1.5f;
    public float sprintSpeed = 5.0f;


    //private vars
    Animator animator;
    Rigidbody rigidBody;

    float moveSpeed = 1.0f;
    bool isJumping = false;
    bool isLanding = false;

    bool walk = true;
    bool run = false;
    bool sprint = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        //var power = Input.GetAxis("Fire1");
        var jump = Input.GetAxis("Jump");
        var walk = Input.GetAxis("Walk");
        var run = Input.GetAxis("Run");
        var sprint = Input.GetAxis("Sprint");


        //set walk, run or sprint speed if input
        if (!Mathf.Approximately(walk, 0))
        {
            this.walk = true;
            this.run = false;
            this.sprint = false;
        }
        else if(!Mathf.Approximately(run, 0))
        {
            this.walk = false;
            this.run = true;
            this.sprint = false;
        }
        else if(!Mathf.Approximately(sprint, 0))
        {
            this.walk = false;
            this.run = false;
            this.sprint = true;
        }
        animator.SetBool("walk", this.walk);
        animator.SetBool("run", this.run);
        animator.SetBool("sprint", this.sprint);

        


        if (isJumping == false && isLanding == false)
        {

            if (Mathf.Approximately(vertical, 0.0f)) //character is idling
            {
                moveSpeed = 1.0f;
                animator.SetBool("forwardPressed", false);
                animator.SetBool("backwardPressed", false);
                if (!Mathf.Approximately(jump, 0))
                {
                    startJump("idleIsSource", 0f, moveSpeed / 1.5f);

                }
            }
            else if (vertical > 0) //character moving forward
            {
                animator.SetBool("forwardPressed", true);
                if (this.sprint)
                {
                    moveSpeed = sprintSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("sprintIsSource", sprintSpeed, sprintSpeed / 2);

                    }
                }
                else if(this.run)
                {
                    moveSpeed = runSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("runIsSource", runSpeed, runSpeed / 2);

                    }
                }
                else
                {
                    moveSpeed = walkSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("walkIsSource", walkSpeed, walkSpeed / 2);

                    }
                }
            }
            else if (vertical < 0) //character moving backward
            {
                moveSpeed = walkBackwardsSpeed;
                animator.SetBool("backwardPressed", true);
                if (!Mathf.Approximately(jump, 0))
                {
                    startJump("backwardsIsSource", moveSpeed, moveSpeed / 2);

                }
            }


            //animator.SetTrigger("endJump");
            var position = transform.forward * Time.deltaTime * moveSpeed * vertical;
            position += transform.position;
            rigidBody.MovePosition(position);
        }
        else if (isJumping == true)
        {
            RaycastHit hit;
            Debug.Log(rigidBody.velocity.y);
            if (rigidBody.velocity.y < 0 && Physics.Raycast(rigidBody.position, rigidBody.velocity.normalized, out hit, (rigidBody.velocity.y * Time.deltaTime) + Mathf.Sqrt(-rigidBody.velocity.y)))
            {
                Debug.Log("Begin Landing");
                isJumping = false;
                isLanding = true;
                animator.SetTrigger("endJump");
                animator.SetBool("jumpPressed", false);
            }
            else
            {
                //Debug.Log("Miss");
            }
        }
        else if (isLanding == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(rigidBody.position, -transform.up.normalized, out hit, 0.2f))
            {
                Debug.Log("Hit Ground");
                animator.SetTrigger("Land");
                isLanding = false;
            }
            else
            {
                //Debug.Log("Miss");
            }
        }
        Vector3 rotate = new Vector3(0, horizontal * turnSpeed * Time.deltaTime / Mathf.Sqrt(moveSpeed), 0);
        transform.Rotate(rotate);
    }
    //void OnCollisionEnter(Collision other)
    //{
    //    if(isLanding == true)
    //    {
    //        GroundController ground = other.gameObject.GetComponent<GroundController>();
    //        if(ground != null)
    //        {
    //            isLanding = false;
    //        }
    //    }
    //}

    public void startJump(string source, float currentSpeed, float up)
    {
        if (isJumping == false && isLanding == false)
        {
            animator.SetBool("idleIsSource", false);
            animator.SetBool("sprintIsSource", false);
            animator.SetBool("backwardsIsSource", false);
            animator.SetBool("runIsSource", false);
            isJumping = true;
            animator.SetBool(source, true);
            animator.SetBool("jumpPressed", true);
            var force = (transform.up.normalized * (up * 0.25f)) + (transform.forward.normalized * Mathf.Sqrt(currentSpeed));
            //Debug.Log(force);
            rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
