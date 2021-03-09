using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;
    [SerializeField][Range(-1f, 1f)] float bluntResistance, slashResistance, piercingResistance, fireResistance;

    [Header("Damage Particle Effect")]
    [SerializeField] GameObject damageEffect;

    [HideInInspector] public bool isDead = false;

    LevelController levelController;
    Attacker attacker;
    Defender defender;
    Animator anim;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    Transform deadCharactersParent;

    float damageAmount;
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
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            SetCurrentHealthToMaxHealth();
    }

    public void SetMaxHealth(float newMaxHealthAmount)
    {
        maxHealth = newMaxHealthAmount;
        currentHealth = maxHealth;
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

    public void DealDamage(float bluntDamage, float slashDamage, float piercingDamage, float fireDamage)
    {
        damageAmount = 0f;
        if (bluntDamage > 0f)    damageAmount += GetDamageAmount(bluntDamage, bluntResistance);
        if (slashDamage > 0f)    damageAmount += GetDamageAmount(slashDamage, slashResistance);
        if (piercingDamage > 0f) damageAmount += GetDamageAmount(piercingDamage, piercingResistance);
        if (fireDamage > 0f)     damageAmount += GetDamageAmount(fireDamage, fireResistance);

        if (damageAmount >= 0f && damageAmount < 1f)
            damageAmount = 1f;
        else
            damageAmount = Mathf.RoundToInt(damageAmount);

        currentHealth -= damageAmount;
        
        if (damageEffect != null)
            StartCoroutine(TriggerDamageEffect());

        // If the character is going to die
        if (currentHealth <= 0 && isDead == false)
        {
            FindNewTargetForOpponent();
            Die();
        }
    }

    float GetDamageAmount(float damage, float resistance)
    {
        return damage - (damage * resistance);
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        boxCollider.enabled = false;
        spriteRenderer.sortingOrder = 4;

        float randomRotation = Random.Range(-70f, 70f);
        transform.eulerAngles = new Vector3(0, 0, randomRotation);
        transform.SetParent(deadCharactersParent);

        if (attacker != null) {
            if (levelController != null)
                levelController.AttackerKilled();
            else
                Debug.LogError("No level controller exists...fix me!");
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
