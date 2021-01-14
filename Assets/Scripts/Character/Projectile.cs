using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
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

    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    bool moveProjectile = true;
    Vector3 startPos, nextPos;
    float x0, x1, dist, nextX, baseY, arc, arcHeight;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        StartCoroutine(ShootProjectile());
    }

    public IEnumerator ShootProjectile()
    {
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
            offsetXFromMiss = Random.Range(0.2f, 0.75f);
            offsetYFromMiss = Random.Range(0.2f, 0.75f);

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

        // transform.Translate(Vector2.right * speed * Time.deltaTime); // Old method (shoot in a straight line)

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
            if (nextPos == targetTransform.position + offset && gameObject.activeInHierarchy) Arrived();

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Attacker attacker = collision.GetComponent<Attacker>();
        if (attacker && attacker.myAttackerSpawner == myShooter.defender.squad.myLaneSpawner)
        {
            Health health = collision.GetComponent<Health>();

            if (health)
            {
                // Reduce health
                health.DealDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    void Arrived()
    {
        moveProjectile = false;
        boxCollider.enabled = false;
        
        if (groundedSprite != null)
        {
            sr.sprite = groundedSprite;
            sr.sortingOrder = 1;
            transform.localScale = new Vector2(0.8f, 0.8f);
            
            Destroy(gameObject, 10f);
        }
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
