using UnityEngine;

public class PlayerRepulsion : MonoBehaviour
{
    public float repulsionForce = 10f; // Força da repulsão
    public float repulsionRadius = 5f; // Raio da repulsão

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CreateRepulsion();
        }
    }

    void CreateRepulsion()
    {
        // Encontre todos os inimigos na área de repulsão
        Collider[] colliders = Physics.OverlapSphere(transform.position, repulsionRadius);
        foreach (Collider collider in colliders)
        {
            // Verifique se o collider pertence a um inimigo
            if (collider.CompareTag("Enemy"))
            {
                // Calcule a direção do inimigo em relação ao jogador
                Vector3 direction = (collider.transform.position - transform.position).normalized;

                // Aplique uma força de repulsão ao inimigo na direção oposta
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction * repulsionForce, ForceMode.Impulse);
                }
            }
        }
    }
}
