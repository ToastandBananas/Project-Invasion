using UnityEngine;

public class CastleCollider : MonoBehaviour
{
    CastleHealth castleHealth;

    void Start()
    {
        castleHealth = FindObjectOfType<CastleHealth>();
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        castleHealth.TakeHealth(1f);
        Destroy(otherCollider.gameObject);
    }
}
