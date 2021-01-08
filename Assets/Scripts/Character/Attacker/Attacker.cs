using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] float attackDamage = 10f;
    public bool isLarge = false;
    public bool isAttacking = false;
    public int maxOpponents = 2;
    public float minAttackDistance = 0.115f;
    float currentSpeed = 1f;
    float distanceToTarget;

    public List<Defender> opponents;
    public Defender currentDefenderAttacking;
    public Health currentTargetsHealth;
    public Squad currentTargetsSquad;

    [HideInInspector] public Health health;
    Animator anim;

    void Start()
    {
        opponents = new List<Defender>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();

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
            if (currentDefenderAttacking != null && Vector2.Distance(transform.position, currentDefenderAttacking.transform.position) > minAttackDistance && isAttacking == false)
                MoveTowardsTarget();
            else if (currentDefenderAttacking == null)
            {
                transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
                if (transform.localScale.x != 1)
                    transform.localScale = new Vector2(1, 1);
            }

            yield return null;
        }
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentDefenderAttacking.transform.position, currentSpeed * Time.deltaTime);
        if (transform.position.x <= currentDefenderAttacking.transform.position.x && transform.localScale.x != -1)
            transform.localScale = new Vector2(-1, 1);
        else if (transform.position.x > currentDefenderAttacking.transform.position.x && transform.localScale.x != 1)
            transform.localScale = new Vector2(1, 1);
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        currentTargetsHealth = currentDefenderAttacking.health;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    public void StrikeCurrentTarget()
    {
        if (currentDefenderAttacking == null) return;

        if (transform.position.x <= currentDefenderAttacking.transform.position.x)
            transform.localScale = new Vector2(-1, 1);
        else
            transform.localScale = new Vector2(1, 1);

        if (currentTargetsHealth != null)
        {
            if (currentTargetsHealth.isDead)
            {
                opponents.Remove(currentDefenderAttacking);
                currentTargetsHealth = null;
                currentDefenderAttacking = null;
                return;
            }
                
            currentTargetsHealth.DealDamage(attackDamage);
        }
    }

    void UpdateAnimationState()
    {
        if (currentDefenderAttacking == null)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }

    public void FindNewTargetForDefenders(Attacker attacker)
    {
        if (attacker.currentDefenderAttacking != null)
        {
            // Remove the dead attacker from the attackersInRange list
            if (attacker.currentDefenderAttacking.squad.attackersInRange.Contains(attacker))
                attacker.currentDefenderAttacking.squad.attackersInRange.Remove(attacker);

            foreach (Defender opponent in attacker.opponents)
            {
                opponent.targetAttacker = null;
                opponent.targetAttackersHealth = null;
                opponent.StopAttacking();
            }

            // Find new target if possible (for the defender that killed this attacker)
            if (attacker.currentDefenderAttacking.squad.attackersInRange.Count > 0)
            {
                foreach (Defender opponent in attacker.opponents)
                {
                    FindNewRandomTargetForDefender(opponent);
                }
            }
            else // If there are no more attackers nearby, reorganize the squad
            {
                attacker.currentDefenderAttacking.squad.AssignUnitPositions();
                attacker.currentDefenderAttacking.squad.AssignLeaderPosition();
            }
        }
        else
        {
            foreach (Defender opponent in attacker.opponents)
            {
                // Remove the dead attacker from the attackersInRange list
                if (opponent.squad.attackersInRange.Contains(attacker))
                    opponent.squad.attackersInRange.Remove(attacker);

                opponent.targetAttacker = null;
                opponent.targetAttackersHealth = null;
                opponent.StopAttacking();
            }

            foreach (Defender opponent in attacker.opponents)
            {
                // Find a new target for each defender that was fighting the attacker that died
                if (opponent.squad.attackersInRange.Count > 0)
                    FindNewRandomTargetForDefender(opponent);
                else
                {
                    opponent.squad.AssignUnitPositions();
                    opponent.squad.AssignLeaderPosition();
                    return;
                }
            }
        }
    }

    void FindNewRandomTargetForDefender(Defender theDefender)
    {
        int randomTargetIndex = Random.Range(0, theDefender.squad.attackersInRange.Count);
        if (randomTargetIndex >= theDefender.squad.attackersInRange.Count && randomTargetIndex > 0)
            randomTargetIndex = theDefender.squad.attackersInRange.Count - 1;

        theDefender.targetAttacker = theDefender.squad.attackersInRange[randomTargetIndex];
        theDefender.targetAttackersHealth = theDefender.squad.attackersInRange[randomTargetIndex].health;
        if (theDefender.squad.attackersInRange[randomTargetIndex].opponents.Contains(theDefender) == false)
            theDefender.squad.attackersInRange[randomTargetIndex].opponents.Add(theDefender);
    }
}
