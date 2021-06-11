using System.Collections;
using UnityEngine;

public class FatZombie : Zombie
{
    bool isEating;

    public override void Update()
    {
        if (health.isDead)
            TryReanimate();

        if (isEating == false)
        {
            if (attackerScript.currentTargetDefender != null && Vector2.Distance(transform.position, attackerScript.currentTargetDefender.transform.position) <= attackerScript.minAttackDistance)
                StartCoroutine(TackleAndEat(attackerScript.currentTargetDefender));
        }

        if (attackerScript.currentTargetDefender != null && attackerScript.currentTargetDefender.isRetreating)
        {
            if (attackerScript.opponents.Contains(attackerScript.currentTargetDefender))
                attackerScript.opponents.Remove(attackerScript.currentTargetDefender);

            attackerScript.GetNewTarget();
        }
    }

    IEnumerator TackleAndEat(Defender targetDefender)
    {
        isEating = true;
        Vector3 targetPos = targetDefender.transform.position;

        attackerScript.anim.Play("TackleAndEat", 0);
        targetDefender.health.InstaKill();

        // Move to the target's position, so that the zombie is on top of them
        while (transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
            yield return null;
        }
    }

    public void StopEating()
    {
        isEating = false;
    }
}
