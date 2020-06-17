using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanesScript : MonoBehaviour
{
    //public parameters

    public float maxSpeed = 100.0f;
    public float currentSpeed = 10.0f;
    public float airDrag = 1.0f;
    public float acceleration = 1.0f;
    public float liftFactor = 0.1f;

    //private vars

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var power = Input.GetAxis("Fire1");
        var engine = Input.GetAxis("Thrust"); //fire the engine


        if(!Mathf.Approximately(engine, 0)) {
            currentSpeed += acceleration;
            currentSpeed = currentSpeed > maxSpeed ? maxSpeed : currentSpeed;
        }
        else
        {
            currentSpeed -= airDrag;
            currentSpeed = currentSpeed < 0 ? 0f : currentSpeed;
        }

        rigidbody.AddRelativeForce(0, liftFactor * currentSpeed, currentSpeed);

        //var position = transform.forward * Time.deltaTime * moveSpeed * vertical;
        //position += transform.position;
        //rigidbody.MovePosition(position);

    }
}
