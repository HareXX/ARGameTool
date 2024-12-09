using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackAnchor : MonoBehaviour
{
    
    private Vector3 previousPosition;
    private float speed;
    private float checkInterval = 0.2f;
    private float timer;

    void Start()
    {
        previousPosition = transform.position;
        timer = checkInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Calculate speed
            speed = Vector3.Distance(previousPosition, transform.position) / checkInterval;
            previousPosition = transform.position;
            timer = checkInterval;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health == null) return;
        Debug.Log("attack speed: " + speed);
        health.Attack(100);


    }
}
