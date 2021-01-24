using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject gun;

    public GameObject projectilePrefab;
    ObjectPool projectileObjectPool;

    [HideInInspector] public Defender defender;

    [Range(0f, 100f)] public float accuracy = 100f;

    const string PROJECTILES_PARENT_NAME = "Projectiles";
    Transform projectilesParent;

    Animator anim;
    bool isAttackerInLane;
    int randomIndex;

    void Start()
    {
        anim = GetComponent<Animator>();
        defender = GetComponent<Defender>();

        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME).transform;

        // Find which object pool to use
        for (int i = 0; i < projectilesParent.childCount; i++)
        {
            if (projectilesParent.GetChild(i).TryGetComponent<ObjectPool>(out ObjectPool objPool))
            {
                if (objPool.objectToPool == projectilePrefab)
                {
                    projectileObjectPool = objPool;
                    return;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (defender.isRetreating == false && defender.squad.rangeCollider.attackersInRange.Count > 0 && defender.squad.attackersNearby.Count == 0)
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
        Projectile newProjectile = projectileObjectPool.GetPooledObject().GetComponent<Projectile>();

        newProjectile.gameObject.SetActive(true);
        newProjectile.transform.position = gun.transform.position;
        newProjectile.transform.rotation = transform.rotation;
        newProjectile.myShooter = this;
        
        randomIndex = Random.Range(0, defender.squad.rangeCollider.attackersInRange.Count);
        if (defender.squad.rangeCollider.attackersInRange.Count > randomIndex)
        {
            newProjectile.targetTransform = defender.squad.rangeCollider.attackersInRange[randomIndex].transform;
            StartCoroutine(newProjectile.ShootProjectile());
        }
        else
            newProjectile.Deactivate();
    }

    /*bool IsAttackerInLane()
    {
        if (defender.squad.myLaneSpawner.transform.childCount <= 0)
            return false;
        else
            return true;
    }*/
}
