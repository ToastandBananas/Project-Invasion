using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Structure parentStructure;

    [Header("Stats")]
    public float maxHealth = 100f;
    float currentHealth;

    [HideInInspector] public List<Attacker> attackersNearby = new List<Attacker>();

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
            DestroyObstacle();
    }

    float GetDamageAmount(float damage, float resistance)
    {
        return damage - (damage * resistance);
    }

    void DestroyObstacle()
    {
        parentStructure.OnStructureDestroyed();

        for (int i = 0; i < attackersNearby.Count; i++)
        {
            if (attackersNearby[i].currentTargetObstacle == this)
            {
                attackersNearby[i].StopAttacking();
                attackersNearby[i].currentTargetObstacle = null;
            }
        }

        currentHealth = maxHealth;
        StartCoroutine(WaitToSetInactive(parentStructure.destroyAnimationTime));
    }

    IEnumerator WaitToSetInactive(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker attacker))
        {
            attackersNearby.Add(attacker);
            attacker.currentTargetObstacle = this;
            attacker.Attack();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker attacker))
        {
            attackersNearby.Remove(attacker);
            attacker.currentTargetObstacle = null;
            attacker.StopAttacking();
        }
    }
}
