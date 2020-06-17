using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanesScript : MonoBehaviour
{
    //public parameters

    public float moveSpeed = 1.0f;

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
        var jump = Input.GetAxis("Jump");


        var position = transform.forward * Time.deltaTime * moveSpeed * vertical;
        position += transform.position;
        rigidbody.MovePosition(position);

    }
}
