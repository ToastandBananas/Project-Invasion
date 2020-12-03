using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] float attackDamage = 10f;
    float currentSpeed = 1f;
    float distanceToTarget;

    public GameObject currentTarget;
    Health currentTargetsHealth;
    public Squad currentTargetsSquad;

    [SerializeField] Vector3 attackOffset = new Vector3(0.1f, 0);

    Animator anim;
    LevelController levelController;

    void Start()
    {
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

    public void Attack(/*GameObject target*/)
    {
        anim.SetBool("isAttacking", true);
        //currentTarget = target;
        currentTargetsHealth = currentTarget.GetComponent<Health>();
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
            anim.SetBool("isAttacking", false);
    }
}
