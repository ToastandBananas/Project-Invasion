using UnityEngine;

public class Lizard : MonoBehaviour
{
    Attacker attackerScript;

    void Start()
    {
        attackerScript = GetComponent<Attacker>();
    }

    void Update()
    {
        if (attackerScript.isAttacking == false && attackerScript.currentDefenderAttacking != null
            && Vector2.Distance(transform.position, attackerScript.currentDefenderAttacking.transform.position) <= 0.125f)//Mathf.Abs(attackerScript.currentDefenderAttacking.attackOffset.x))
        {
            attackerScript.Attack();
        }
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == attackerScript.currentTarget)
            attackerScript.Attack();
    }*/
}
