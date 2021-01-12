using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] GameObject gun;

    [HideInInspector] public AttackerSpawner myLaneSpawner;

    GameObject projectilesParent;
    const string PROJECTILES_PARENT_NAME = "Projectiles";

    Animator anim;
    Defender defender;
    bool isAttackerInLane;
    int randomIndex;

    void Start()
    {
        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME);
        if (projectilesParent == null)
            projectilesParent = new GameObject(PROJECTILES_PARENT_NAME);

        anim = GetComponent<Animator>();
        defender = GetComponent<Defender>();

        SetLaneSpawner();
    }

    void Update()
    {
        // Set animation state
        if (IsAttackerInLane())
            anim.SetBool("isShooting", true);
        else
            anim.SetBool("isShooting", false);
    }

    public void Fire()
    {
        Projectile newProjectile = Instantiate(projectile, gun.transform.position, transform.rotation);
        newProjectile.transform.SetParent(projectilesParent.transform);
        newProjectile.myShooter = this;

        randomIndex = Random.Range(0, myLaneSpawner.transform.childCount);
        newProjectile.targetTransform = myLaneSpawner.transform.GetChild(randomIndex).transform;
    }

    void SetLaneSpawner()
    {
        AttackerSpawner[] attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            bool isCloseEnough = (Mathf.Abs(spawner.transform.position.y - defender.squad.transform.position.y) <= Mathf.Epsilon);

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
