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
        GameObject otherObject = collision.gameObject;

        if (otherObject.GetComponent<Defender>())
            attackerScript.Attack(otherObject);
    }
}
