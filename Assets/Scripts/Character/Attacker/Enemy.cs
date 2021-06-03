using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Attacker attackerScript;
    [HideInInspector] public Health health;

    void Awake()
    {
        TryGetComponent(out attackerScript);
        TryGetComponent(out health);
    }

    public virtual void Update()
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
        else if (attackerScript.currentTargetDefender != null && attackerScript.currentTargetDefender.isRetreating)
        {
            if (attackerScript.opponents.Contains(attackerScript.currentTargetDefender))
                attackerScript.opponents.Remove(attackerScript.currentTargetDefender);

            attackerScript.GetNewTarget();
        }
    }
}
