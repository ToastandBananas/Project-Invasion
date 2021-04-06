using UnityEngine;

public class Knight : MonoBehaviour
{
    Defender defenderScript;
    Health health;
    SquadData squadData;

    void Awake()
    {
        defenderScript = GetComponent<Defender>();
        health = GetComponent<Health>();
        squadData = GameManager.instance.squadData;

        SetKnightData();
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

    public void SetKnightData()
    {
        health.thornsDamageMultiplier = squadData.knightThornsDamageMultiplier;

        // Leader:
        if (defenderScript.squad.leader == defenderScript)
        {
            if (squadData.knightLeaderHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.knightLeaderHealth);
            if (squadData.knightLeaderSlashDamage > 0)
                defenderScript.SetAttackDamage(0, squadData.knightLeaderSlashDamage, 0, 0);
        }
        else // Unit:
        {
            if (squadData.knightHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.knightHealth);
            if (squadData.knightSlashDamage > 0)
                defenderScript.SetAttackDamage(0, squadData.knightSlashDamage, 0, 0);
        }

        health.SetCurrentHealthToMaxHealth();
    }
}
