using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectile, gun;

    AttackerSpawner myLaneSpawner;
    Animator anim;
    bool isAttackerInLane;

    void Start()
    {
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
        Instantiate(projectile, gun.transform.position, transform.rotation);
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
