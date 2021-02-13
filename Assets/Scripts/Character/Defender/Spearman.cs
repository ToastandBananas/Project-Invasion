using UnityEngine;

public class Spearman : MonoBehaviour
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

        SetSpearmenData();
        health.SetCurrentHealthToMaxHealth();
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
            if (squadData.spearmenLeaderHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.spearmenLeaderHealth);
            if (squadData.spearmenLeaderMeleeDamage > 0)
                defenderScript.SetAttackDamage(defenderScript.GetAttackDamage() + squadData.spearmenLeaderMeleeDamage);
            if (squadData.spearmenLeaderRangedDamage > 0)
                shooterScript.SetShootDamage(shooterScript.GetShootDamage() + squadData.spearmenLeaderRangedDamage);
            if (squadData.spearmenLeaderAccuracy > 0)
                shooterScript.SetShootAccuracy(shooterScript.GetShootAccuracy() + squadData.spearmenLeaderAccuracy);
        }
        else // Unit:
        {
            if (squadData.spearmenHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.spearmenHealth);
            if (squadData.spearmenMeleeDamage > 0)
                defenderScript.SetAttackDamage(defenderScript.GetAttackDamage() + squadData.spearmenMeleeDamage);
            if (squadData.spearmenRangedDamage > 0)
                shooterScript.SetShootDamage(shooterScript.GetShootDamage() + squadData.spearmenRangedDamage);
            if (squadData.spearmenAccuracy > 0)
                shooterScript.SetShootAccuracy(shooterScript.GetShootAccuracy() + squadData.spearmenAccuracy);
        }
    }
}
