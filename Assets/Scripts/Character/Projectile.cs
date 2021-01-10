using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Damage of the projectile")]
    [SerializeField] float damage = 10f;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 5f;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1f;

    [Tooltip("Position we want to hit")]
    public Vector3 targetPos;

    Vector3 startPos, nextPos;
    float x0, x1, dist, nextX, baseY, arc;

    void Start()
    {
        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        startPos = transform.position;
    }

    void Update()
    {
        // transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Compute the next position, with arc added in
        x0 = startPos.x;
        x1 = targetPos.x;
        dist = x1 - x0;
        nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
        arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(nextPos - transform.position);
        transform.position = nextPos;

        // Do something when we reach the target
        // if (nextPos == targetPos) Arrived();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var health = collision.GetComponent<Health>();
        var attacker = collision.GetComponent<Attacker>();

        if (attacker && health)
        {
            // Reduce health
            health.DealDamage(damage);
            Destroy(gameObject);
        }
    }

    void Arrived()
    {
        Destroy(gameObject);
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
