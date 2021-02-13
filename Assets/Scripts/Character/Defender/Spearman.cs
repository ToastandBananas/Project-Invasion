using UnityEngine;

public class Spearman : MonoBehaviour
{
    Defender defenderScript;
    Shooter shooterScript;
    Health health;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        shooterScript = GetComponent<Shooter>();
        health = GetComponent<Health>();

        SetSpearmenData();
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;

        if (defenderScript.isAttacking == false && defenderScript.targetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.targetAttacker.transform.position) <= defenderScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }

    public void SetSpearmenData()
    {
        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (SquadData.spearmenLeaderHealth > 0)
                health.SetHealth(SquadData.spearmenLeaderHealth);
            if (SquadData.spearmenLeaderMeleeDamage > 0)
                defenderScript.SetAttackDamage(SquadData.spearmenLeaderMeleeDamage);
            if (SquadData.spearmenLeaderRangedDamage > 0)
                shooterScript.SetShootDamage(SquadData.spearmenLeaderRangedDamage);
            if (SquadData.spearmenLeaderAccuracy > 0)
                shooterScript.SetShootAccuracy(SquadData.spearmenLeaderAccuracy);
        }
        else // Unit:
        {
            if (SquadData.spearmenHealth > 0)
                health.SetHealth(SquadData.spearmenHealth);
            if (SquadData.spearmenMeleeDamage > 0)
                defenderScript.SetAttackDamage(SquadData.spearmenMeleeDamage);
            if (SquadData.spearmenRangedDamage > 0)
                shooterScript.SetShootDamage(SquadData.spearmenRangedDamage);
            if (SquadData.spearmenAccuracy > 0)
                shooterScript.SetShootAccuracy(SquadData.spearmenAccuracy);
        }
    }
}
