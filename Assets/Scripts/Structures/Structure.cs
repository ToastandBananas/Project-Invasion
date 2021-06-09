using UnityEngine;

public class Structure : MonoBehaviour
{
    [Header("Basic Info")]
    public StructureType structureType;
    [SerializeField] int goldCost = 50;
    [SerializeField] int suppliesCost = 50;
    public string description;
    public int maxStructureCount = 1;

    [Header("Obstacles")]
    public Obstacle[] obstacles;

    [HideInInspector] public int currentStructureCount;
    [HideInInspector] public bool canPlaceMore = false;
    [HideInInspector] public bool structurePlaced = false;

    [HideInInspector] public AttackerSpawner myLaneSpawner;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public SquadData squadData;

    [HideInInspector] public Color activeColor = new Color(1f, 1f, 1f, 1f); // White
    [HideInInspector] public Color invalidColor = new Color(1f, 0f, 0f, 0.4f); // Red and opaque
    [HideInInspector] public Color ghostImageColor = new Color(1f, 1f, 1f, 0.4f); // White and opaque

    [HideInInspector] public float destroyAnimationTime;

    DefenderSpawner defenderSpawner;

    public virtual void Start()
    {
        currentStructureCount = 1;

        audioManager = AudioManager.instance;
        defenderSpawner = DefenderSpawner.instance;
        squadData = GameManager.instance.squadData;
    }

    public int GetGoldCost()
    {
        return goldCost;
    }

    public int GetSuppliesCost()
    {
        return suppliesCost;
    }

    public virtual float GetMaxHealth()
    {
        if (obstacles.Length > 0)
            return obstacles[0].maxHealth;

        return 0f;
    }

    public virtual void OnStructureDestroyed()
    {
        currentStructureCount--;
        if (maxStructureCount > 0)
            canPlaceMore = true;

        if (currentStructureCount == 0)
        {
            defenderSpawner.RemoveCell(new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
            defenderSpawner.structures.Remove(this);
            Destroy(gameObject, destroyAnimationTime);
        }
    }

    public virtual void PlaceNewStructure(Vector2 coordinates)
    {
        structurePlaced = true;

        transform.position = coordinates;
        SetLaneSpawner();
    }
    
    public virtual void BuildNextStructure()
    {
        currentStructureCount++;
        if (currentStructureCount == maxStructureCount)
            canPlaceMore = false;
    }

    public virtual void SetGhostImageColor(Color color)
    {

    }

    public virtual void ShowNextStructureGhostImage(ResourceDisplay currencyDisplay)
    {

    }

    public virtual void HideNextStructureGhostImage()
    {
        
    }

    public bool IsAttackersNearby()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (obstacles[i].attackersNearby.Count > 0)
                return true;
        }

        return false;
    }

    public void SetLaneSpawner()
    {
        AttackerSpawner[] attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            bool isCloseEnough = (Mathf.Abs(spawner.transform.position.y - transform.position.y) <= Mathf.Epsilon);

            if (isCloseEnough) myLaneSpawner = spawner;
        }
    }
}
