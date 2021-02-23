using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    ObjectPool projectileObjectPool;

    public RangedWeaponType rangedWeaponType;
    [SerializeField][Range(0f, 100f)] float accuracy = 100f;
    [SerializeField] float shootDamage = 10f;

    [HideInInspector] public bool isShootingCastle;
    [HideInInspector] public Attacker attacker;
    [HideInInspector] public Defender defender;

    const string PROJECTILES_PARENT_NAME = "Projectiles";
    Transform projectilesParent;

    GameObject gun;
    AudioManager audioManager;
    Animator anim;
    CastleCollider castleCollider;
    int randomIndex;

    void Start()
    {
        audioManager = AudioManager.instance;
        anim = GetComponent<Animator>();
        castleCollider = CastleCollider.instance;
        attacker = GetComponent<Attacker>();
        defender = GetComponent<Defender>();

        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME).transform;
        gun = transform.Find("Gun").gameObject;

        // Find which object pool to use for this shooter's projectiles
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
        if ((defender != null && defender.squad.squadPlaced && defender.isRetreating == false && defender.squad.rangeCollider.attackersInRange.Count > 0 && defender.squad.attackersNearby.Count == 0)
            || (attacker != null && (attacker.rangeCollider.defendersInRange.Count > 0 || isShootingCastle)))
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
        
        if (defender != null)
            randomIndex = Random.Range(0, defender.squad.rangeCollider.attackersInRange.Count);
        else if (attacker != null)
            randomIndex = Random.Range(0, attacker.rangeCollider.defendersInRange.Count);

        if (defender != null && defender.squad.rangeCollider.attackersInRange.Count > randomIndex)
        {
            newProjectile.target = defender.squad.rangeCollider.attackersInRange[randomIndex].transform;
            newProjectile.targetPos = newProjectile.target.position;
            StartCoroutine(newProjectile.ShootProjectile());
        }
        else if (attacker != null && attacker.rangeCollider.defendersInRange.Count > randomIndex)
        {
            newProjectile.target = attacker.rangeCollider.defendersInRange[randomIndex].transform;
            newProjectile.targetPos = newProjectile.target.position;
            StartCoroutine(newProjectile.ShootProjectile());
        }
        else if (isShootingCastle)
        {
            newProjectile.targetPos = new Vector3(Random.Range(-0.2f, 0f), transform.position.y);
            StartCoroutine(newProjectile.ShootProjectile());
        }
        else
            newProjectile.Deactivate();
    }

    // For use as a keyframe in the shooter's animation
    public void PlayDrawBowSound()
    {
        audioManager.PlayRandomSound(audioManager.bowDrawSounds);
    }

    public float GetRangedDamage()
    {
        return shootDamage;
    }

    public void SetShootDamage(float newDamage)
    {
        shootDamage = newDamage;
    }

    public float GetRangedAccuracy()
    {
        return accuracy;
    }

    public void SetShootAccuracy(float newAccuracy)
    {
        accuracy = newAccuracy;
    }

    /*bool IsAttackerInLane()
    {
        if (defender.squad.myLaneSpawner.transform.childCount <= 0)
            return false;
        else
            return true;
    }*/
}
