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
    public float bluntDamage, piercingDamage, fireDamage, healAmount;
    [HideInInspector] public float startingBluntDamage, startingPiercingDamage, startingFireDamage, startingHealAmount;
    public float secondaryRangedDamageMultiplier = 1f;
    public bool shouldKnockback;

    [HideInInspector] public bool isShootingSecondaryProjectile;
    [HideInInspector] public bool isShootingCastle;
    [HideInInspector] public bool isHealer;
    [HideInInspector] public Attacker attacker;
    [HideInInspector] public Defender defender;

    Defender currentHealingTarget;
    bool isShooting, isHealing;

    const string PROJECTILES_PARENT_NAME = "Projectiles";
    Transform projectilesParent;

    GameObject gun;
    AudioManager audioManager;
    Animator anim;
    CastleCollider castleCollider;

    void Awake()
    {
        attacker = GetComponent<Attacker>();
        defender = GetComponent<Defender>();
        if (defender != null)
            defender.myShooter = this;
    }

    void Start()
    {
        audioManager = AudioManager.instance;
        anim = GetComponent<Animator>();
        castleCollider = CastleCollider.instance;

        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME).transform;
        gun = transform.Find("Gun").gameObject;

        startingBluntDamage = bluntDamage;
        startingPiercingDamage = piercingDamage;
        startingFireDamage = fireDamage;
        startingHealAmount = healAmount;

        if (startingHealAmount > 0)
            isHealer = true;

        // Find which object pool to use for this shooter's projectiles
        for (int i = 0; i < projectilesParent.childCount; i++)
        {
            if (projectilesParent.GetChild(i).TryGetComponent(out ObjectPool objPool))
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
        if (isHealer)
        {
            if (defender != null)
                currentHealingTarget = defender.squad.rangeCollider.GetFurthestDefenderWithLowHealth();

            if (currentHealingTarget == null && isHealing)
                StopHealing();
        }

        if ((defender != null && defender.isRetreating == false && defender.squad.squadPlaced && Vector2.Distance(transform.localPosition, defender.unitPosition) <= defender.minDistanceFromTargetPosition && defender.squad.attackersNearby.Count == 0 && ((isHealer == false && defender.squad.rangeCollider.attackersInRange.Count > 0) || (isHealer && defender.squad.rangeCollider.defendersInRange.Count > 0 && currentHealingTarget != null)))
            || (attacker != null && (attacker.rangeCollider.defendersInRange.Count > 0 || isShootingCastle || attacker.currentTargetResourceDeposit != null || attacker.currentTargetObstacle != null)) && transform.position.x < 9.5f)
        {
            if (isHealer == false && isShooting == false)
                StartCoroutine(StartShooting());
            else if (isHealer && isHealing == false)
                StartCoroutine(StartHealing());
        }
        else
        {
            if (isHealing)
                StopHealing();
            else if (isShooting)
                StopShooting();
        }
    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1.5f));
        isShooting = true;
        anim.SetBool("isShooting", true);
    }

    void StopShooting()
    {
        isShooting = false;
        anim.SetBool("isShooting", false);
    }

    IEnumerator StartHealing()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1.5f));
        isHealing = true;
        anim.SetBool("isHealing", true);
    }

    void StopHealing()
    {
        anim.Play("Idle", 0);
        isHealing = false;
        anim.SetBool("isHealing", false);
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

        if (defender != null && (defender.squad.rangeCollider.attackersInRange.Count > randomIndex || defender.squad.rangeCollider.defendersInRange.Count > randomIndex))
        {
            if (isHealer == false) // If not a healer
                AssignTargetToProjectile(newProjectile, defender.squad.rangeCollider.attackersInRange[randomIndex].transform);
            else if (currentHealingTarget != null) // If healer
                AssignTargetToProjectile(newProjectile, currentHealingTarget.transform);
            else
                newProjectile.Deactivate();

            StartCoroutine(newProjectile.ShootProjectile());
        }
        else if (attacker != null)
        {
            if (attacker.rangeCollider.defendersInRange.Count > randomIndex)
            {
                AssignTargetToProjectile(newProjectile, attacker.rangeCollider.defendersInRange[randomIndex].transform);
                StartCoroutine(newProjectile.ShootProjectile());
            }
            else if (attacker.currentTargetObstacle != null)
            {
                newProjectile.targetPos = new Vector3(attacker.currentTargetObstacle.transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y);
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
        if (target != null)
        {
            projectile.target = target;
            projectile.targetPos = target.position;
        }
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

    public void SetHealAmount(float healAmountAddOn)
    {
        healAmount += Mathf.RoundToInt(healAmountAddOn);
    }

    /*bool IsAttackerInLane()
    {
        if (defender.squad.myLaneSpawner.transform.childCount <= 0)
            return false;
        else
            return true;
    }*/
}
