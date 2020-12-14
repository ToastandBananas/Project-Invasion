using System.Collections;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int goldCost = 100;
    public bool isAttacking = false;
    public bool isMoving = false;
    public float minAttackDistance = 0.125f;
    float currentSpeed = 0f;

    float randomAttackOffsetY;
    //public float attackOffsetX = -0.1f;
    //public Vector3 attackOffset;
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
        //attackOffset = new Vector3(attackOffsetX, randomAttackOffsetY);

        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        squad = transform.parent.parent.GetComponent<Squad>();

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
            if (targetAttacker != null && targetAttacker.health.isDead)
            {
                targetAttacker.health.FindNewTargetForOpponent();
                //targetAttacker = null;
                //currentTargetsHealth = null;
                //StopAttacking();
            }

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

    public int GetGoldCost()
    {
        return goldCost;
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
        transform.position = Vector2.MoveTowards(transform.position, targetAttacker.transform.position/* + attackOffset*/, currentSpeed * Time.deltaTime);
        if (transform.position.x <= targetAttacker.transform.position.x && transform.localScale.x != -1)
            transform.localScale = new Vector2(1, 1);
        else if (transform.position.x > targetAttacker.transform.position.x && transform.localScale.x != 1)
            transform.localScale = new Vector2(-1, 1);
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
        if (currentSpeed == 0 && transform.localScale.x != 1)
            transform.localScale = new Vector2(1, 1);
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        currentTargetsHealth = targetAttacker.health;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
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
