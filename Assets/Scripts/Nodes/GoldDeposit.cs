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

    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer sr;

    [HideInInspector] public Defender myLaborer;
    [HideInInspector] public ResourceNode resourceNode;

    [HideInInspector] public bool occupied;

    bool canProduce;
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

    public void ProduceGold()
    {
        resourceDisplay.AddGold(goldAmountEarnedEachProductionCycle);
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

        resourceNode.goldDeposits.Remove(this);
        if (resourceNode.goldDeposits.Count == 0)
        {
            DefenderSpawner.instance.goldNodes.Remove(resourceNode);
            DefenderSpawner.instance.RemoveNode(resourceNode.transform.position);

            Destroy(resourceNode.gameObject, 1f);
        }

        audioManager.PlayRandomSound(audioManager.rockSmashSounds);
        anim.SetBool("isDestroyed", true);

        Destroy(gameObject, 1f);
    }
}
