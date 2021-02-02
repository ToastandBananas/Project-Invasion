using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite defaultSprite;

    [Tooltip("The sprite that will be used when the projectile hits the ground")]
    public Sprite groundedSprite;

    [Tooltip("Damage of the projectile")]
    [SerializeField] float damage = 10f;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 5f;

    [Tooltip("Multiplier for the arc height, which is dependent on the distance from the target (set to 0 for no arc)")]
    public float arcMultiplier = 0.1f;

    [Tooltip("Position we want to hit")]
    public Transform targetTransform;

    [HideInInspector] public Shooter myShooter;

    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;

    bool moveProjectile = true;
    Vector3 startPos, nextPos;
    float x0, x1, dist, nextX, baseY, arc, arcHeight;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public IEnumerator ShootProjectile()
    {
        moveProjectile = true;

        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        startPos = transform.position;

        x0 = startPos.x;
        x1 = targetTransform.position.x;
        dist = x1 - x0;
        arcHeight = dist * arcMultiplier;

        float random = Random.Range(0f, 100f);
        Vector3 offset = Vector3.zero;

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

        while (moveProjectile)
        {
            // Compute the next position, with arc added in
            x0 = startPos.x;
            x1 = targetTransform.position.x + offset.x;
            dist = x1 - x0;
            nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
            baseY = Mathf.Lerp(startPos.y, targetTransform.position.y + offset.y, (nextX - x0) / dist);
            arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

            // Rotate to face the next position, and then move there
            transform.rotation = LookAt2D(nextPos - transform.position);
            transform.position = nextPos;

            // Do something when we reach the target
            if (nextPos == targetTransform.position + offset && gameObject.activeInHierarchy)
                StartCoroutine(Arrived());

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent<Attacker>(out Attacker attacker);
        collision.TryGetComponent<Defender>(out Defender defender);

        if ((attacker != null && myShooter.defender != null && attacker.myAttackerSpawner == myShooter.defender.squad.myLaneSpawner)
            || (defender != null && myShooter.attacker != null && defender.squad.myLaneSpawner == myShooter.attacker.myAttackerSpawner))
        {
            Health health = collision.GetComponent<Health>();

            if (health != null) StartCoroutine(HitTarget(health));
        }

    }

    IEnumerator Arrived()
    {
        moveProjectile = false;
        boxCollider.enabled = false;
        
        if (groundedSprite != null)
        {
            sr.sprite = groundedSprite;
            sr.sortingOrder = 2;
            transform.localScale = new Vector2(0.9f, 0.9f);
        }

        if (anim != null)
            anim.SetBool("hitTarget", true);

        yield return new WaitForSeconds(10f);
        Deactivate();
    }

    IEnumerator HitTarget(Health health)
    {
        // Reduce health
        health.DealDamage(damage);
        moveProjectile = false;

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
}
