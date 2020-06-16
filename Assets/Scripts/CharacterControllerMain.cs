using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMain : MonoBehaviour
{
    //public vars
    public float jumpTimer1 = 1;
    public float jumpTimer2 = 3;
    public float turnSpeed = 180f;
    public float runSpeed = 3.0f;
    public float runBackwardsSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float jumpHitDist = 10.0f;

    //private vars
    Animator animator;
    Rigidbody rigidbody;
        
    float moveSpeed = 1.0f;
    bool isJumping = false;
    bool isLanding = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var power = Input.GetAxis("Fire1");
        var jump = Input.GetAxis("Jump");

            

        //var rotation = rigidbody.rotation;
        //rotation.Set(rotation.x, rotation.y + (horizontal * turnSpeed * Time.deltaTime / Mathf.Sqrt(moveSpeed)), rotation.z, rotation.w);
        //rigidbody.rotation = rotation;

        //rigidbody.AddForce(transform.forward * moveSpeed * vertical, ForceMode.VelocityChange);
        
        //rigidbody.velocity = transform.forward * moveSpeed * vertical;
        //rigidbody.velocity = new Vector3(0, 0, 10);
        //Debug.Log(rigidbody.velocity);
        //transform.position += transform.forward * Time.deltaTime * moveSpeed * vertical;
        if (isJumping == false && isLanding == false)
        {
            if (Mathf.Approximately(power, 0.0f))
            {
                animator.SetBool("powerPressed", false);
            }

            if (Mathf.Approximately(vertical, 0.0f))
            {
                moveSpeed = 1.0f;
                animator.SetBool("forwardPressed", false);
                animator.SetBool("backwardPressed", false);
                animator.SetBool("powerPressed", false);
                if (!Mathf.Approximately(jump, 0))
                {
                    startJump("idleIsSource", 0f, moveSpeed * 2);

                }
            }
            else if (vertical > 0)
            {
                animator.SetBool("forwardPressed", true);
                if (!Mathf.Approximately(power, 0))
                {
                    animator.SetBool("powerPressed", true);
                    moveSpeed = sprintSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("sprintIsSource", moveSpeed, moveSpeed);

                    }
                }
                else
                {
                    animator.SetBool("powerPressed", false);
                    moveSpeed = runSpeed;
                    if (!Mathf.Approximately(jump, 0))
                    {
                        startJump("runIsSource", moveSpeed * 2, moveSpeed);

                    }
                }
            }
            else if (vertical < 0)
            {
                moveSpeed = runBackwardsSpeed;
                animator.SetBool("backwardPressed", true);
                if (!Mathf.Approximately(jump, 0))
                {
                    startJump("backwardsIsSource", moveSpeed, moveSpeed);

                }
            }


            //animator.SetTrigger("endJump");
            var position = transform.forward * Time.deltaTime * moveSpeed * vertical;
            position += transform.position;
            rigidbody.MovePosition(position);
        }
        else if (isJumping == true)
        {
            RaycastHit hit;
            Debug.Log(rigidbody.velocity.y);
            if (rigidbody.velocity.y < 0 && Physics.Raycast(rigidbody.position, rigidbody.velocity.normalized, out hit, jumpHitDist + Mathf.Sqrt(-rigidbody.velocity.y)))
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
            if (Physics.Raycast(rigidbody.position, -transform.up.normalized, out hit, 0.2f))
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
            rigidbody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
