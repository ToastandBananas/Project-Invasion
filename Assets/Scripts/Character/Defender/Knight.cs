using UnityEngine;

public class Knight : MonoBehaviour
{
    Defender defenderScript;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == defenderScript.attackerBeingAttackedBy.gameObject)
            defenderScript.Attack();
    }
}
