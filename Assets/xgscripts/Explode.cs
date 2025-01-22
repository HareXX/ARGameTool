using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    GameObject fire;
    GameObject explosion;
    GameObject bomb;
    public float explosionRadius = 5f; // 爆炸范围
    public float explosionForce = 700f; // 爆炸力

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
        // 在炸弹位置创建一个球形范围，检测范围内的所有碰撞体
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // 检查物体是否有刚体
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 添加爆炸力
                //rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                // 如果是石头，调用石头的爆炸逻辑
                StoneExplosion stone = hit.GetComponent<StoneExplosion>();
                if (stone != null)
                {
                    stone.Explode();
                }
            }
        }

        // 添加爆炸特效
        explosion.SetActive(true);
        bomb.SetActive(false);
        fire.SetActive(false);
        Destroy(gameObject, 4f);
    }
    private void OnDrawGizmos()
    {
        // 可视化爆炸范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.name == "fire")
        {
            Invoke("setExplosion", 0.5f);
        }
    }
}
