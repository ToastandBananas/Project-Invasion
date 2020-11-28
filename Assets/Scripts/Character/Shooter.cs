using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectile, gun;

    GameObject projectilesParent;
    const string PROJECTILES_PARENT_NAME = "Projectiles";

    AttackerSpawner myLaneSpawner;
    Animator anim;
    bool isAttackerInLane;

    void Start()
    {
        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME);
        if (projectilesParent == null)
            projectilesParent = new GameObject(PROJECTILES_PARENT_NAME);

        anim = GetComponent<Animator>();
        SetLaneSpawner();
    }

    void Update()
    {
        // Set animation state
        if (IsAttackerInLane())
            anim.SetBool("isAttacking", true);
        else
            anim.SetBool("isAttacking", false);
    }

    public void Fire()
    {
        GameObject newProjectile = Instantiate(projectile, gun.transform.position, transform.rotation);
        newProjectile.transform.SetParent(projectilesParent.transform);
    }

    void SetLaneSpawner()
    {
        AttackerSpawner[] attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            bool isCloseEnough = (Mathf.Abs(spawner.transform.position.y - transform.position.y) <= Mathf.Epsilon);

            if (isCloseEnough) myLaneSpawner = spawner;
        }
    }

    bool IsAttackerInLane()
    {
        if (myLaneSpawner.transform.childCount <= 0)
            return false;
        else
            return true;
    }
}
