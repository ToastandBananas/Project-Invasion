using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource Counts")]
    public int goldDepositCount;
    public int suppliesDepositCount;
    
    [Header("Spawn Locations")]
    public List<Vector2> possibleDepositSpawnLocations = new List<Vector2>();

    [HideInInspector] public Squad laborerSquadCurrentlyOnNode;
    [HideInInspector] public AttackerSpawner myLaneSpawner;

    [HideInInspector] public List<ResourceDeposit> resourceDeposits = new List<ResourceDeposit>();
    [HideInInspector] public List<ResourceDeposit> unoccupiedResourceDeposits = new List<ResourceDeposit>();
    // [HideInInspector] public List<Attacker> attackersAttackingNode = new List<Attacker>();

    DefenderSpawner defenderSpawner;
    GameAssets gameAssets;

    void Start()
    {
        SetLaneSpawner();

        defenderSpawner = DefenderSpawner.instance;
        gameAssets = GameAssets.instance;

        int resourceDepositCount = 0;

        for (int i = 0; i < goldDepositCount; i++)
        {
            ResourceDeposit goldDeposit = Instantiate(gameAssets.goldDeposit, transform);
            resourceDepositCount++;

            int randomIndex = Random.Range(0, possibleDepositSpawnLocations.Count);
            goldDeposit.transform.localPosition = possibleDepositSpawnLocations[randomIndex];
            possibleDepositSpawnLocations.Remove(possibleDepositSpawnLocations[randomIndex]);
        }

        //int resourceDepositCount = transform.childCount;
        
        for (int i = 0; i < resourceDepositCount; i++)
        {
            resourceDeposits.Add(transform.GetChild(i).GetComponent<ResourceDeposit>());
            unoccupiedResourceDeposits.Add(resourceDeposits[i]);
        }

        // Add this node to our list of nodes
        defenderSpawner.AddNode(transform.position);
        defenderSpawner.resourceNodes.Add(this);

        // Choose a random sprite
        for (int i = 0; i < resourceDeposits.Count; i++)
        {
            if (resourceDeposits[i].resourceType == ResourceType.Gold)
            {
                int randomIndex = Random.Range(0, gameAssets.goldDepositAnimatorControllers.Count);
                resourceDeposits[i].anim.runtimeAnimatorController = gameAssets.goldDepositAnimatorControllers[randomIndex];
                //gameAssets.goldDepositAnimatorControllers.Remove(gameAssets.goldDepositAnimatorControllers[randomIndex]);
            }

            int coinToss = Random.Range(0, 2);
            if (coinToss == 0)
                resourceDeposits[i].sr.flipX = false;
            else
                resourceDeposits[i].sr.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Attacker attacker);
        collision.TryGetComponent(out Defender defender);

        // If a laborer enters the square
        if (defender != null && defender.squad.squadType == SquadType.Laborers && defender.isRetreating == false)
        {
            Laborer laborer = defender.GetComponent<Laborer>();
            if (laborer.targetResourceDeposit == null && unoccupiedResourceDeposits.Count > 0)
            {
                // Choose a random deposit
                int randomIndex = Random.Range(0, unoccupiedResourceDeposits.Count);

                // Get a random x offset for the Laborer's new unit position we will be assigning it
                float coinToss = Random.Range(0, 2);
                float xOffset;
                if (coinToss == 0)
                    xOffset = unoccupiedResourceDeposits[randomIndex].miningXOffset;
                else
                    xOffset = -unoccupiedResourceDeposits[randomIndex].miningXOffset;

                // Set the Laborer's unit position to the left or right of the deposit
                defender.unitPosition = unoccupiedResourceDeposits[randomIndex].transform.localPosition + new Vector3(xOffset, 0);

                // Set the Laborer's target deposit to this one
                laborer.targetResourceDeposit = unoccupiedResourceDeposits[randomIndex];
                unoccupiedResourceDeposits[randomIndex].myLaborer = defender;

                //Set occupied to true for the deposit
                unoccupiedResourceDeposits[randomIndex].occupied = true;

                // Remove the deposit from our list of unoccupied deposits
                unoccupiedResourceDeposits.Remove(unoccupiedResourceDeposits[randomIndex]);

                laborerSquadCurrentlyOnNode = defender.squad;
            }
        }
        else if (attacker != null && attacker.canAttackNodes) // If an attacker who can attack resource nodes enters the square
        {
            // attackersAttackingNode.Add(attacker);

            // Set the Attacker's target Resource Node to this Node
            attacker.currentTargetNode = this;

            // Set the Attacker's target resource deposit to a random deposit within this Node
            AssignTargetToAttacker(attacker);
        }
    }

    public void AssignTargetToAttacker(Attacker attacker)
    {
        if (resourceDeposits.Count > 0)
            attacker.currentTargetResourceDeposit = resourceDeposits[Random.Range(0, resourceDeposits.Count)];
        else
            attacker.ClearTargetVariables();
    }

    void SetLaneSpawner()
    {
        AttackerSpawner[] attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            bool isCloseEnough = (Mathf.Abs(spawner.transform.position.y - transform.position.y) <= Mathf.Epsilon);

            if (isCloseEnough) myLaneSpawner = spawner;
        }
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
            if (Application.isPlaying == false)
                Gizmos.DrawIcon(transform.position, "GoldDeposit_Icon.png", true);
        #endif
    }

    /*void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out Attacker attacker);
        if (attacker != null && attackersAttackingNode.Contains(attacker))
            attackersAttackingNode.Remove(attacker);
    }*/
}
