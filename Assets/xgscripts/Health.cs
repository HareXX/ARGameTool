using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float health = 100f;

    private void Start()
    {
        
    }

    public void Attack(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            //ËÀÍö¶¯»­
            Destroy(gameObject);
        }
    }
}
