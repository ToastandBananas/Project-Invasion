using UnityEngine;

public class Archer : MonoBehaviour
{
    Defender defenderScript;
    Shooter shooterScript;
    Health health;
    SquadData squadData;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        shooterScript = GetComponent<Shooter>();
        health = GetComponent<Health>();
        squadData = GameManager.instance.squadData;

        SetArcherData();
        health.SetCurrentHealthToMaxHealth();
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;
        
        if (defenderScript.squad.shouldRetreatWhenEnemyNear == false && defenderScript.isAttacking == false && defenderScript.targetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.targetAttacker.transform.position) <= defenderScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }

    public void SetArcherData()
    {
        defenderScript.squad.shouldRetreatWhenEnemyNear = squadData.archerShouldRetreatWhenEnemyNear;
        shooterScript.secondaryRangedDamageMultiplier += squadData.archerFireArrowsDamageMultiplier;

        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (squadData.archerLeaderHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.archerLeaderHealth);
            if (squadData.archerLeaderPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.archerLeaderPiercingDamage, 0);
            if (squadData.archerLeaderRangedPiercingDamage > 0)
                shooterScript.SetRangedDamage(0, squadData.archerLeaderRangedPiercingDamage, 0);
            if (squadData.archerLeaderAccuracy > 0)
                shooterScript.accuracy += squadData.archerLeaderAccuracy;
        }
        else // Unit:
        {
            if (squadData.archerHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.archerHealth);
            if (squadData.archerPiercingDamage > 0)
                defenderScript.SetAttackDamage(0, 0, squadData.archerPiercingDamage, 0);
            if (squadData.archerRangedPiercingDamage > 0)
                shooterScript.SetRangedDamage(0, squadData.archerRangedPiercingDamage, 0);
            if (squadData.archerAccuracy > 0)
                shooterScript.accuracy += squadData.archerAccuracy;
        }
    }
}
