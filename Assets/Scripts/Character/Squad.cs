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
    
    public int attackerCount = 0;
    public List<Attacker> attackersInRange;

    // Squads with a max of 12 units
    Vector2[] twelveLeaderPositions = { new Vector2(-0.3f, 0), new Vector2(-0.1f, 0), new Vector2(0.1f, 0) };
    Vector2[] twelveUnitPositions   = { new Vector2(0.25f, 0.1f), new Vector2(0.25f, -0.1f), new Vector2(0.25f, 0.3f), new Vector2(0.25f, -0.3f),
                                        new Vector2(0.05f, 0.1f), new Vector2(0.05f, -0.1f), new Vector2(0.05f, 0.3f), new Vector2(0.05f, -0.3f),
                                        new Vector2(-0.15f, 0.1f), new Vector2(-0.15f, -0.1f), new Vector2(-0.15f, 0.3f), new Vector2(-0.15f, -0.3f) };

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
        if (maxSquadSize == MaxSquadSize.Twelve)
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].unitPosition = twelveUnitPositions[i];
            }
        }
    }

    public void AssignLeaderPosition()
    {
        if (maxSquadSize == MaxSquadSize.Twelve)
        {
            if (units.Count > 8)
                leader.unitPosition = twelveLeaderPositions[0];
            else if (units.Count > 4)
                leader.unitPosition = twelveLeaderPositions[1];
            else
                leader.unitPosition = twelveLeaderPositions[2];
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
            attackerCount++;
            attackersInRange.Add(attacker);
            attacker.currentTargetsSquad = this;

            int totalDefendersAttackingAttacker = 0;
            foreach (Defender defender in units)
            {
                if (totalDefendersAttackingAttacker == attacker.maxOpponents)
                    return;

                if (defender.targetAttacker == null && totalDefendersAttackingAttacker == 0)
                {
                    // Send in the first defender
                    defender.targetAttacker = attacker;
                    attacker.currentTarget = defender.gameObject;
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
        }
    }
}
