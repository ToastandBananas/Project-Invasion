using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] int goldCost = 100;

    // Does not count the leader, unless it's a squad size of One
    enum MaxSquadSize { Sixteen, Fifteen, Twelve, Nine, Eight, Seven, Five, Four, Three, Two, One }
    [SerializeField] MaxSquadSize maxSquadSize;

    public Transform leaderParent, unitsParent;
    public List<Defender> units;
    public Defender leader;
    
    public List<Attacker> attackersInRange;

    // Squads with a max of 12 units
    Vector2[] fourLeaderPositions = { new Vector2(-0.15f, 0)};
    Vector2[] fourUnitPositions   = { new Vector2(0.05f, 0.1f), new Vector2(0.05f, -0.1f), new Vector2(0.05f, 0.3f), new Vector2(0.05f, -0.3f) };

    void Start()
    {
        leaderParent = transform.GetChild(0);
        unitsParent = transform.GetChild(1);
        units = new List<Defender>();
        attackersInRange = new List<Attacker>();

        leader = leaderParent.GetChild(0).GetComponent<Defender>();
        for (int i = 0; i < unitsParent.childCount; i++)
        {
            units.Add(unitsParent.GetChild(i).GetComponent<Defender>());
        }

        AssignUnitPositions();
        AssignLeaderPosition();
    }

    public int GetGoldCost()
    {
        return goldCost;
    }

    public void AssignUnitPositions()
    {
        if (maxSquadSize == MaxSquadSize.Four)
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].unitPosition = fourUnitPositions[i];
            }
        }
    }

    public void AssignLeaderPosition()
    {
        if (leader != null)
        {
            if (maxSquadSize == MaxSquadSize.Four)
                leader.unitPosition = fourLeaderPositions[0];
        }
    }

    public void Retreat()
    {
        // Retreat all units (likely because the squad leader died)
        // TODO
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker attacker))
        {
            attackersInRange.Add(attacker);
            attacker.currentTargetsSquad = this;

            int totalDefendersAttackingAttacker = 0;
            if (units.Count > 0)
            {
                foreach (Defender defender in units)
                {
                    if (totalDefendersAttackingAttacker == attacker.maxOpponents)
                        return;

                    if (defender.targetAttacker == null && totalDefendersAttackingAttacker == 0)
                    {
                        // Send in the first defender
                        defender.targetAttacker = attacker;
                        attacker.currentDefenderAttacking = defender;
                        attacker.opponents.Add(defender);
                        totalDefendersAttackingAttacker++;
                    }
                    else if (defender.targetAttacker == null && totalDefendersAttackingAttacker > 0 && totalDefendersAttackingAttacker < attacker.maxOpponents)
                    {
                        // Send in the maximum defenders possible per the attacker type
                        defender.targetAttacker = attacker;
                        attacker.opponents.Add(defender);
                        totalDefendersAttackingAttacker++;
                    }
                }

                if (attacker.currentDefenderAttacking == null) // If the attacker's current target is still null at this point, attack a random defender
                {
                    int random = Random.Range(0, units.Count + 1);

                    if (random == units.Count) // Attack the leader of the squad
                    {
                        attacker.opponents.Add(leader);
                        attacker.currentDefenderAttacking = leader;
                    }
                    else // Attack a random unit from the squad
                    {
                        attacker.opponents.Add(units[random]);
                        attacker.currentDefenderAttacking = units[random];
                    }
                }
            }
            else
            {
                if (leader.targetAttacker == null)
                    leader.targetAttacker = attacker;
                
                attacker.currentDefenderAttacking = leader;
                attacker.opponents.Add(leader);
                totalDefendersAttackingAttacker++;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker attacker))
        {
            if (attackersInRange.Contains(attacker))
                attackersInRange.Remove(attacker);

            if (attacker.currentTargetsSquad = this)
                attacker.currentTargetsSquad = null;

            if (leader != null && leader.targetAttacker != null && leader.targetAttacker == attacker)
                leader.targetAttacker.health.FindNewTargetForOpponent();
            else
            {
                if (units.Count > 0)
                {
                    foreach (Defender unit in units)
                    {
                        if (unit.targetAttacker != null && unit.targetAttacker == attacker)
                            unit.targetAttacker.health.FindNewTargetForOpponent();
                    }
                }
            }
        }
    }
}
