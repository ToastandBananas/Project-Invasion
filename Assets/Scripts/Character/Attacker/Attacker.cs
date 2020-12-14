using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] float attackDamage = 10f;
    public bool isLarge = false;
    public bool isAttacking = false;
    public int maxOpponents = 2;
    public float minAttackDistance = 0.125f;
    float currentSpeed = 1f;
    float distanceToTarget;

    public List<Defender> opponents;
    public GameObject currentTarget;
    public Defender currentDefenderAttacking;
    public Health currentTargetsHealth;
    public Squad currentTargetsSquad;

    public Health health;
    Animator anim;
    LevelController levelController;

    void Start()
    {
        opponents = new List<Defender>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        levelController = FindObjectOfType<LevelController>();

        StartCoroutine(Movement());
    }

    void OnDestroy()
    {
        if (levelController != null)
            levelController.AttackerKilled();
    }

    void FixedUpdate()
    {
        UpdateAnimationState();
    }

    IEnumerator Movement()
    {
        while (health.isDead == false)
        {
            if (opponents.Count > 0)
            {
                for (int i = 0; i < opponents.Count; i++)
                {
                    if (opponents[i].health.isDead)
                    {
                        opponents[i].health.FindNewTargetForOpponent();
                    }
                }
            }

            if (currentTarget != null && Vector2.Distance(transform.position, currentTarget.transform.position) > minAttackDistance)
                MoveTowardsTarget();
            else
                transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position/* + new Vector3(-currentDefenderAttacking.attackOffsetX, 0)*/, currentSpeed * Time.deltaTime);
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        currentDefenderAttacking = currentTarget.GetComponent<Defender>();
        currentTargetsHealth = currentDefenderAttacking.health;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
        if (opponents.Count > 0)
        {
            foreach (Defender defender in opponents)
            {
                if (defender.health.isDead)
                {
                    opponents.Remove(defender);
                    return;
                }
            }
        }
    }

    public void StrikeCurrentTarget()
    {
        if (currentTarget == null) return;

        if (currentTargetsHealth != null)
        {
            if (currentTargetsHealth.isDead)
            {
                currentTarget = null;
                currentTargetsHealth = null;
                currentDefenderAttacking = null;
                return;
            }
                
            currentTargetsHealth.DealDamage(attackDamage);
        }
    }

    void UpdateAnimationState()
    {
        if (currentTarget == null)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }
}
