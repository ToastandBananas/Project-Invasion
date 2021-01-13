﻿using System.Collections;
using UnityEngine;

public class Defender : MonoBehaviour
{
    public bool isAttacking = false;
    public bool isMoving = false;
    public float minAttackDistance = 0.115f;
    float currentSpeed = 0f;

    float randomAttackOffsetY;
    public Vector2 unitPosition;
    Vector2 currentLocalPosition;

    public Attacker targetAttacker;
    public Health targetAttackersHealth;

    CurrencyDisplay currencyDisplay;
    Animator anim;
    public Squad squad;
    [HideInInspector] public Health health;

    void Awake()
    {
        squad = transform.parent.parent.GetComponent<Squad>();
    }

    void Start()
    {
        randomAttackOffsetY = Random.Range(-0.15f, 0.15f);

        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();

        SetMovementSpeed(0.25f);

        StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        UpdateAnimationState();
    }

    IEnumerator Movement()
    {
        while (health.isDead == false)
        {
            if (squad.attackersInRange.Count == 0 || (targetAttacker == null && Vector2.Distance(transform.localPosition, unitPosition) > 0.025f))
                MoveUnitIntoPosition();
            else if (targetAttacker != null)
                MoveTowardsAttacker();

            yield return null;
        }
    }

    public void AddGold(int amount)
    {
        currencyDisplay.AddGold(amount);
    }

    public void MoveUnitIntoPosition()
    {
        currentLocalPosition = transform.localPosition;
        if (currentLocalPosition != unitPosition && Vector2.Distance(transform.localPosition, unitPosition) > 0.025f)
        {
            isMoving = true;
            anim.SetBool("isMoving", true);

            if (unitPosition.x <= transform.localPosition.x - 0.001f && transform.localScale.x != -1)
                transform.localScale = new Vector2(-1, 1);
            else if (unitPosition.x >= transform.localPosition.x && transform.localScale.x != 1)
                transform.localScale = new Vector2(1, 1);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.parent.position.x, transform.parent.position.y) + unitPosition, currentSpeed * Time.deltaTime);
        }
        else if (transform.localScale.x != 1)
        {
            isMoving = false;
            anim.SetBool("isMoving", false);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", false);
        }
    }

    public void MoveTowardsAttacker()
    {
        isMoving = true;
        anim.SetBool("isMoving", true);

        if (isAttacking == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetAttacker.transform.position, currentSpeed * Time.deltaTime);
            if (transform.position.x <= targetAttacker.transform.position.x && transform.localScale.x != 1)
                transform.localScale = new Vector2(1, 1);
            else if (transform.position.x > targetAttacker.transform.position.x && transform.localScale.x != -1)
                transform.localScale = new Vector2(-1, 1);
        }
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
        if (currentSpeed == 0 && transform.localScale.x != 1 && isAttacking == false)
            transform.localScale = new Vector2(1, 1);
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        targetAttackersHealth = targetAttacker.health;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    public void StrikeCurrentTarget(float damage)
    {
        if (targetAttacker == null) return;

        if (transform.position.x <= targetAttacker.transform.position.x)
            transform.localScale = new Vector2(1, 1);
        else
            transform.localScale = new Vector2(-1, 1);

        if (targetAttackersHealth)
            targetAttackersHealth.DealDamage(damage);
    }

    void UpdateAnimationState()
    {
        if (targetAttacker == null)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }

    public void FindNewTargetForAttackers(Defender defender)
    {
        if (transform.parent == defender.squad.unitsParent) // Remove the defender from the units list
            defender.squad.units.Remove(defender);
        else if (transform.parent == defender.squad.leaderParent)
        {
            // The leader was killed, so retreat the remaining units in the squad
            defender.squad.leader = null;
            defender.squad.Retreat();
        }

        foreach (Attacker attacker in defender.squad.attackersInRange) // For each attacker in range of the defender who died...
        {
            Defender theCurrentDefenderBeingAttacked = attacker.currentDefenderAttacking;

            if (attacker.opponents.Contains(defender))
                attacker.opponents.Remove(defender);

            // Remove any opponents missing in the list (will happen if another attacker kills the defender)
            for (int i = 0; i < attacker.opponents.Count; i++)
            {
                if (attacker.opponents[i] == null)
                    attacker.opponents.Remove(attacker.opponents[i]);
            }

            if (theCurrentDefenderBeingAttacked == defender || theCurrentDefenderBeingAttacked == null) // If the attacker was fighting the defender who died (or if it became null somehow)
            {
                attacker.currentDefenderAttacking = null;
                attacker.currentTargetsHealth = null;
                attacker.isAttacking = false;

                if (attacker.opponents.Count > 0) // If the attacker already has another defender (opponent) attacking him
                {
                    attacker.currentDefenderAttacking = attacker.opponents[0];
                    attacker.currentTargetsHealth = attacker.opponents[0].health;
                    if (Vector2.Distance(attacker.transform.position, attacker.opponents[0].transform.position) > attacker.minAttackDistance)
                        attacker.StopAttacking();
                }
                else if (defender.squad.units.Count > 0 || defender.squad.leader != null) // If there's still defenders in the squad
                {
                    // Find a new target if possible (for the attacker who killed this defender)
                    int randomTargetIndex = Random.Range(0, defender.squad.units.Count);
                    if (defender.squad.leader != null) randomTargetIndex++;

                    if ((randomTargetIndex == defender.squad.units.Count || defender.squad.units.Count == 0) && attacker.opponents.Contains(defender.squad.leader) == false)
                    {
                        // Attack the squad's leader
                        attacker.currentDefenderAttacking = defender.squad.leader;
                        attacker.currentTargetsHealth = defender.squad.leader.health;
                        attacker.opponents.Add(defender.squad.leader);

                        if (defender.squad.leader.targetAttacker == null)
                        {
                            defender.squad.leader.targetAttacker = attacker;
                            defender.squad.leader.targetAttackersHealth = attacker.health;
                        }

                        if (Vector2.Distance(attacker.transform.position, defender.squad.leader.transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                    else if (attacker.opponents.Contains(defender.squad.units[randomTargetIndex]) == false)
                    {
                        // Attack a random unit in the squad
                        attacker.currentDefenderAttacking = defender.squad.units[randomTargetIndex];
                        attacker.currentTargetsHealth = defender.squad.units[randomTargetIndex].health;
                        attacker.opponents.Add(defender.squad.units[randomTargetIndex]);

                        if (defender.squad.units[randomTargetIndex].targetAttacker == null)
                        {
                            defender.squad.units[randomTargetIndex].targetAttacker = attacker;
                            defender.squad.units[randomTargetIndex].targetAttackersHealth = attacker.health;
                        }

                        if (Vector2.Distance(attacker.transform.position, defender.squad.units[randomTargetIndex].transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                }
            }

            // Send in another defender if the attacker can fight multiple opponents
            if (theCurrentDefenderBeingAttacked == defender && attacker.maxOpponents > attacker.opponents.Count)
            {
                foreach (Defender potentialOpponent in defender.squad.units)
                {
                    if (potentialOpponent.targetAttacker == null && attacker.opponents.Contains(potentialOpponent) == false)
                    {
                        potentialOpponent.targetAttacker = attacker;
                        attacker.opponents.Add(potentialOpponent);
                        if (attacker.maxOpponents == attacker.opponents.Count)
                            return;
                    }
                }

                if (attacker.maxOpponents > attacker.opponents.Count && defender.squad.leader != null && attacker.opponents.Contains(defender.squad.leader) == false)
                {
                    defender.squad.leader.targetAttacker = attacker;
                    attacker.opponents.Add(defender.squad.leader);
                }
            }
        }
    }
}
