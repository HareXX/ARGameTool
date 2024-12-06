using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    GameObject fire;
    GameObject explosion;
    GameObject bomb;
    void Start()
    {
        fire = transform.Find("fire").gameObject;
        explosion = transform.Find("explosion").gameObject;
        bomb = transform.Find("bomb").gameObject;
        //Invoke("setExplosion", 2f);
    }

    void Update()
    {
        
    }
    void setExplosion()
    {
        fire.SetActive(true);
        Invoke("setExplosionBegin", 2f);
    }
    void setExplosionBegin()
    {
        explosion.SetActive(true);
        bomb.SetActive(false);
        fire.SetActive(false);
        Destroy(gameObject, 4f);
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.name == "fire")
        {
            Invoke("setExplosion", 0.5f);
        }
    }
}
