using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] GameObject damageEffect;
    public bool isDead = false;

    LevelController levelController;
    Attacker attacker;
    Defender defender;
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

    public void SetHealth(float newHealthAmount)
    {
        health = newHealthAmount;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        if (damageEffect != null)
            StartCoroutine(TriggerDamageEffect());

        // If the character is going to die
        if (health <= 0 && isDead == false)
        {
            FindNewTargetForOpponent();
            Die();
        }
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
