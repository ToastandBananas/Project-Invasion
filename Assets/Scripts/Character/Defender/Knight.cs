using UnityEngine;

public class Knight : MonoBehaviour
{
    Defender defenderScript;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
    }

    void Update()
    {
        if (defenderScript.isAttacking == false && defenderScript.targetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.targetAttacker.transform.position) <= Mathf.Abs(defenderScript.attackOffset.x) + 0.05f)
        {
            Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == defenderScript.attackerBeingAttackedBy.gameObject)
            defenderScript.Attack();
    }*/
}
