using UnityEngine;

public class GoldDeposit : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    [HideInInspector] public float startingMaxHealth;

    [Header("Mining Stats")]
    public float miningXOffset = 0.125f;
    public int hitsToProduceGold = 4;
    public int goldAmountEarnedEachProductionCycle = 20;

    [HideInInspector] public ResourceNode resourceNode;
    [HideInInspector] public bool occupied;

    bool canProduce;
    
    ResourceDisplay resourceDisplay;

    void Start()
    {
        currentHealth = maxHealth;
        startingMaxHealth = maxHealth;

        resourceNode = transform.GetComponentInParent<ResourceNode>();
        resourceDisplay = ResourceDisplay.instance;

        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    public void ProduceGold()
    {
        resourceDisplay.AddGold(goldAmountEarnedEachProductionCycle);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            canProduce = false;
        }
    }
}
