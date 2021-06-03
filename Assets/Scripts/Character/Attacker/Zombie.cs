using UnityEngine;

public class Zombie : Enemy
{
    public float minResurrectWaitTime = 4f;
    public float maxResurrectWaitTime = 10f;

    bool hasBeenResurrected;

    public override void Update()
    {
        if (health.isDead)
        {
            if (hasBeenResurrected == false)
            {
                int random = Random.Range(0, 3);
                if (random == 0)
                {
                    hasBeenResurrected = true;
                    StartCoroutine(attackerScript.health.Resurrect(Random.Range(minResurrectWaitTime, maxResurrectWaitTime), this));
                }
                else
                    this.enabled = false;
            }
            else
                this.enabled = false;
        }

        if (attackerScript.isAttacking == false)
        {
            if (attackerScript.currentTargetDefender != null && Vector2.Distance(transform.position, attackerScript.currentTargetDefender.transform.position) <= attackerScript.minAttackDistance)
            {
                attackerScript.Attack();
            }
        }
    }
}