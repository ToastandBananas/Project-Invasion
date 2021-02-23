using UnityEngine;

public class Knight : MonoBehaviour
{
    Defender defenderScript;
    Health health;
    SquadData squadData;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        health = GetComponent<Health>();
        squadData = GameManager.instance.squadData;

        SetKnightData();
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

    public void SetKnightData()
    {
        // Leader:
        if (defenderScript.squad.leader == defenderScript)
        {
            if (squadData.knightLeaderHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.knightLeaderHealth);
            if (squadData.knightLeaderMeleeDamage > 0)
                defenderScript.SetAttackDamage(defenderScript.GetMeleeDamage() + squadData.knightLeaderMeleeDamage);
        }
        else // Unit:
        {
            if (squadData.knightHealth > 0)
                health.SetMaxHealth(health.GetMaxHealth() + squadData.knightHealth);
            if (squadData.knightMeleeDamage > 0)
                defenderScript.SetAttackDamage(defenderScript.GetMeleeDamage() + squadData.knightMeleeDamage);
        }
    }
}
