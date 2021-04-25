using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject projectilePrefab;
    public GameObject secondaryProjectilePrefab;
    ObjectPool projectileObjectPool;
    ObjectPool secondaryProjectileObjectPool;

    [Header("Weapon Stats")]
    public RangedWeaponType rangedWeaponType;
    [Range(0f, 100f)] public float accuracy = 100f;
    public float bluntDamage, piercingDamage, fireDamage;
    [HideInInspector] public float startingBluntDamage, startingPiercingDamage, startingFireDamage;
    public float secondaryRangedDamageMultiplier = 1f;
    public bool shouldKnockback;

    [HideInInspector] public bool isShootingSecondaryProjectile;
    [HideInInspector] public bool isShootingCastle;
    [HideInInspector] public Attacker attacker;
    [HideInInspector] public Defender defender;

    const string PROJECTILES_PARENT_NAME = "Projectiles";
    Transform projectilesParent;

    GameObject gun;
    AudioManager audioManager;
    Animator anim;
    CastleCollider castleCollider;

    void Start()
    {
        audioManager = AudioManager.instance;
        anim = GetComponent<Animator>();
        castleCollider = CastleCollider.instance;
        attacker = GetComponent<Attacker>();
        defender = GetComponent<Defender>();
        if (defender != null)
            defender.myShooter = this;

        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME).transform;
        gun = transform.Find("Gun").gameObject;

        startingBluntDamage = bluntDamage;
        startingPiercingDamage = piercingDamage;
        startingFireDamage = fireDamage;

        // Find which object pool to use for this shooter's projectiles
        for (int i = 0; i < projectilesParent.childCount; i++)
        {
            if (projectilesParent.GetChild(i).TryGetComponent<ObjectPool>(out ObjectPool objPool))
            {
                if (objPool.objectToPool == projectilePrefab)
                    projectileObjectPool = objPool;
                else if (objPool.objectToPool == secondaryProjectilePrefab)
                    secondaryProjectileObjectPool = objPool;
            }
        }
    }

    void FixedUpdate()
    {
        if ((defender != null && Vector2.Distance(transform.localPosition, defender.unitPosition) <= defender.minDistanceFromTargetPosition && defender.squad.squadPlaced && defender.isRetreating == false && defender.squad.rangeCollider.attackersInRange.Count > 0 && defender.squad.attackersNearby.Count == 0)
            || (attacker != null && (attacker.rangeCollider.defendersInRange.Count > 0 || isShootingCastle || attacker.currentTargetResourceDeposit != null)) && transform.position.x < 9.5f)
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
        Projectile newProjectile;
        if (isShootingSecondaryProjectile == false)
            newProjectile = projectileObjectPool.GetPooledObject().GetComponent<Projectile>();
        else
            newProjectile = secondaryProjectileObjectPool.GetPooledObject().GetComponent<Projectile>();

        newProjectile.gameObject.SetActive(true);
        newProjectile.transform.position = gun.transform.position;
        newProjectile.transform.rotation = transform.rotation;
        newProjectile.myShooter = this;

        int randomIndex = 0;
        if (defender != null && defender.squad.rangeCollider.attackersInRange.Count > 0)
            randomIndex = Random.Range(0, defender.squad.rangeCollider.attackersInRange.Count);
        else if (attacker != null && attacker.rangeCollider.defendersInRange.Count > 0)
            randomIndex = Random.Range(0, attacker.rangeCollider.defendersInRange.Count);

        if (defender != null && defender.squad.rangeCollider.attackersInRange.Count > randomIndex)
        {
            AssignTargetToProjectile(newProjectile, defender.squad.rangeCollider.attackersInRange[randomIndex].transform);
            StartCoroutine(newProjectile.ShootProjectile());
        }
        else if (attacker != null)
        {
            if (attacker.rangeCollider.defendersInRange.Count > randomIndex)
            {
                AssignTargetToProjectile(newProjectile, attacker.rangeCollider.defendersInRange[randomIndex].transform);
                StartCoroutine(newProjectile.ShootProjectile());
            }
            else if (attacker.currentTargetNode != null)
            {
                if (attacker.currentTargetResourceDeposit != null)
                {
                    AssignTargetToProjectile(newProjectile, attacker.currentTargetResourceDeposit.transform);
                    StartCoroutine(newProjectile.ShootProjectile());
                }
            }
            else if (isShootingCastle)
            {
                newProjectile.targetPos = new Vector3(Random.Range(-0.2f, 0f), transform.position.y);
                StartCoroutine(newProjectile.ShootProjectile());
            }
            else
                newProjectile.Deactivate();
        }
        else
            newProjectile.Deactivate();
    }

    void AssignTargetToProjectile(Projectile projectile, Transform target)
    {
        projectile.target = target;
        projectile.targetPos = target.position;
    }

    // For use as a keyframe in the shooter's animation
    public void PlayDrawBowSound()
    {
        audioManager.PlayRandomSound(audioManager.bowDrawSounds);
    }

    public void SetRangedDamage(float bluntDamageAddOn, float piercingDamageAddOn, float fireDamageAddOn)
    {
        bluntDamage += Mathf.RoundToInt(bluntDamageAddOn);
        piercingDamage += Mathf.RoundToInt(piercingDamageAddOn);
        fireDamage += Mathf.RoundToInt(fireDamageAddOn);
    }

    /*bool IsAttackerInLane()
    {
        if (defender.squad.myLaneSpawner.transform.childCount <= 0)
            return false;
        else
            return true;
    }*/
}
