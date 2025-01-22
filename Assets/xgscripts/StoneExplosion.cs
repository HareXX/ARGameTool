using UnityEngine;

public class StoneExplosion : MonoBehaviour
{
    public GameObject fracturedStonePrefab; // 预碎片模型
    public ParticleSystem explosionEffect;  // 粒子效果
    public float destroyDelay = 5f;         // 碎片销毁延迟

    public void Explode()
    {
        // 显示粒子效果
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 显示碎片
        if (fracturedStonePrefab != null)
        {
            GameObject fracturedStone = Instantiate(fracturedStonePrefab, transform.position, transform.rotation);
            Destroy(fracturedStone, destroyDelay); // 延迟销毁碎片
        }

        // 销毁原始石头
        Destroy(gameObject);
    }
}