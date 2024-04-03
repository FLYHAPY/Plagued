using UnityEngine;

public class PlayerRepulsion : MonoBehaviour
{
    public float repulsionForce = 10f; // For�a da repuls�o
    public float repulsionRadius = 5f; // Raio da repuls�o

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CreateRepulsion();
        }
    }

    void CreateRepulsion()
    {
        // Encontre todos os inimigos na �rea de repuls�o
        Collider[] colliders = Physics.OverlapSphere(transform.position, repulsionRadius);
        foreach (Collider collider in colliders)
        {
            // Verifique se o collider pertence a um inimigo
            if (collider.CompareTag("Enemy"))
            {
                // Calcule a dire��o do inimigo em rela��o ao jogador
                Vector3 direction = (collider.transform.position - transform.position).normalized;

                // Aplique uma for�a de repuls�o ao inimigo na dire��o oposta
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction * repulsionForce, ForceMode.Impulse);
                }
            }
        }
    }
}
