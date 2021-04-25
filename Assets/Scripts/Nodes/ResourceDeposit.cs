using UnityEngine;

public class ResourceDeposit : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    [HideInInspector] public float startingMaxHealth;

    [Header("Resource Stats")]
    public ResourceType resourceType;
    public int hitsToProduce = 4;
    public int goldEarnedEachProductionCycle;
    public int suppliesEarnedEachProductionCycle;

    [Header("Laborer Position Offset")]
    public float miningXOffset = 0.125f;

    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer sr;

    [HideInInspector] public Defender myLaborer;
    [HideInInspector] public ResourceNode resourceNode;

    [HideInInspector] public bool occupied;

    bool canProduce = true;
    bool isDestroyed;

    AudioManager audioManager;
    ResourceDisplay resourceDisplay;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        startingMaxHealth = maxHealth;

        resourceNode = transform.GetComponentInParent<ResourceNode>();
        resourceDisplay = ResourceDisplay.instance;
        audioManager = AudioManager.instance;

        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    public void ProduceResources()
    {
        if (canProduce)
        {
            if (resourceType == ResourceType.Gold)
                resourceDisplay.AddGold(goldEarnedEachProductionCycle);
            else if (resourceType == ResourceType.Wood)
                resourceDisplay.AddSupplies(suppliesEarnedEachProductionCycle);
        }
    }

    public void StopProducing()
    {
        canProduce = false;
    }

    public void StartProducing()
    {
        canProduce = true;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 && isDestroyed == false)
            DestroyDeposit();
    }

    void DestroyDeposit()
    {
        canProduce = false;
        isDestroyed = true;

        resourceNode.resourceDeposits.Remove(this);
        if (resourceNode.resourceDeposits.Count == 0)
        {
            DefenderSpawner.instance.resourceNodes.Remove(resourceNode);
            DefenderSpawner.instance.RemoveNode(resourceNode.transform.position);

            Destroy(resourceNode.gameObject, 1f);
        }

        audioManager.PlayRandomSound(audioManager.rockSmashSounds);
        anim.SetBool("isDestroyed", true);

        Destroy(gameObject, 1f);
    }
}
