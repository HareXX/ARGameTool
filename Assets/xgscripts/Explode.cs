using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    GameObject fire;
    GameObject explosion;
    GameObject bomb;
    public float explosionRadius = 5f; // ��ը��Χ
    public float explosionForce = 700f; // ��ը��

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
        // ��ը��λ�ô���һ�����η�Χ����ⷶΧ�ڵ�������ײ��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // ��������Ƿ��и���
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // ��ӱ�ը��
                //rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                // �����ʯͷ������ʯͷ�ı�ը�߼�
                StoneExplosion stone = hit.GetComponent<StoneExplosion>();
                if (stone != null)
                {
                    stone.Explode();
                }
            }
        }

        // ��ӱ�ը��Ч
        explosion.SetActive(true);
        bomb.SetActive(false);
        fire.SetActive(false);
        Destroy(gameObject, 4f);
    }
    private void OnDrawGizmos()
    {
        // ���ӻ���ը��Χ
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
