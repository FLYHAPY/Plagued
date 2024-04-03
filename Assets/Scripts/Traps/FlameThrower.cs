using UnityEngine;

public class FlamethrowerTrap : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab da bala
    public float fireRate = 1f; // Taxa de disparo em segundos
    public float detectionRange = 20f; // Alcance da detecção

    void Update()
    {
        // Verifique se o jogador está dentro do alcance de detecção
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                ShootAtPlayer();
                break;
            }
        }
    }

    void ShootAtPlayer()
    {
        // Instancie a bala na posição e orientação do lançador de chamas
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
