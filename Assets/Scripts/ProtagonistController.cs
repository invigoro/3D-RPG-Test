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

    public float jumpTimeMultiplier = 10f;

    public float blockingSpeedMultiplier = 0.75f;
    public float attackingSpeedMultiplier = 0.6f;


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
        var block = !Mathf.Approximately(Input.GetAxis("Block"), 0);
        var attack = !Mathf.Approximately(Input.GetAxis("Attack"), 0);

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

        animator.SetBool("blockPressed", block);
        animator.SetBool("attackPressed", attack);
        


        if (isJumping == false)
        {

            if (Mathf.Approximately(vertical, 0.0f)) //character is idling
            {
                moveSpeed = 1.0f;
                animator.SetBool("forwardPressed", false);
                animator.SetBool("backwardPressed", false);
                if (!Mathf.Approximately(jump, 0))
                {
                    startJump("idleIsSource", 0f, moveSpeed);

                }
            }
            else if (vertical > 0) //character moving forward
            {
                animator.SetBool("forwardPressed", true);
                if (this.sprint && !block)
                {
                    moveSpeed = sprintSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("sprintIsSource", sprintSpeed, sprintSpeed);

                    }
                }
                else if(this.run && !block)
                {
                    moveSpeed = runSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("runIsSource", runSpeed, runSpeed);

                    }
                }
                else
                {
                    moveSpeed = walkSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("walkIsSource", walkSpeed, walkSpeed);

                    }
                }
            }
            else if (vertical < 0) //character moving backward
            {
                moveSpeed = walkBackwardsSpeed;
                animator.SetBool("backwardPressed", true);
                if (!Mathf.Approximately(jump, 0))
                {
                    startJump("backwardsIsSource", moveSpeed, moveSpeed);

                }
            }


            //animator.SetTrigger("endJump");
            var position = transform.forward * Time.deltaTime * moveSpeed * vertical;
            if(block) { position *= blockingSpeedMultiplier; }
            if(attack) { position *= attackingSpeedMultiplier; }
            position += transform.position;
            rigidBody.MovePosition(position);
        }
        else if (isJumping == true)
        {
            RaycastHit hit;
            //Debug.Log($"y velocity : {rigidBody.velocity.y}");
            var maxDist = Mathf.Abs(rigidBody.velocity.y) < 1 ? 1 : Mathf.Abs(DistanceTraveledY(rigidBody)) * jumpTimeMultiplier;
            if (rigidBody.velocity.y < 0 && Physics.Raycast(rigidBody.position, new Vector3(0, -1, 0), out hit, maxDist))
            {
                Debug.Log("Hit");
                endJump();
            }
            else
            {
                //Debug.Log("Miss");
            }
        }
        //else if (isLanding == true)
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(rigidBody.position, -transform.up.normalized, out hit, 0.2f))
        //    {
        //        Debug.Log("Hit Ground");
        //        animator.SetTrigger("Land");
        //        isLanding = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Miss");
        //    }
        //}
        Vector3 rotate = new Vector3(0, horizontal * turnSpeed * Time.deltaTime / Mathf.Sqrt(moveSpeed), 0);
        transform.Rotate(rotate);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isJumping && collision.gameObject.GetComponentInParent<GroundScript>() != null)
        {
            endJump();
        }
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

    public void endJump()
    {
        Debug.Log("Begin Landing");
        isJumping = false;
        animator.SetTrigger("endJump");
        animator.SetBool("jumpPressed", false);
    }

    public float DistanceTraveledY(Rigidbody rigidBody)
    {
        var p1 = (Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2)) + (rigidBody.velocity.y * Time.deltaTime);
        Debug.Log($"Distance called: {p1}");
        return p1;
    }
}
