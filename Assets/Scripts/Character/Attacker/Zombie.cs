using UnityEngine;

public class Zombie : MonoBehaviour
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

        if (attackerScript.isAttacking == false)
        {
            if (attackerScript.currentTargetDefender != null && Vector2.Distance(transform.position, attackerScript.currentTargetDefender.transform.position) <= attackerScript.minAttackDistance)
            {
                attackerScript.Attack();
            }
        }
    }
}
