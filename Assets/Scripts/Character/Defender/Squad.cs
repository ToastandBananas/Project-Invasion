using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [Header("Basic Info")]
    public SquadType squadType;
    [SerializeField] int goldCost = 100;
    public string description;

    int maxUnitCount;
    enum SquadFormation { Line, StaggeredLine, Wedge, Scattered }
    [Header("Squad Formation")]
    [SerializeField] SquadFormation squadFormation;

    [Header("Ranged Squads Only:")]
    public Squad castleWallVersionOfSquad;
    public bool shouldRetreatWhenEnemyNear;
    public bool isRangedUnit;
    public bool isCastleWallSquad;
    [SerializeField] int shootRange;

    [Header("Other")]
    public bool squadPlaced;

    [HideInInspector] public Defender leader;
    [HideInInspector] public List<Defender> units = new List<Defender>();
    [HideInInspector] public List<Attacker> attackersNearby = new List<Attacker>();

    [HideInInspector] public Transform leaderParent;
    [HideInInspector] public Transform unitsParent;
    [HideInInspector] public AttackerSpawner myLaneSpawner;
    [HideInInspector] public RangeCollider rangeCollider;
    [HideInInspector] public bool abilityActive;

    AbilityIconController abilityIconController;

    #region Formation Positions
    // Line formation positions:
    // 3 units
    Vector2[] leaderPositions_Line_Three = { new Vector2(-0.15f, 0) };
    Vector2[] unitPositions_Line_Three   = { new Vector2(0.05f, 0f), new Vector2(0.05f, 0.25f), new Vector2(0.05f, -0.25f) };
    // 4 units
    Vector2[] leaderPositions_Line_Four = { new Vector2(-0.15f, 0) };
    Vector2[] unitPositions_Line_Four   = { new Vector2(0.05f, 0.1f), new Vector2(0.05f, -0.1f), new Vector2(0.05f, 0.3f), new Vector2(0.05f, -0.3f) };

    // Wedge formation positions:
    // 3 units
    Vector2[] leaderPositions_Wedge_Three = { new Vector2(-0.15f, 0) };
    Vector2[] unitPositions_Wedge_Three   = { new Vector2(0.05f, 0f), new Vector2(0f, 0.25f), new Vector2(0f, -0.25f) };
    #endregion

    void Awake()
    {
        leaderParent = transform.GetChild(0);
        unitsParent = transform.GetChild(1);

        if (leaderParent.childCount > 0)
            leader = leaderParent.GetChild(0).GetComponent<Defender>();
        for (int i = 0; i < unitsParent.childCount; i++)
        {
            units.Add(unitsParent.GetChild(i).GetComponent<Defender>());
        }
    }

    void Start()
    {
        abilityIconController = AbilityIconController.instance;
        rangeCollider = GetComponentInChildren<RangeCollider>();

        maxUnitCount = units.Count;

        AssignUnitPositions();
        AssignLeaderPosition();
    }

    public void AssignUnitPositions()
    {
        if (squadFormation == SquadFormation.Line)
        {
            if (maxUnitCount == 3) AssignPositions(unitPositions_Line_Three);
            else if (maxUnitCount == 4) AssignPositions(unitPositions_Line_Four);
            else LogFormationError();
        }
        else if (squadFormation == SquadFormation.StaggeredLine)
        {
            LogFormationError();
        }
        else if (squadFormation == SquadFormation.Wedge)
        {
            if (maxUnitCount == 3) AssignPositions(unitPositions_Wedge_Three);
            else LogFormationError();
        }
        else if (squadFormation == SquadFormation.Scattered)
        {
            LogFormationError();
        }
    }

    public void AssignLeaderPosition()
    {
        if (leader != null)
        {
            if (squadFormation == SquadFormation.Line)
            {
                if (maxUnitCount == 3) leader.unitPosition = leaderPositions_Line_Three[0];
                if (maxUnitCount == 4) leader.unitPosition = leaderPositions_Line_Four[0];
                else LogFormationError();
            }
            else if (squadFormation == SquadFormation.StaggeredLine)
            {
                LogFormationError();
            }
            else if (squadFormation == SquadFormation.Wedge)
            {
                if (maxUnitCount == 3) leader.unitPosition = leaderPositions_Wedge_Three[0];
                else LogFormationError();
            }
            else if (squadFormation == SquadFormation.Scattered)
            {
                LogFormationError();
            }
        }
    }

    void AssignPositions(Vector2[] formationPositionsArray)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].unitPosition = formationPositionsArray[i];
        }
    }

    void LogFormationError()
    {
        Debug.LogError("Formation positions for this unit size do no exist. Create them in the Squad script...");
    }

    public void SetLaneSpawner()
    {
        AttackerSpawner[] attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            bool isCloseEnough = (Mathf.Abs(spawner.transform.position.y - transform.position.y) <= Mathf.Epsilon);

            if (isCloseEnough) myLaneSpawner = spawner;
        }
    }

    public void Retreat()
    {
        // Retreat all units (likely because the squad leader died)
        DefenderSpawner.instance.RemoveCell(transform.position);

        // First clear out current target data for attackers who are attacking this squad
        for (int i = 0; i < myLaneSpawner.transform.childCount; i++)
        {
            Attacker attacker = myLaneSpawner.transform.GetChild(i).GetComponent<Attacker>();
            if (attacker != null && attacker.currentTargetsSquad == this)
            {
                attacker.currentDefenderAttacking = null;
                attacker.currentTargetsHealth = null;
                attacker.currentTargetsSquad = null;
                attacker.opponents.Clear();
                attacker.StopAttacking();
            }
        }

        GetComponent<BoxCollider2D>().enabled = false;
        attackersNearby.Clear();
        if (rangeCollider != null)
            rangeCollider.attackersInRange.Clear();

        // Run back to the castle
        if (leader != null)
            StartCoroutine(leader.Retreat());

        foreach (Defender unit in units)
        {
            StartCoroutine(unit.Retreat());
        }
    }

    public void SetSortingOrder(int sortingOrder)
    {
        if (leader != null)
            leader.sr.sortingOrder = sortingOrder;
        for (int i = 0; i < units.Count; i++)
        {
            units[i].sr.sortingOrder = sortingOrder;
        }
    }

    public int GetGoldCost()
    {
        return goldCost;
    }

    public void SetGoldCost(int newGoldCost)
    {
        goldCost = newGoldCost;
    }

    public int GetShootRange()
    {
        return shootRange;
    }

    public void SetShootRange(int newShootRange)
    {
        shootRange = newShootRange;
    }

    void OnMouseEnter()
    {
        abilityIconController.selectedSquad = this;
        abilityIconController.EnableAbilityIcons();
    }

    void OnMouseExit()
    {
        if (abilityIconController.selectedSquad = this)
        {
            abilityIconController.selectedSquad = null;
            abilityIconController.DisableAbilityIcons();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCastleWallSquad == false)
        {
            if (collision.TryGetComponent<Attacker>(out Attacker attacker))
            {
                if (shouldRetreatWhenEnemyNear)
                    Retreat();
                else
                {
                    attackersNearby.Add(attacker);
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

                        if (attacker.currentDefenderAttacking == null) // If the attacker's current target is still null at this point (because each unit already has a target), attack any random defender
                        {
                            int random = Random.Range(0, units.Count + 1); // We add one to account for the leader

                            if (random == units.Count) // Attack the leader of the squad
                            {
                                attacker.opponents.Add(leader);
                                attacker.currentDefenderAttacking = leader;
                                if (leader.targetAttacker == null) // If the leader doesn't have a target, set its target to the attacker
                                    leader.targetAttacker = attacker;
                            }
                            else // Attack a random unit from the squad
                            {
                                attacker.opponents.Add(units[random]);
                                attacker.currentDefenderAttacking = units[random];
                                if (units[random].targetAttacker == null) // If the unit doesn't have a target, set its target to the attacker
                                    units[random].targetAttacker = attacker;
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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isCastleWallSquad == false)
        {
            if (collision.TryGetComponent<Attacker>(out Attacker attacker))
            {
                if (attackersNearby.Contains(attacker))
                    attackersNearby.Remove(attacker);

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
}
