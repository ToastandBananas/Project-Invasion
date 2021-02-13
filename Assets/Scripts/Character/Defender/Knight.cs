using UnityEngine;

public class Knight : MonoBehaviour
{
    Defender defenderScript;
    Health health;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        health = GetComponent<Health>();

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
        // Leader:
        if (defenderScript.squad.leader == defenderScript)
        {
            if (SquadData.knightLeaderHealth > 0)
                health.SetHealth(SquadData.knightLeaderHealth);
            if (SquadData.knightLeaderMeleeDamage > 0)
                defenderScript.SetAttackDamage(SquadData.knightLeaderMeleeDamage);
        }
        else // Unit:
        {
            if (SquadData.knightHealth > 0)
                health.SetHealth(SquadData.knightHealth);
            if (SquadData.knightMeleeDamage > 0)
                defenderScript.SetAttackDamage(SquadData.knightMeleeDamage);
        }
    }
}
