using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsScript : MonoBehaviour
{
    public float damage = 5f;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected");
        var script = other?.gameObject?.GetComponent<MonsterScript>();
        if (script != null)
        {
            Debug.Log("Monster Detected");
            script.Damage(damage);
        }
    }
}
