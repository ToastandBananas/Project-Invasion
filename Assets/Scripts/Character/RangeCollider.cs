using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : MonoBehaviour
{
    public List<Attacker> attackersInRange = new List<Attacker>();
    public List<Defender> defendersInRange = new List<Defender>();
    public List<Squad>    squadsInRange    = new List<Squad>();

    [HideInInspector] public BoxCollider2D boxCollider;
    [HideInInspector] public Vector2 originalOffset;
    [HideInInspector] public Vector2 originalSize;

    Attacker attacker;
    Squad squad;

    void Start()
    {
        attacker = transform.GetComponentInParent<Attacker>();
        squad = transform.GetComponentInParent<Squad>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalOffset = boxCollider.offset;
        originalSize = boxCollider.size;
    }

    public Defender GetFurthestDefenderWithLowHealth()
    {
        Defender furthestDefender = null;
        float furthestDefenderDistance = 0;

        for (int i = 0; i < defendersInRange.Count; i++)
        {
            if (defendersInRange[i].health.GetCurrentHealth() < defendersInRange[i].health.GetMaxHealth())
            {
                if (i == 0)
                {
                    furthestDefender = defendersInRange[i];
                    furthestDefenderDistance = Vector2.Distance(transform.position, defendersInRange[i].transform.position);
                }
                else
                {
                    float dist = Vector2.Distance(transform.position, defendersInRange[i].transform.position);
                    if (dist > furthestDefenderDistance)
                    {
                        furthestDefender = defendersInRange[i];
                        furthestDefenderDistance = dist;
                    }
                }
            }
        }

        return furthestDefender;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (squad != null) // If this Range Collider belongs to a friendly Squad
        {
            if (squad.squadType == SquadType.Priests && collision.CompareTag("Defender"))
            {
                if (collision.TryGetComponent(out Defender friendlyDefender))
                {
                    defendersInRange.Add(friendlyDefender);
                    if (squadsInRange.Contains(friendlyDefender.squad) == false)
                        squadsInRange.Add(friendlyDefender.squad);
                }
            }
            else if (collision.CompareTag("Attacker") && collision.TryGetComponent(out Attacker enemyAttacker))
                attackersInRange.Add(enemyAttacker);
        }
        else if (attacker != null) // If this Range Collider belongs to an Attacker
        {
            if (collision.CompareTag("Defender") && collision.TryGetComponent(out Defender enemyDefender))
            {
                if (attacker.myAttackerSpawner == enemyDefender.squad.myLaneSpawner)
                    defendersInRange.Add(enemyDefender);
            }
            else if (collision.CompareTag("Obstacle") && collision.TryGetComponent(out Obstacle obstacle))
            {
                if (attacker.myAttackerSpawner == obstacle.parentStructure.myLaneSpawner)
                    attacker.currentTargetObstacle = obstacle;
            }
            else if (attacker.canAttackNodes && collision.CompareTag("Deposit") && collision.TryGetComponent(out ResourceDeposit goldDeposit))
            {
                if (attacker.myAttackerSpawner == goldDeposit.resourceNode.myLaneSpawner)
                {
                    attacker.currentTargetNode = goldDeposit.resourceNode;
                    attacker.currentTargetResourceDeposit = goldDeposit;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (squad != null)
        {
            if (squad.squadType == SquadType.Priests && collision.CompareTag("Defender") && collision.TryGetComponent(out Defender friendlyDefender) && defendersInRange.Contains(friendlyDefender))
                defendersInRange.Remove(friendlyDefender);
            else if (squad.squadType == SquadType.Priests && collision.CompareTag("Squad") && collision.TryGetComponent(out Squad friendlySquad) && squadsInRange.Contains(friendlySquad))
                squadsInRange.Remove(friendlySquad);
            else if (collision.CompareTag("Attacker") && collision.TryGetComponent(out Attacker enemyAttacker) && attackersInRange.Contains(enemyAttacker))
                attackersInRange.Remove(enemyAttacker);
        }
        else if (attacker != null)
        {
            if (collision.TryGetComponent(out Defender enemyDefender) && defendersInRange.Contains(enemyDefender))
                defendersInRange.Remove(enemyDefender);
        }
    }
}
