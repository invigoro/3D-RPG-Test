using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public float maxHealth = 20f;
    public float totalHealth;
    // Start is called before the first frame update
    void Start()
    {
        totalHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        totalHealth -= damage;
        if(totalHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        this.gameObject.SetActive(false);
    }
}
