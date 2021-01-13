using System.Collections;
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

    void FixedUpdate()
    {
        if (defender.isRetreating == false && IsAttackerInLane())
        {
            if (anim.GetBool("isShooting") == false)
                StartCoroutine(StartShooting());
        }
        else
            anim.SetBool("isShooting", false);
    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1.5f));
        anim.SetBool("isShooting", true);
    }

    public void Fire()
    {
        Projectile newProjectile = Instantiate(projectile, gun.transform.position, transform.rotation);
        newProjectile.transform.SetParent(projectilesParent.transform);
        newProjectile.myShooter = this;

        randomIndex = Random.Range(0, myLaneSpawner.transform.childCount);
        if (myLaneSpawner.transform.GetChild(randomIndex) != null)
            newProjectile.targetTransform = myLaneSpawner.transform.GetChild(randomIndex).transform;
        else
            Destroy(newProjectile.gameObject);
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
