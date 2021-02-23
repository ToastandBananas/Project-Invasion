using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    /// <summary>
    /// For use in AttackerSpawner script (each enemy has a point value...each time a group of attackers is spawned, 
    /// there can only be a certain amount of points used, in order to prevent too many strong enemies from spawning at one time
    /// </summary>
    [Header("Spawn Info")]
    public int spawnPoints = 20;
    public bool isLarge;

    [Header("Attack/Movement Info")]
    [SerializeField] MeleeWeaponType meleeWeaponType;
    [SerializeField] float meleeDamage = 10f;
    public float castleAttackDamage = 5f;
    public float minAttackDistance = 0.115f;
    public int maxOpponents = 2;
    public float runSpeed = 0.5f;
    float currentSpeed = 1f;

    [HideInInspector] public List<Defender> opponents = new List<Defender>();
    [HideInInspector] public Defender currentDefenderAttacking;
    [HideInInspector] public Health currentTargetsHealth;
    [HideInInspector] public Squad currentTargetsSquad;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isAttackingCastle;
    [HideInInspector] public AttackerSpawner myAttackerSpawner;
    [HideInInspector] public Health health;
    [HideInInspector] public RangeCollider rangeCollider;

    AudioManager audioManager;
    Animator anim;
    CastleHealth castleHealth;

    void Start()
    {
        health = GetComponent<Health>();
        rangeCollider = GetComponentInChildren<RangeCollider>();
        audioManager = AudioManager.instance;
        anim = GetComponent<Animator>();
        castleHealth = CastleHealth.instance;

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
            if (currentDefenderAttacking != null && Vector2.Distance(transform.position, currentDefenderAttacking.transform.position) > minAttackDistance && isAttacking == false && isAttackingCastle == false)
                MoveTowardsTarget();
            else if (currentDefenderAttacking == null && isAttackingCastle == false)
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

    public void StopMoving()
    {
        SetMovementSpeed(0f);
    }

    public void SetRunSpeed()
    {
        SetMovementSpeed(runSpeed);
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
        if (currentDefenderAttacking != null)
            currentTargetsHealth = currentDefenderAttacking.health;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        isAttackingCastle = false;
        anim.SetBool("isAttacking", false);
    }

    public void StrikeCurrentTarget()
    {
        if (currentDefenderAttacking == null && isAttackingCastle == false) return;

        if (currentDefenderAttacking != null)
        {
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

                currentTargetsHealth.DealDamage(meleeDamage);
            }
        }
        else if (isAttackingCastle)
        {
            castleHealth.TakeHealth(castleAttackDamage);
            if (castleHealth.GetHealth() <= 0f)
            {
                StopAttacking();
                CastleCollider.instance.enabled = false;
            }
        }

        audioManager.PlayMeleeHitSound(meleeWeaponType);
    }

    void UpdateAnimationState()
    {
        if (currentDefenderAttacking == null && isAttackingCastle == false)
            StopAttacking();
    }

    public void FindNewTargetForDefenders(Attacker attacker)
    {
        if (attacker.currentDefenderAttacking != null)
        {
            // Remove the dead attacker from the attackersInRange list
            if (attacker.currentDefenderAttacking.squad.attackersNearby.Contains(attacker))
                attacker.currentDefenderAttacking.squad.attackersNearby.Remove(attacker);

            if (attacker.currentDefenderAttacking.squad.rangeCollider != null && attacker.currentDefenderAttacking.squad.rangeCollider.attackersInRange.Contains(attacker))
                attacker.currentDefenderAttacking.squad.rangeCollider.attackersInRange.Remove(attacker);

            foreach (Defender opponent in attacker.opponents)
            {
                opponent.targetAttacker = null;
                opponent.targetAttackersHealth = null;
                opponent.StopAttacking();
            }

            // Find new target if possible (for the defender that killed this attacker)
            if (attacker.currentDefenderAttacking.squad.attackersNearby.Count > 0)
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
                if (opponent.squad.attackersNearby.Contains(attacker))
                    opponent.squad.attackersNearby.Remove(attacker);

                if (opponent.squad.rangeCollider != null && opponent.squad.rangeCollider.attackersInRange.Contains(attacker))
                    opponent.squad.rangeCollider.attackersInRange.Remove(attacker);

                opponent.targetAttacker = null;
                opponent.targetAttackersHealth = null;
                opponent.StopAttacking();
            }

            foreach (Defender opponent in attacker.opponents)
            {
                // Find a new target for each defender that was fighting the attacker that died
                if (opponent.squad.attackersNearby.Count > 0)
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
        if (theDefender.squad.attackersNearby.Count > 0)
        {
            int randomTargetIndex = Random.Range(0, theDefender.squad.attackersNearby.Count);
            if (randomTargetIndex >= theDefender.squad.attackersNearby.Count && randomTargetIndex > 0)
                randomTargetIndex = theDefender.squad.attackersNearby.Count - 1;

            theDefender.targetAttacker = theDefender.squad.attackersNearby[randomTargetIndex];
            theDefender.targetAttackersHealth = theDefender.squad.attackersNearby[randomTargetIndex].health;
            if (theDefender.squad.attackersNearby[randomTargetIndex].opponents.Contains(theDefender) == false)
                theDefender.squad.attackersNearby[randomTargetIndex].opponents.Add(theDefender);
        }
    }
}