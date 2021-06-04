// using UnityEngine;

public class Knight : Ally
{
    public override void Start()
    {
        base.Start();

        SetKnightData();
    }

    public void SetKnightData()
    {
        defenderScript.health.thornsDamageMultiplier = squadData.knightThornsDamageMultiplier;

        // Leader:
        if (defenderScript.squad.leader == defenderScript)
        {
            if (squadData.knightLeaderHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.knightLeaderHealth);
            if (squadData.knightLeaderSlashDamage > 0)
                defenderScript.SetAttackDamage(0, squadData.knightLeaderSlashDamage, 0, 0);
        }
        else // Unit:
        {
            if (squadData.knightHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.knightHealth);
            if (squadData.knightSlashDamage > 0)
                defenderScript.SetAttackDamage(0, squadData.knightSlashDamage, 0, 0);
        }

        defenderScript.health.SetCurrentHealthToMaxHealth();
    }
}
