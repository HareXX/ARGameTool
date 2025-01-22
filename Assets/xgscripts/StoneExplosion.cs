using UnityEngine;

public class StoneExplosion : MonoBehaviour
{
    public GameObject fracturedStonePrefab; // Ԥ��Ƭģ��
    public ParticleSystem explosionEffect;  // ����Ч��
    public float destroyDelay = 5f;         // ��Ƭ�����ӳ�

    public void Explode()
    {
        // ��ʾ����Ч��
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // ��ʾ��Ƭ
        if (fracturedStonePrefab != null)
        {
            GameObject fracturedStone = Instantiate(fracturedStonePrefab, transform.position, transform.rotation);
            Destroy(fracturedStone, destroyDelay); // �ӳ�������Ƭ
        }

        // ����ԭʼʯͷ
        Destroy(gameObject);
    }
}