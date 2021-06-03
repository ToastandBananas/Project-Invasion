using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float maxHealth = 100f;
    [HideInInspector] public float startingMaxHealth;
    float currentHealth;
    [Range(-1f, 1f)] public float bluntResistance, slashResistance, piercingResistance, fireResistance;

    [Header("Damage Particle Effect")]
    [SerializeField] GameObject damageEffect;

    [HideInInspector] public bool thornsActive = false;
    [HideInInspector] public float thornsDamageMultiplier;

    [HideInInspector] public bool isDead = false;

    Attacker attacker;
    Defender defender;

    LevelController levelController;
    Animator anim;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    Transform deadCharactersParent;
    
    const string EFFECTS_PARENT_NAME = "Effects";
    Transform effectsParent;
    ObjectPool damageEffectObjectPool;

    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        TryGetComponent<Attacker>(out attacker);
        TryGetComponent<Defender>(out defender);
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        deadCharactersParent = GameObject.Find("Dead Characters").transform;

        startingMaxHealth = maxHealth;
        currentHealth = maxHealth;

        // Find which object pool to use for damage effects (blood, etc.)
        effectsParent = GameObject.Find(EFFECTS_PARENT_NAME).transform;
        for (int i = 0; i < effectsParent.childCount; i++)
        {
            if (effectsParent.GetChild(i).TryGetComponent<ObjectPool>(out ObjectPool objPool))
            {
                if (objPool.objectToPool == damageEffect)
                {
                    damageEffectObjectPool = objPool;
                    return;
                }
            }
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += Mathf.RoundToInt(healAmount);
        if (currentHealth > maxHealth)
            SetCurrentHealthToMaxHealth();
    }

    public void SetMaxHealth(float newMaxHealthAmount)
    {
        maxHealth = Mathf.RoundToInt(newMaxHealthAmount);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetCurrentHealthToMaxHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float bluntDamage, float slashDamage, float piercingDamage, float fireDamage, bool ignoreResistances, bool knockback)
    {
        float finalDamageAmount = 0f;

        // If we shouldn't ignore damage resistances
        if (ignoreResistances == false)
        {
            if (bluntDamage > 0f) finalDamageAmount += GetDamageAmount(bluntDamage, bluntResistance);
            if (slashDamage > 0f) finalDamageAmount += GetDamageAmount(slashDamage, slashResistance);
            if (piercingDamage > 0f) finalDamageAmount += GetDamageAmount(piercingDamage, piercingResistance);
            if (fireDamage > 0f) finalDamageAmount += GetDamageAmount(fireDamage, fireResistance);
        }
        else // Ignore resistances
        {
            finalDamageAmount += bluntDamage;
            finalDamageAmount += slashDamage;
            finalDamageAmount += piercingDamage;
            finalDamageAmount += fireDamage;
        }

        // Adjust the damage based off of current difficulty settings
        if (defender != null)
            finalDamageAmount *= PlayerPrefsController.GetDifficultyMultiplier_EnemyAttackDamage();

        if (finalDamageAmount < 1f)
            finalDamageAmount = 1f;
        else
            finalDamageAmount = Mathf.RoundToInt(finalDamageAmount);

        currentHealth -= finalDamageAmount;

        if (PlayerPrefsController.DamagePopupsEnabled())
            TextPopup.CreateDamagePopup(transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(0.05f, 0.15f)), finalDamageAmount, false, defender != null);
        
        if (damageEffect != null)
            StartCoroutine(TriggerDamageEffect());

        // If the character is going to die
        if (currentHealth <= 0 && isDead == false)
        {
            FindNewTargetForOpponent();
            Die();
        }
        else if (knockback && currentHealth > 0f) // If being knocked back
        {
            if (defender != null && defender.isLarge == false)
                StartCoroutine(defender.Knockback());
            else if (attacker != null && attacker.isLarge == false)
                StartCoroutine(attacker.Knockback());
        }

        if (defender != null && defender.squad.squadType == SquadType.Laborers && defender.isRetreating == false && currentHealth > 0)
            defender.squad.Retreat();
    }

    float GetDamageAmount(float damage, float resistance)
    {
        return damage - (damage * resistance);
    }

    void Die()
    {
        // Play the appropriate death sound
        if (defender != null)
            AudioManager.instance.PlayDeathSound(defender.voiceType);
        else if (attacker != null)
            AudioManager.instance.PlayDeathSound(attacker.voiceType);

        isDead = true;
        thornsActive = false;
        anim.SetBool("isDead", true);
        boxCollider.enabled = false;
        spriteRenderer.sortingOrder = -8000;

        // Rotate the body a random rotation
        float randomRotation = Random.Range(-70f, 70f);
        transform.eulerAngles = new Vector3(0, 0, randomRotation);
        transform.SetParent(deadCharactersParent);

        // Defender:
        if (defender != null)
        {
            // Disable character scripts to prevent further running of Update functions
            if (defender.myShooter != null)
                defender.myShooter.enabled = false;
            
            defender.enabled = false;
        }

        // Attacker:
        if (attacker != null)
        {
            // Drop supplies for the player
            if (attacker.suppliesDroppedOnDeath > 0)
            {
                ResourceDisplay.instance.AddSupplies(attacker.suppliesDroppedOnDeath);
                TextPopup.CreateResourceGainPopup(transform.position + new Vector3(0f, 0.2f), attacker.suppliesDroppedOnDeath, ResourceType.Supplies);
            }

            if (attacker.currentTargetObstacle != null)
                attacker.currentTargetObstacle.attackersNearby.Remove(attacker);

            // Disable character scripts to prevent further running of Update functions
            if (attacker.myShooter != null)
            {
                attacker.myShooter.enabled = false;
                attacker.rangeCollider.enabled = false;
            }
            
            attacker.enabled = false;

            // Subtract from total number of attackers left in the level
            levelController.AttackerKilled();
        }
    }

    public IEnumerator Resurrect(float waitToResurrectTime, Enemy enemyScript)
    {
        if (attacker != null)
        {
            levelController.AttackerResurrected();
            attacker.suppliesDroppedOnDeath = 0;
        }

        yield return new WaitForSeconds(waitToResurrectTime);

        isDead = false;
        SetCurrentHealthToMaxHealth();
        boxCollider.enabled = true;

        transform.rotation = Quaternion.Euler(Vector3.zero);

        if (attacker != null)
        {
            attacker.anim.SetBool("isDead", false);
            attacker.anim.Play("Raise");

            if (attacker.myShooter != null)
            {
                attacker.myShooter.enabled = true;
                attacker.rangeCollider.enabled = true;
            }

            if (enemyScript != null)
                enemyScript.enabled = true;

            attacker.enabled = true;

            StartCoroutine(attacker.Movement());
        }
        else if (defender != null)
        {
            defender.anim.SetBool("isDead", false);
            defender.anim.Play("Idle");

            if (defender.myShooter != null)
                defender.myShooter.enabled = true;

            defender.enabled = true;
        }
    }

    IEnumerator TriggerDamageEffect()
    {
        GameObject damageEffect = damageEffectObjectPool.GetPooledObject();
        damageEffect.transform.position = transform.position;
        damageEffect.SetActive(true);

        yield return new WaitForSeconds(20f);
        damageEffect.SetActive(false);
        damageEffect.GetComponent<ParticleSystem>().Clear();
    }

    public void FindNewTargetForOpponent()
    {
        // If this is an attacker who is dying
        if (attacker != null)
        {
            // Find a new target for each defender who was fighting the attacker who died:
            attacker.FindNewTargetForDefenders(attacker);
        }

        // If this is a defender who is dying
        if (defender != null)
        {
            // Find a new target for each attacker who was fighting the defender who died
            defender.FindNewTargetForAttackers(defender);

            // If there are no more units in the squad and the leader is dead, get rid of the Squad GameObject & free up the space so we can spawn in a new squad
            if (defender.squad != null && defender.squad.units.Count == 0 && defender.squad.leader == null)
            {
                DefenderSpawner.instance.RemoveCell(defender.squad.transform.position);
                Destroy(defender.squad.gameObject);
            }
        }
    }
}
