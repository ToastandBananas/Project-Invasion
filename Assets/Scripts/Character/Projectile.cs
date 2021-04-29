using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite defaultSprite;

    [Tooltip("The sprite that will be used when the projectile hits the ground")]
    public Sprite groundedSprite;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 3f;

    [Tooltip("Multiplier for the arc height, which is dependent on the distance from the target (set to 0 for no arc)")]
    public float arcMultiplier = 0.1f;

    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Shooter myShooter;

    AudioManager audioManager;
    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;

    bool moveProjectile = true;

    void Awake()
    {
        audioManager = AudioManager.instance;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public IEnumerator ShootProjectile()
    {
        moveProjectile = true;

        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        Vector3 startPos = transform.position;

        float x0 = startPos.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        float arcHeight = dist * arcMultiplier;
        if (targetPos.x < myShooter.transform.position.x)
            arcHeight *= -1f;

        float random = Random.Range(0f, 100f);
        Vector3 offset = Vector3.zero;

        // If the shooter is missing
        if (random > myShooter.accuracy)
        {
            float offsetXFromMiss, offsetYFromMiss;
            offsetXFromMiss = Random.Range(0.1f, 0.4f);
            offsetYFromMiss = Random.Range(0.1f, 0.4f);

            // Randomize whether the offsets will be negative or positive values
            int randomX, randomY;
            randomX = Random.Range(0, 2);
            randomY = Random.Range(0, 2);

            if (randomX == 0)
                offsetXFromMiss *= -1f;
            if (randomY == 0)
                offsetYFromMiss *= -1f;

            offset = new Vector3(offsetXFromMiss, offsetYFromMiss);
        }

        audioManager.PlayShootOrThrowSound(myShooter.rangedWeaponType);

        while (moveProjectile)
        {
            UpdateSortingLayer();

            if (target != null)
                targetPos = target.position;

            // Compute the next position, with arc added in
            x0 = startPos.x;
            x1 = targetPos.x + offset.x;
            dist = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startPos.y, targetPos.y + offset.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

            // Rotate to face the next position, and then move there
            transform.rotation = LookAt2D(nextPos - transform.position);
            transform.position = nextPos;

            // Do something when we reach the target
            if (nextPos == targetPos + offset && gameObject.activeInHierarchy)
                StartCoroutine(Arrived());

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Attacker attacker);
        collision.TryGetComponent(out Defender defender);

        if (gameObject.activeInHierarchy)
        {
            if ((attacker != null && myShooter.defender != null && (attacker.myAttackerSpawner == myShooter.defender.squad.myLaneSpawner || myShooter.defender.squad.isCastleWallSquad)) // If shooting an attacker
                || (defender != null && myShooter.attacker != null && defender.squad.myLaneSpawner == myShooter.attacker.myAttackerSpawner)) // If shooting a defender
            {
                Health health = collision.GetComponent<Health>();

                if (health != null)
                    StartCoroutine(HitTarget(health, false));
                else
                    Deactivate();
            }
            else if (myShooter.attacker != null) // If this is an attacker
            {
                if (myShooter.attacker.canAttackNodes && myShooter.attacker.currentTargetNode != null) // If attacking a Resource Node
                {
                    if (myShooter.attacker.currentTargetResourceDeposit != null) // If attacking a Resource Deposit
                    {
                        collision.TryGetComponent(out ResourceDeposit resourceDeposit);
                        if (resourceDeposit != null && resourceDeposit.resourceNode.myLaneSpawner == myShooter.attacker.myAttackerSpawner && myShooter.attacker.currentTargetResourceDeposit == resourceDeposit)
                        {
                            HitTarget(null, true);

                            resourceDeposit.TakeDamage(myShooter.attacker.structuralAttackDamage);

                            // Retreat the laborers if the deposit is getting attacked
                            foreach (ResourceDeposit deposit in resourceDeposit.resourceNode.resourceDeposits)
                            {
                                if (deposit.myLaborer != null)
                                {
                                    if (deposit.myLaborer.isRetreating == false)
                                        deposit.myLaborer.squad.Retreat();

                                    deposit.myLaborer = null;
                                }
                            }

                            // If the resource deposit is destroyed, find new target deposits for each Attacker attacking this resource deposit
                            if (resourceDeposit.currentHealth <= 0)
                            {
                                for (int i = 0; i < myShooter.attacker.myAttackerSpawner.transform.childCount; i++)
                                {
                                    Attacker attackerInLane = myShooter.attacker.myAttackerSpawner.transform.GetChild(i).GetComponent<Attacker>();
                                    if (attackerInLane.currentTargetResourceDeposit == resourceDeposit)
                                        attackerInLane.FindNewTargetDeposit();
                                }
                            }

                            if (PlayerPrefsController.DamagePopupsEnabled())
                                TextPopup.CreateDamagePopup(transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0f, 0.15f)), myShooter.attacker.structuralAttackDamage, false, true);
                        }
                    }
                }
                else // If attacking the castle
                {
                    collision.TryGetComponent(out CastleCollider castleCollider);
                    if (castleCollider != null)
                    {
                        HitTarget(null, true);

                        CastleHealth.instance.TakeHealth(myShooter.attacker.structuralAttackDamage);

                        if (PlayerPrefsController.DamagePopupsEnabled())
                            TextPopup.CreateDamagePopup(transform.position + new Vector3(Random.Range(-0.2f, -0.1f), Random.Range(0f, 0.15f)), myShooter.attacker.structuralAttackDamage, false, true);
                    }
                }
            }
        }
    }

    IEnumerator Arrived()
    {
        moveProjectile = false;
        boxCollider.enabled = false;
        
        if (groundedSprite != null)
        {
            sr.sprite = groundedSprite;
            sr.sortingOrder = -7000;
            transform.localScale = new Vector2(0.9f, 0.9f);
        }

        if (anim != null)
            anim.SetBool("hitTarget", true);

        audioManager.PlayRangedHitSound(myShooter.rangedWeaponType, true);

        yield return new WaitForSeconds(10f);
        Deactivate();
    }

    IEnumerator HitTarget(Health health, bool isAttackingBuilding)
    {
        moveProjectile = false;

        // Reduce health
        if (health != null)
            health.TakeDamage(myShooter.bluntDamage, 0, myShooter.piercingDamage, myShooter.fireDamage, false, myShooter.shouldKnockback);
        
        audioManager.PlayRangedHitSound(myShooter.rangedWeaponType, isAttackingBuilding);

        // For projectiles that have an animation, such as a fireball
        if (anim != null)
        {
            anim.SetBool("hitTarget", true);
            yield return new WaitForSeconds(1f);
        }

        Deactivate();
    }

    public void Deactivate()
    {
        transform.localScale = new Vector2(1f, 1f);
        boxCollider.enabled = true;

        sr.sprite = defaultSprite;
        sr.sortingOrder = 7;

        target = null;

        if (anim != null)
        {
            anim.SetBool("hitTarget", false);
            anim.Rebind();
            anim.Update(0f);
        }

        gameObject.SetActive(false);
    }
    
    /// This is a 2D version of Quaternion.LookAt; it returns a quaternion
    /// that makes the local +X axis point in the given forward direction.
    /// 
    /// Forward direction:
    /// Quaternion that rotates +X to align with forward
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    void UpdateSortingLayer()
    {
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}
