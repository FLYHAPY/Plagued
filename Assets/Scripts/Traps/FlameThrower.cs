using UnityEngine;

public class FlamethrowerTrap : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab da bala
    public float fireRate = 1f; // Taxa de disparo em segundos
    public float detectionRange = 20f; // Alcance da detec��o

    void Update()
    {
        // Verifique se o jogador est� dentro do alcance de detec��o
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
        // Instancie a bala na posi��o e orienta��o do lan�ador de chamas
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
