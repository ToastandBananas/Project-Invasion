// using UnityEngine;

public class Spearman : Ally
{
    public override void Start()
    {
        base.Start();

        SetSpearmenData();
    }

    public void SetSpearmenData()
    {
        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (squadData.spearmenLeaderHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.spearmenLeaderHealth);
            if (squadData.spearmenLeaderPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.spearmenLeaderPiercingDamage, 0);
            if (squadData.spearmenLeaderRangedPiercingDamage > 0)
                defenderScript.myShooter.SetRangedDamage(0, squadData.spearmenLeaderPiercingDamage, 0);
            if (squadData.spearmenLeaderAccuracy > 0)
                defenderScript.myShooter.accuracy += squadData.spearmenLeaderAccuracy;
        }
        else // Unit:
        {
            if (squadData.spearmenHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.spearmenHealth);
            if (squadData.spearmenPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.spearmenPiercingDamage, 0);
            if (squadData.spearmenRangedPiercingDamage > 0)
                defenderScript.myShooter.SetRangedDamage(0, squadData.spearmenRangedPiercingDamage, 0);
            if (squadData.spearmenAccuracy > 0)
                defenderScript.myShooter.accuracy += squadData.spearmenAccuracy;
        }

        defenderScript.health.SetCurrentHealthToMaxHealth();
    }
}
