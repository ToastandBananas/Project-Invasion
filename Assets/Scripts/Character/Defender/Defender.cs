using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int goldCost = 100;
    public bool isAttacking = false;
    float currentSpeed = 0f;

    float randomAttackOffsetY;
    [SerializeField] float attackOffsetX = -0.1f;
    public Vector3 attackOffset;
    public Vector2 unitPosition;
    Vector2 currentLocalPosition;


    public Attacker targetAttacker;
    public Health currentTargetsHealth;

    CurrencyDisplay currencyDisplay;
    Animator anim;
    public Squad squad;
    public Health health;

    void Start()
    {
        randomAttackOffsetY = Random.Range(-0.15f, 0.15f);
        attackOffset = new Vector3(attackOffsetX, randomAttackOffsetY);

        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        squad = transform.parent.parent.GetComponent<Squad>();

        SetMovementSpeed(.25f);
    }

    void Update()
    {
        if (squad.attackersInRange.Count == 0)
            MoveUnitIntoPosition();
        else if (targetAttacker != null && currentSpeed > 0)
            MoveTowardsAttacker();
    }

    void FixedUpdate()
    {
        UpdateAnimationState();
    }

    public void AddGold(int amount)
    {
        currencyDisplay.AddGold(amount);
    }

    public int GetGoldCost()
    {
        return goldCost;
    }

    public void MoveUnitIntoPosition()
    {
        currentLocalPosition = transform.localPosition;
        if (currentLocalPosition != unitPosition)
        {
            if (unitPosition.x <= transform.localPosition.x - 0.001f && transform.localScale.x != -1)
                transform.localScale = new Vector2(-1, 1);
            else if (unitPosition.x >= transform.localPosition.x && transform.localScale.x != 1)
                transform.localScale = new Vector2(1, 1);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.parent.position.x, transform.parent.position.y) + unitPosition, currentSpeed * Time.deltaTime);
        }
        else if (transform.localScale.x != 1)
            transform.localScale = new Vector2(1, 1);
    }

    public void MoveTowardsAttacker()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetAttacker.transform.position + attackOffset, currentSpeed * Time.deltaTime);
        //if (Vector2.Distance(transform.position, attackerBeingAttackedBy.transform.position) <= attackOffset.x)
            //Attack();
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
        if (currentSpeed == 0)
            transform.localScale = Vector2.one;
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        currentTargetsHealth = targetAttacker.health;
    }

    public void StrikeCurrentTarget(float damage)
    {
        if (targetAttacker == null) return;

        if (currentTargetsHealth)
            currentTargetsHealth.DealDamage(damage);
    }

    void UpdateAnimationState()
    {
        if (targetAttacker == null)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }
}
