using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : MonoBehaviour
{
    [HideInInspector] public List<Attacker> attackersInRange = new List<Attacker>();
    [HideInInspector] public List<Defender> defendersInRange = new List<Defender>();

    [HideInInspector] public BoxCollider2D boxCollider;
    [HideInInspector] public Vector2 originalOffset;
    [HideInInspector] public Vector2 originalSize;

    Attacker attacker;
    Squad squad;

    void Start()
    {
        attacker = transform.parent.GetComponent<Attacker>();
        squad = transform.parent.GetComponent<Squad>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalOffset = boxCollider.offset;
        originalSize = boxCollider.size;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (squad != null && collision.TryGetComponent(out Attacker enemyAttacker))
        {
            attackersInRange.Add(enemyAttacker);
        }
        else if (attacker != null)
        {
            if (collision.TryGetComponent(out Defender enemyDefender))
            {
                if (attacker.myAttackerSpawner == enemyDefender.squad.myLaneSpawner)
                    defendersInRange.Add(enemyDefender);
            }
            else if (attacker.canAttackNodes && collision.TryGetComponent(out ResourceDeposit goldDeposit))
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
        if (squad != null && collision.TryGetComponent(out Attacker enemyAttacker))
        {
            if (attackersInRange.Contains(enemyAttacker))
                attackersInRange.Remove(enemyAttacker);
        }
        else if (attacker != null)
        {
            if (collision.TryGetComponent(out Defender enemyDefender))
            {
                if (defendersInRange.Contains(enemyDefender))
                    defendersInRange.Remove(enemyDefender);
            }
        }
    }
}
