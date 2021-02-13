using UnityEngine;

public class Archer : MonoBehaviour
{
    Defender defenderScript;
    Shooter shooterScript;
    Health health;

    public bool fireArrowsUnlocked;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        shooterScript = GetComponent<Shooter>();
        health = GetComponent<Health>();

        SetArcherData();
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
        defenderScript.squad.shouldRetreatWhenEnemyNear = SquadData.archerShouldRetreatWhenEnemyNear;
        fireArrowsUnlocked = SquadData.archerFireArrowsUnlocked;

        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (SquadData.archerLeaderHealth > 0)
                health.SetHealth(SquadData.archerLeaderHealth);
            if (SquadData.archerLeaderMeleeDamage > 0)
                defenderScript.SetAttackDamage(SquadData.archerLeaderMeleeDamage);
            if (SquadData.archerLeaderRangedDamage > 0)
                shooterScript.SetShootDamage(SquadData.archerLeaderRangedDamage);
            if (SquadData.archerLeaderAccuracy > 0)
                shooterScript.SetShootAccuracy(SquadData.archerLeaderAccuracy);
        }
        else // Unit:
        {
            if (SquadData.archerHealth > 0)
                health.SetHealth(SquadData.archerHealth);
            if (SquadData.archerMeleeDamage > 0)
                defenderScript.SetAttackDamage(SquadData.archerMeleeDamage);
            if (SquadData.archerRangedDamage > 0)
                shooterScript.SetShootDamage(SquadData.archerRangedDamage);
            if (SquadData.archerAccuracy > 0)
                shooterScript.SetShootAccuracy(SquadData.archerAccuracy);
        }
    }
}
