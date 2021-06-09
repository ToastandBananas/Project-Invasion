using System.Collections;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [Header("Attack/Movement Info")]
    public float minAttackDistance = 0.115f;
    public float runSpeed = 0.5f;
    float currentSpeed = 0f;
    float knockbackSpeed = 1.25f;
    float knockbackDistance = 0.5f;
    public bool isLarge;

    [Header("Weapon Info")]
    [SerializeField] MeleeWeaponType meleeWeaponType;
    public float bluntDamage, slashDamage, piercingDamage, fireDamage;
    [HideInInspector] public float startingBluntDamage, startingSlashDamage, startingPiercingDamage, startingFireDamage;
    public bool shouldKnockback;

    [Header("Voice")]
    public VoiceType voiceType;

    [Header("Components")]
    public BoxCollider2D boxCollider;

    [HideInInspector] public bool isLeader;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isRetreating;
    [HideInInspector] public bool isBeingKnockedBack;

    [HideInInspector] public Attacker currentTargetAttacker;
    [HideInInspector] public Health currentTargetAttackersHealth;

    [HideInInspector] public Ally allyScript;
    [HideInInspector] public Animator anim, effectAnim1, effectAnim2, effectAnim3;
    [HideInInspector] public SpriteRenderer sr, effectSR1, effectSR2, effectSR3;
    [HideInInspector] public Squad squad;
    [HideInInspector] public Health health;
    [HideInInspector] public Shooter myShooter;
    [HideInInspector] public Laborer laborer;

    [HideInInspector] public Vector2 unitPosition;
    [HideInInspector] public float minDistanceFromTargetPosition = 0.025f;

    AudioManager audioManager;
    ResourceDisplay currencyDisplay;

    void Awake()
    {
        transform.parent.parent.TryGetComponent(out squad);
        transform.GetChild(0).TryGetComponent(out sr); // SpriteRenderer
        TryGetComponent(out laborer);

        if (boxCollider == null)
            TryGetComponent(out boxCollider);
    }

    void Start()
    {
        audioManager = AudioManager.instance;
        currencyDisplay = ResourceDisplay.instance;

        TryGetComponent(out allyScript);
        TryGetComponent(out health);
        TryGetComponent(out anim);

        transform.GetChild(0).GetChild(0).TryGetComponent(out effectAnim1);
        transform.GetChild(0).GetChild(1).TryGetComponent(out effectAnim2);
        transform.GetChild(0).GetChild(2).TryGetComponent(out effectAnim3);
        transform.GetChild(0).GetChild(0).TryGetComponent(out effectSR1);
        transform.GetChild(0).GetChild(1).TryGetComponent(out effectSR2);
        transform.GetChild(0).GetChild(2).TryGetComponent(out effectSR3);

        startingBluntDamage = bluntDamage;
        startingSlashDamage = slashDamage;
        startingPiercingDamage = piercingDamage;
        startingFireDamage = fireDamage;

        if (transform.parent == squad.leaderParent)
            isLeader = true;

        StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        if (currentTargetAttacker != null && currentTargetAttacker.health.isDead)
            GetNewTarget();

        if (health.isDead == false)
        {
            UpdateAnimationState();
            UpdateSortingLayer();
        }
    }

    IEnumerator Movement()
    {
        while (health.isDead == false && isRetreating == false)
        {
            if (squad.squadPlaced && isBeingKnockedBack == false)
            {
                if (squad.attackersNearby.Count == 0 || currentTargetAttacker == null)
                    MoveUnitIntoPosition();
                else if (currentTargetAttacker != null && squad.squadFormation != SquadFormation.Wall)
                    MoveTowardsAttacker();
                else if (squad.squadFormation == SquadFormation.Wall)
                    AttackNearestAttacker();
            }

            yield return null;
        }
    }

    public IEnumerator Retreat()
    {
        if (isRetreating == false)
        {
            StopAttacking();

            isRetreating = true;
            isMoving = true;

            currentTargetAttacker = null;
            currentTargetAttackersHealth = null;

            anim.SetBool("isMoving", true);
            if (squad.isRangedUnit && myShooter != null)
                anim.SetBool("isShooting", false);

            if (squad.squadType == SquadType.Laborers)
            {
                Laborer laborer = GetComponent<Laborer>();
                if (laborer.isWorking)
                    laborer.StopWorking();
            }

            // While the defender is still within the bounds of the visible map (-1.25 on the x is outside of our camera's view)
            while (transform.position.x > -1.25f && health.isDead == false)
            {
                if (transform.localScale.x != -1)
                    transform.localScale = new Vector2(-1, 1); // Flip the sprite

                if (transform.position.x > 0.25f || (transform.position.x <= 0.25 && transform.position.y >= 2.9f && transform.position.y <= 3.1f))
                    transform.Translate(Vector2.left * runSpeed * 2 * Time.deltaTime); // Retreat double the speed
                else if (transform.position.x <= 0.25f && transform.position.y < 2.9f)
                {
                    sr.sortingOrder = -650;
                    transform.Translate(Vector2.up * runSpeed * 2 * Time.deltaTime);
                }
                else if (transform.position.x <= 0.25f && transform.position.y > 3.1f)
                {
                    sr.sortingOrder = -650;
                    transform.Translate(Vector2.down * runSpeed * 2 * Time.deltaTime);
                }

                if (transform.position.x <= -2.25f)
                {
                    Destroy(squad.gameObject); // Destroy the squad
                    break;
                }

                yield return null;
            }
        }
    }

    public void AddGold(int amount)
    {
        currencyDisplay.AddGold(amount);
    }

    public void MoveUnitIntoPosition()
    {
        Vector2 currentLocalPosition = transform.localPosition;
        if (currentLocalPosition != unitPosition && Vector2.Distance(currentLocalPosition, unitPosition) > minDistanceFromTargetPosition)
        {
            isMoving = true;
            anim.SetBool("isMoving", true);

            FaceLocalTarget(unitPosition);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.parent.position.x, transform.parent.position.y) + unitPosition, currentSpeed * Time.deltaTime);
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                anim.SetBool("isMoving", false);
            }

            if (laborer == null || (laborer != null && laborer.isWorking == false))
            {
                if (transform.localScale.x != 1)
                    transform.localScale = new Vector2(1, 1);
            }
        }
    }

    void MoveTowardsAttacker()
    {
        isMoving = true;
        anim.SetBool("isMoving", true);

        if (isAttacking == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTargetAttacker.transform.position, currentSpeed * Time.deltaTime);

            FaceTarget(currentTargetAttacker.transform.position);
        }
    }

    void AttackNearestAttacker()
    {
        if (squad.attackersNearby.Count > 0)
        {
            Attacker nearestAttacker = null;

            // Find the nearest attacker by comparing the distances from each attacker to this defender
            foreach (Attacker attacker in squad.attackersNearby)
            {
                if (nearestAttacker == null) // If a nearestAttacker hasn't been assigned yet, automatically assign the first attacker in our attackersNearby list
                    nearestAttacker = attacker;
                else // Then compare distances for each attacker
                {
                    if (Vector2.Distance(attacker.transform.position, transform.position) < Vector2.Distance(nearestAttacker.transform.position, transform.position))
                    {
                        // If this attacker is closer than the currently assigned nearestAttacker, replace it with this attacker
                        nearestAttacker = attacker;
                    }
                }
            }

            currentTargetAttacker = nearestAttacker;
            if (currentTargetAttacker != null)
                currentTargetAttackersHealth = currentTargetAttacker.health;
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
    
    public IEnumerator Knockback()
    {
        float knockbackDirection = -1f;
        for (int i = 0; i < squad.attackersNearby.Count; i++)
        {
            if (squad.attackersNearby[i].currentTargetDefender == this)
            {
                if (squad.attackersNearby[i].transform.position.x >= transform.position.x)
                    knockbackDirection = -1f;
                else
                    knockbackDirection = 1f;

                break;
            }
        }

        Vector2 knockbackDestination = new Vector2(transform.position.x + (knockbackDistance * knockbackDirection), transform.position.y);

        isBeingKnockedBack = true;
        while (isBeingKnockedBack && isRetreating == false)
        {
            StopAttacking();

            transform.position = Vector2.MoveTowards(transform.position, knockbackDestination, knockbackSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, knockbackDestination) <= minDistanceFromTargetPosition)
                isBeingKnockedBack = false;

            yield return null;
        }
    }

    public void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);

        if (currentTargetAttacker != null)
        {
            currentTargetAttackersHealth = currentTargetAttacker.health;

            if (currentTargetAttacker.currentTargetDefender == null)
            {
                currentTargetAttacker.currentTargetDefender = this;
                currentTargetAttacker.currentTargetsSquad = squad;
            }
        }
    }

    public void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }

    public void StrikeCurrentTarget()
    {
        if (currentTargetAttacker == null) return;

        FaceTarget(currentTargetAttacker.transform.position);

        if (currentTargetAttackersHealth != null)
        {
            if (currentTargetAttackersHealth.isDead)
            {
                currentTargetAttacker.FindNewTargetForDefenders(currentTargetAttacker);
                return;
            }
            else
            {
                // Deal damage to self if enemy has thorns active
                if (currentTargetAttackersHealth.thornsActive && currentTargetAttackersHealth.thornsDamageMultiplier > 0f)
                    health.TakeDamage(bluntDamage * currentTargetAttackersHealth.thornsDamageMultiplier, slashDamage * currentTargetAttackersHealth.thornsDamageMultiplier, piercingDamage * currentTargetAttackersHealth.thornsDamageMultiplier, fireDamage * currentTargetAttackersHealth.thornsDamageMultiplier, true, false);

                currentTargetAttackersHealth.TakeDamage(bluntDamage, slashDamage, piercingDamage, fireDamage, false, shouldKnockback);
            }
        }

        audioManager.PlayMeleeHitSound(meleeWeaponType);
    }

    void UpdateAnimationState()
    {
        if (currentTargetAttacker == null && isAttacking)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }

    void UpdateSortingLayer()
    {
        if (isRetreating == false)
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);

        effectSR1.sortingOrder = sr.sortingOrder - 1;
        effectSR2.sortingOrder = sr.sortingOrder - 2;
        effectSR3.sortingOrder = sr.sortingOrder - 3;
    }

    public void FindNewTargetForAttackers(Defender defender)
    {
        currentTargetAttacker = null;
        currentTargetAttackersHealth = null;

        if (transform.parent == defender.squad.unitsParent) // Remove the defender from the units list
            defender.squad.units.Remove(defender);
        else if (transform.parent == defender.squad.leaderParent)
        {
            defender.squad.Retreat(); // The leader was killed, so retreat the remaining units in the squad
            defender.squad.leader = null;
        }

        foreach (Attacker attacker in defender.squad.attackersNearby) // For each attacker in range of the defender who died...
        {
            Defender theCurrentDefenderBeingAttacked = attacker.currentTargetDefender;

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
                attacker.currentTargetDefender = null;
                attacker.currentTargetsHealth = null;
                attacker.isAttacking = false;

                if (attacker.opponents.Count > 0) // If the attacker already has another defender (opponent) attacking him
                {
                    attacker.currentTargetDefender = attacker.opponents[0];
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
                        attacker.currentTargetDefender = defender.squad.leader;
                        attacker.currentTargetsHealth = defender.squad.leader.health;
                        attacker.opponents.Add(defender.squad.leader);

                        if (defender.squad.leader.currentTargetAttacker == null)
                        {
                            defender.squad.leader.currentTargetAttacker = attacker;
                            defender.squad.leader.currentTargetAttackersHealth = attacker.health;
                        }

                        if (Vector2.Distance(attacker.transform.position, defender.squad.leader.transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                    else if (attacker.opponents.Contains(defender.squad.units[randomTargetIndex]) == false)
                    {
                        // Attack a random unit in the squad
                        attacker.currentTargetDefender = defender.squad.units[randomTargetIndex];
                        attacker.currentTargetsHealth = defender.squad.units[randomTargetIndex].health;
                        attacker.opponents.Add(defender.squad.units[randomTargetIndex]);

                        if (defender.squad.units[randomTargetIndex].currentTargetAttacker == null)
                        {
                            defender.squad.units[randomTargetIndex].currentTargetAttacker = attacker;
                            defender.squad.units[randomTargetIndex].currentTargetAttackersHealth = attacker.health;
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
                    if (potentialOpponent.currentTargetAttacker == null && attacker.opponents.Contains(potentialOpponent) == false)
                    {
                        potentialOpponent.currentTargetAttacker = attacker;
                        attacker.opponents.Add(potentialOpponent);
                        if (attacker.maxOpponents == attacker.opponents.Count)
                            return;
                    }
                }

                if (attacker.maxOpponents > attacker.opponents.Count && defender.squad.leader != null && attacker.opponents.Contains(defender.squad.leader) == false)
                {
                    defender.squad.leader.currentTargetAttacker = attacker;
                    attacker.opponents.Add(defender.squad.leader);
                }
            }
        }
    }

    public void GetNewTarget()
    {
        StopAttacking();

        if (squad.attackersNearby.Contains(currentTargetAttacker))
            squad.attackersNearby.Remove(currentTargetAttacker);

        currentTargetAttacker = null;
        currentTargetAttackersHealth = null;

        if (squad.attackersNearby.Count > 0)
        {
            currentTargetAttacker = squad.attackersNearby[0];
            currentTargetAttackersHealth = squad.attackersNearby[0].health;
        }
    }

    public void SetAttackDamage(float bluntDamageAddOn, float slashDamageAddOn, float piercingDamageAddOn, float fireDamageAddOn)
    {
        bluntDamage += Mathf.RoundToInt(bluntDamageAddOn);
        slashDamage += Mathf.RoundToInt(slashDamageAddOn);
        piercingDamage += Mathf.RoundToInt(piercingDamageAddOn);
        fireDamage += Mathf.RoundToInt(fireDamageAddOn);
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x && transform.localScale.x != -1)
            transform.localScale = new Vector2(-1, 1);
        else if (targetPosition.x >= transform.position.x && transform.localScale.x != 1)
            transform.localScale = new Vector2(1, 1);
    }

    public void FaceLocalTarget(Vector2 targetLocalPosition)
    {
        if (targetLocalPosition.x < transform.localPosition.x && transform.localScale.x != -1)
            transform.localScale = new Vector2(-1, 1);
        else if (targetLocalPosition.x >= transform.localPosition.x && transform.localScale.x != 1)
            transform.localScale = new Vector2(1, 1);
    }
}