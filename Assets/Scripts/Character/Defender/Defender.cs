using System.Collections;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [Header("Attack/Movement Info")]
    public float minAttackDistance = 0.115f;
    public float runSpeed = 0.5f;
    float currentSpeed = 0f;

    [Header("Weapon Info")]
    [SerializeField] MeleeWeaponType meleeWeaponType;
    public float bluntDamage, slashDamage, piercingDamage, fireDamage;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isRetreating = false;

    [HideInInspector] public Attacker targetAttacker;
    [HideInInspector] public Health targetAttackersHealth;

    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Squad squad;
    [HideInInspector] public Health health;
    [HideInInspector] public Shooter myShooter;

    [HideInInspector] public Vector2 unitPosition;
    float randomAttackOffsetY;
    Vector2 currentLocalPosition;

    AudioManager audioManager;
    CurrencyDisplay currencyDisplay;

    void Awake()
    {
        squad = transform.parent.parent.GetComponent<Squad>();
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        randomAttackOffsetY = Random.Range(-0.15f, 0.15f);

        audioManager = AudioManager.instance;
        currencyDisplay = CurrencyDisplay.instance;
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();

        StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        UpdateAnimationState();
    }

    IEnumerator Movement()
    {
        while (health.isDead == false && isRetreating == false)
        {
            if (squad.squadPlaced)
            {
                if (squad.attackersNearby.Count == 0 || (targetAttacker == null && Vector2.Distance(transform.localPosition, unitPosition) > 0.025f))
                    MoveUnitIntoPosition();
                else if (targetAttacker != null)
                    MoveTowardsAttacker();
            }

            yield return null;
        }
    }

    public IEnumerator Retreat()
    {
        sr.sortingOrder = 3;
        isRetreating = true;
        isAttacking = false;
        isMoving = true;

        targetAttacker = null;
        targetAttackersHealth = null;

        anim.SetBool("isMoving", true);
        anim.SetBool("isAttacking", false);
        if (squad.isRangedUnit)
            anim.SetBool("isShooting", false);

        
        while (transform.position.x > -1.25f)
        {
            if (transform.localScale.x != -1)
                transform.localScale = new Vector2(-1, 1); // Flip the sprite

            if (transform.position.x > 0.25f || (transform.position.x <= 0.25 && transform.position.y >= 2.9f && transform.position.y <= 3.1f))
                transform.Translate(Vector2.left * currentSpeed * 2 * Time.deltaTime); // Retreat double the speed
            else if (transform.position.x <= 0.25f && transform.position.y < 2.9f)
                transform.Translate(Vector2.up * currentSpeed * 2 * Time.deltaTime);
            else if (transform.position.x <= 0.25f && transform.position.y > 3.1f)
                transform.Translate(Vector2.down * currentSpeed * 2 * Time.deltaTime);

            if (transform.position.x <= -2.25f)
            {
                Destroy(squad.gameObject); // Destroy the squad
                break;
            }

            yield return null;
        }
    }

    public void AddGold(int amount)
    {
        currencyDisplay.AddGold(amount);
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
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", false);
            if (transform.localScale.x != 1)
                transform.localScale = new Vector2(1, 1);
        }
    }

    public void MoveTowardsAttacker()
    {
        isMoving = true;
        anim.SetBool("isMoving", true);

        if (isAttacking == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetAttacker.transform.position, currentSpeed * Time.deltaTime);
            if (transform.position.x <= targetAttacker.transform.position.x && transform.localScale.x != 1)
                transform.localScale = new Vector2(1, 1);
            else if (transform.position.x > targetAttacker.transform.position.x && transform.localScale.x != -1)
                transform.localScale = new Vector2(-1, 1);
        }
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
        if (currentSpeed == 0 && transform.localScale.x != 1 && isAttacking == false)
            transform.localScale = new Vector2(1, 1);
    }

    public void StopMoving()
    {
        SetMovementSpeed(0f);
    }

    public void SetRunSpeed()
    {
        SetMovementSpeed(runSpeed);
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        targetAttackersHealth = targetAttacker.health;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    public void StrikeCurrentTarget()
    {
        if (targetAttacker == null) return;

        if (transform.position.x <= targetAttacker.transform.position.x)
            transform.localScale = new Vector2(1, 1);
        else
            transform.localScale = new Vector2(-1, 1);

        if (targetAttackersHealth)
            targetAttackersHealth.DealDamage(bluntDamage, slashDamage, piercingDamage, fireDamage);

        audioManager.PlayMeleeHitSound(meleeWeaponType);
    }

    void UpdateAnimationState()
    {
        if (targetAttacker == null)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }

    public void FindNewTargetForAttackers(Defender defender)
    {
        targetAttacker = null;
        targetAttackersHealth = null;

        if (transform.parent == defender.squad.unitsParent) // Remove the defender from the units list
            defender.squad.units.Remove(defender);
        else if (transform.parent == defender.squad.leaderParent)
        {
            defender.squad.Retreat(); // The leader was killed, so retreat the remaining units in the squad
            defender.squad.leader = null;
        }

        foreach (Attacker attacker in defender.squad.attackersNearby) // For each attacker in range of the defender who died...
        {
            Defender theCurrentDefenderBeingAttacked = attacker.currentDefenderAttacking;

            if (attacker.opponents.Contains(defender))
                attacker.opponents.Remove(defender);

            // Remove any opponents missing in the list (will happen if another attacker kills the defender)
            for (int i = 0; i < attacker.opponents.Count; i++)
            {
                if (attacker.opponents[i] == null)
                    attacker.opponents.Remove(attacker.opponents[i]);
            }

            if (theCurrentDefenderBeingAttacked == defender || theCurrentDefenderBeingAttacked == null) // If the attacker was fighting the defender who died (or if it became null somehow)
            {
                attacker.currentDefenderAttacking = null;
                attacker.currentTargetsHealth = null;
                attacker.isAttacking = false;

                if (attacker.opponents.Count > 0) // If the attacker already has another defender (opponent) attacking him
                {
                    attacker.currentDefenderAttacking = attacker.opponents[0];
                    attacker.currentTargetsHealth = attacker.opponents[0].health;
                    if (Vector2.Distance(attacker.transform.position, attacker.opponents[0].transform.position) > attacker.minAttackDistance)
                        attacker.StopAttacking();
                }
                else if (defender.squad.units.Count > 0 || defender.squad.leader != null) // If there's still defenders in the squad
                {
                    // Find a new target if possible (for the attacker who killed this defender)
                    int randomTargetIndex = Random.Range(0, defender.squad.units.Count);
                    if (defender.squad.leader != null) randomTargetIndex++;

                    if ((randomTargetIndex == defender.squad.units.Count || defender.squad.units.Count == 0) && attacker.opponents.Contains(defender.squad.leader) == false)
                    {
                        // Attack the squad's leader
                        attacker.currentDefenderAttacking = defender.squad.leader;
                        attacker.currentTargetsHealth = defender.squad.leader.health;
                        attacker.opponents.Add(defender.squad.leader);

                        if (defender.squad.leader.targetAttacker == null)
                        {
                            defender.squad.leader.targetAttacker = attacker;
                            defender.squad.leader.targetAttackersHealth = attacker.health;
                        }

                        if (Vector2.Distance(attacker.transform.position, defender.squad.leader.transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                    else if (attacker.opponents.Contains(defender.squad.units[randomTargetIndex]) == false)
                    {
                        // Attack a random unit in the squad
                        attacker.currentDefenderAttacking = defender.squad.units[randomTargetIndex];
                        attacker.currentTargetsHealth = defender.squad.units[randomTargetIndex].health;
                        attacker.opponents.Add(defender.squad.units[randomTargetIndex]);

                        if (defender.squad.units[randomTargetIndex].targetAttacker == null)
                        {
                            defender.squad.units[randomTargetIndex].targetAttacker = attacker;
                            defender.squad.units[randomTargetIndex].targetAttackersHealth = attacker.health;
                        }

                        if (Vector2.Distance(attacker.transform.position, defender.squad.units[randomTargetIndex].transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                }
            }

            // Send in another defender if the attacker can fight multiple opponents
            if (theCurrentDefenderBeingAttacked == defender && attacker.maxOpponents > attacker.opponents.Count)
            {
                foreach (Defender potentialOpponent in defender.squad.units)
                {
                    if (potentialOpponent.targetAttacker == null && attacker.opponents.Contains(potentialOpponent) == false)
                    {
                        potentialOpponent.targetAttacker = attacker;
                        attacker.opponents.Add(potentialOpponent);
                        if (attacker.maxOpponents == attacker.opponents.Count)
                            return;
                    }
                }

                if (attacker.maxOpponents > attacker.opponents.Count && defender.squad.leader != null && attacker.opponents.Contains(defender.squad.leader) == false)
                {
                    defender.squad.leader.targetAttacker = attacker;
                    attacker.opponents.Add(defender.squad.leader);
                }
            }
        }
    }

    public void SetAttackDamage(float bluntDamageAddOn, float slashDamageAddOn, float piercingDamageAddOn, float fireDamageAddOn)
    {
        bluntDamage += bluntDamageAddOn;
        slashDamage += slashDamageAddOn;
        piercingDamage += piercingDamageAddOn;
        fireDamage += fireDamageAddOn;
    }
}