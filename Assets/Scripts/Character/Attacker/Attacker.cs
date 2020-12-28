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
}
