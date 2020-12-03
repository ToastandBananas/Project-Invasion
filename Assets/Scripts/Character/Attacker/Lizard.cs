using UnityEngine;

public class Lizard : MonoBehaviour
{
    Attacker attackerScript;

    void Start()
    {
        attackerScript = GetComponent<Attacker>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == attackerScript.currentTarget)
            attackerScript.Attack();
    }
}
