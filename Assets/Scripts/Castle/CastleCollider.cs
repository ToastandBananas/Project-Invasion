using UnityEngine;

public class CastleCollider : MonoBehaviour
{
    CastleHealth castleHealth;

    void Start()
    {
        castleHealth = FindObjectOfType<CastleHealth>();
    }

    void OnTriggerEnter2D()
    {
        castleHealth.TakeHealth(1);
    }
}
