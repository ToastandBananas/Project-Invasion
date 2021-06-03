using UnityEngine;

public class Spearman : MonoBehaviour
{
    Defender defenderScript;
    Shooter shooterScript;
    Health health;
    SquadData squadData;

    void Awake()
    {
        defenderScript = GetComponent<Defender>();
        shooterScript = GetComponent<Shooter>();
        health = GetComponent<Health>();
        squadData = GameManager.instance.squadData;

        SetSpearmenData();
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;

        if (defenderScript.isAttacking == false && defenderScript.currentTargetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.currentTargetAttacker.transform.position) <= defenderScript.minAttackDistance)
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
            if (squadData.spearmenLeaderHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.spearmenLeaderHealth);
            if (squadData.spearmenLeaderPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.spearmenLeaderPiercingDamage, 0);
            if (squadData.spearmenLeaderRangedPiercingDamage > 0)
                shooterScript.SetRangedDamage(0, squadData.spearmenLeaderPiercingDamage, 0);
            if (squadData.spearmenLeaderAccuracy > 0)
                shooterScript.accuracy += squadData.spearmenLeaderAccuracy;
        }
        else // Unit:
        {
            if (squadData.spearmenHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.spearmenHealth);
            if (squadData.spearmenPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.spearmenPiercingDamage, 0);
            if (squadData.spearmenRangedPiercingDamage > 0)
                shooterScript.SetRangedDamage(0, squadData.spearmenRangedPiercingDamage, 0);
            if (squadData.spearmenAccuracy > 0)
                shooterScript.accuracy += squadData.spearmenAccuracy;
        }

        health.SetCurrentHealthToMaxHealth();
    }
}
