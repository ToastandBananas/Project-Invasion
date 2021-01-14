using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] GameObject gun;

    [HideInInspector] public Defender defender;

    [Range(0f, 100f)] public float accuracy = 100f;

    Transform projectilesParent;
    const string PROJECTILES_PARENT_NAME = "Projectiles";

    Animator anim;
    bool isAttackerInLane;
    int randomIndex;

    void Start()
    {
        anim = GetComponent<Animator>();
        defender = GetComponent<Defender>();

        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME).transform;
        if (projectilesParent == null)
            projectilesParent = new GameObject(PROJECTILES_PARENT_NAME).transform;

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

        newProjectile.transform.position = gun.transform.position;
        newProjectile.transform.rotation = transform.rotation;
        newProjectile.transform.SetParent(projectilesParent.transform);
        newProjectile.myShooter = this;
        newProjectile.gameObject.SetActive(true);

        randomIndex = Random.Range(0, defender.squad.myLaneSpawner.transform.childCount);
        if (defender.squad.myLaneSpawner.transform.childCount > randomIndex)
            newProjectile.targetTransform = defender.squad.myLaneSpawner.transform.GetChild(randomIndex).transform;
        else
            Destroy(newProjectile.gameObject);
    }

    bool IsAttackerInLane()
    {
        if (defender.squad.myLaneSpawner.transform.childCount <= 0)
            return false;
        else
            return true;
    }
}
