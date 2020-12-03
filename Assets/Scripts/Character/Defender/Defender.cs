using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int goldCost = 100;
    [SerializeField] Vector3 attackOffset = new Vector3(-0.1f, 0);
    public Vector2 unitPosition;
    Vector2 currentLocalPosition;

    public Attacker attackerBeingAttackedBy;

    float currentSpeed = 0f;
    Health currentTargetsHealth;

    CurrencyDisplay currencyDisplay;
    Animator anim;
    public Squad squad;

    void Start()
    {
        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        anim = GetComponent<Animator>();
        squad = transform.parent.parent.GetComponent<Squad>();

        SetMovementSpeed(.25f);
    }

    void Update()
    {
        if (squad.underAttack == false)
            MoveUnitIntoPosition();
        else if (attackerBeingAttackedBy != null && currentSpeed > 0)
            MoveTowardsAttacker();
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
            if (unitPosition.x < transform.localPosition.x - 0.001f && transform.localScale.x != -1)
                transform.localScale = new Vector2(-1, 1);
            else if (unitPosition.x >= transform.localPosition.x && transform.localScale.x != 1)
                transform.localScale = new Vector2(1, 1);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.parent.position.x, transform.parent.position.y) + unitPosition, currentSpeed * Time.deltaTime);
        }
    }

    public void MoveTowardsAttacker()
    {
        transform.position = Vector2.MoveTowards(transform.position, attackerBeingAttackedBy.transform.position + attackOffset, currentSpeed * Time.deltaTime);
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
        if (currentSpeed == 0)
            transform.localScale = Vector2.one;
    }

    public void Attack()
    {
        anim.SetBool("isAttacking", true);
        currentTargetsHealth = attackerBeingAttackedBy.GetComponent<Health>();
    }

    public void StrikeCurrentTarget(float damage)
    {
        if (attackerBeingAttackedBy == null) return;

        if (currentTargetsHealth)
            currentTargetsHealth.DealDamage(damage);
    }
}
