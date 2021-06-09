public class Priest : Ally
{
    public override void Start()
    {
        base.Start();

        SetPriestData();
    }

    public override void FixedUpdate()
    {
        // This is empty because we want to override the base FixedUpdate and prevent it from running
    }

    void SetPriestData()
    {
        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (squadData.priestLeaderHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.priestLeaderHealth);

            if (squadData.priestLeaderHealAmount > 0)
                defenderScript.myShooter.SetHealAmount(squadData.priestLeaderHealAmount);
        }
        else // Unit:
        {
            if (squadData.priestHealth > 0)
                defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.priestHealth);

            if (squadData.priestHealAmount > 0)
                defenderScript.myShooter.SetHealAmount(squadData.priestHealAmount);
        }

        defenderScript.health.SetCurrentHealthToMaxHealth();
    }
}
