using UnityEngine;

public class Archer : MonoBehaviour
{
    Defender defenderScript;
    Shooter shooterScript;
    Health health;
    SquadData squadData;

    public bool fireArrowsUnlocked;

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
        fireArrowsUnlocked = squadData.archerFireArrowsUnlocked;
        shooterScript.SetSecondaryRangedDamageMultiplier(shooterScript.GetSecondaryRangedDamageMultiplier() + squadData.fireArrowsDamageMultiplier);

        if (defenderScript.squad.leader == defenderScript)
        {
            // Leader:
            if (squadData.archerLeaderHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.archerLeaderHealth);
            if (squadData.archerLeaderMeleeDamage > 0)
                defenderScript.SetAttackDamage(defenderScript.GetMeleeDamage() + squadData.archerLeaderMeleeDamage);
            if (squadData.archerLeaderRangedDamage > 0)
                shooterScript.SetShootDamage(shooterScript.GetRangedDamage() + squadData.archerLeaderRangedDamage);
            if (squadData.archerLeaderAccuracy > 0)
                shooterScript.SetShootAccuracy(shooterScript.GetRangedAccuracy() + squadData.archerLeaderAccuracy);
        }
        else // Unit:
        {
            if (squadData.archerHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.archerHealth);
            if (squadData.archerMeleeDamage > 0)
                defenderScript.SetAttackDamage(defenderScript.GetMeleeDamage() + squadData.archerMeleeDamage);
            if (squadData.archerRangedDamage > 0)
                shooterScript.SetShootDamage(shooterScript.GetRangedDamage() + squadData.archerRangedDamage);
            if (squadData.archerAccuracy > 0)
                shooterScript.SetShootAccuracy(shooterScript.GetRangedAccuracy() + squadData.archerAccuracy);
        }
    }
}
