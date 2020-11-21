using UnityEngine;

public class Attacker : MonoBehaviour
{
    float currentSpeed = 1f;
    GameObject currentTarget;
    Health currentTargetsHealth;

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

    public void Attack(GameObject target)
    {
        anim.SetBool("isAttacking", true);
        currentTarget = target;
        currentTargetsHealth = currentTarget.GetComponent<Health>();
    }

    public void StrikeCurrentTarget(float damage)
    {
        if (currentTarget == null) return;
        
        if (currentTargetsHealth)
            currentTargetsHealth.DealDamage(damage);
    }

    void UpdateAnimationState()
    {
        if (currentTarget == null)
            anim.SetBool("isAttacking", false);
    }
}
