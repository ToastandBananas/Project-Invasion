using UnityEngine;

public class Archer : Ally
{
    public override void Start()
    {
        base.Start();

        SetArcherData();
    }

    public override void FixedUpdate()
    {
        if (defenderScript.squad.shouldRetreatWhenEnemyNear == false && defenderScript.isAttacking == false && defenderScript.currentTargetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.currentTargetAttacker.transform.position) <= defenderScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }

    public void SetArcherData()
    {
        defenderScript.squad.shouldRetreatWhenEnemyNear = squadData.archerShouldRetreatWhenEnemyNear;
        defenderScript.myShooter.secondaryRangedDamageMultiplier += squadData.archerFireArrowsDamageMultiplier;

        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (squadData.archerLeaderHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.archerLeaderHealth);
            if (squadData.archerLeaderPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.archerLeaderPiercingDamage, 0);
            if (squadData.archerLeaderRangedPiercingDamage > 0)
                defenderScript.myShooter.SetRangedDamage(0, squadData.archerLeaderRangedPiercingDamage, 0);
            if (squadData.archerLeaderAccuracy > 0)
                defenderScript.myShooter.accuracy += squadData.archerLeaderAccuracy;
        }
        else // Unit:
        {
            if (squadData.archerHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.archerHealth);
            if (squadData.archerPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.archerPiercingDamage, 0);
            if (squadData.archerRangedPiercingDamage > 0)
                defenderScript.myShooter.SetRangedDamage(0, squadData.archerRangedPiercingDamage, 0);
            if (squadData.archerAccuracy > 0)
                defenderScript.myShooter.accuracy += squadData.archerAccuracy;
        }

        defenderScript.health.SetCurrentHealthToMaxHealth();
    }
}
