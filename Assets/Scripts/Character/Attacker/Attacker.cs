using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] float attackDamage = 10f;
    public Vector3 attackOffset = new Vector3(0.1f, 0);
    public bool isAttacking = false;
    public int maxOpponents = 2;
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
    }

    void OnDestroy()
    {
        if (levelController != null)
            levelController.AttackerKilled();
    }

    void Update()
    {
        if (currentTarget != null && currentSpeed > 0)
            MoveTowardsTarget();
        else
            transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        UpdateAnimationState();
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position + attackOffset, currentSpeed * Time.deltaTime);
        //if (Vector2.Distance(transform.position, currentTarget.transform.position) <= attackOffset.x)
            //Attack();
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        currentDefenderAttacking = currentTarget.GetComponent<Defender>();
        currentTargetsHealth = currentDefenderAttacking.health;
    }

    public void StrikeCurrentTarget()
    {
        if (currentTarget == null) return;
        
        if (currentTargetsHealth != null)
            currentTargetsHealth.DealDamage(attackDamage);
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
