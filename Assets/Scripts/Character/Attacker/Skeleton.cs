using UnityEngine;

public class Skeleton : MonoBehaviour
{
    Attacker attackerScript;
    Health health;

    void Start()
    {
        attackerScript = GetComponent<Attacker>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;

        if (attackerScript.isAttacking == false && attackerScript.currentDefenderAttacking != null
            && Vector2.Distance(transform.position, attackerScript.currentDefenderAttacking.transform.position) <= Mathf.Abs(attackerScript.attackOffset.x) + 0.05f)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            attackerScript.Attack();
        }
    }
}
