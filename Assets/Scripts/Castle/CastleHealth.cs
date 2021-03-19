using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] float baseHealth = 100f;
    [SerializeField] float health;

    float maxHealth;
    Text healthText;
    RectTransform healthBarMask;
    float maskStartingWidth;

    LevelController levelController;

    #region Singleton
    public static CastleHealth instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        health = baseHealth * PlayerPrefsController.GetDifficultyMultiplier();
        maxHealth = health;

        healthText = GetComponentInChildren<Text>();
        healthBarMask = transform.Find("Health Bar Mask").GetComponent<RectTransform>();
        maskStartingWidth = healthBarMask.sizeDelta.x;
        levelController = LevelController.instance;

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        healthText.text = Mathf.RoundToInt(health).ToString() + " / " + maxHealth.ToString();
        if (health > 0)
            healthBarMask.sizeDelta = new Vector2(maskStartingWidth * (health / maxHealth), healthBarMask.sizeDelta.y);
        else
            healthBarMask.sizeDelta = new Vector2(0f, healthBarMask.sizeDelta.y);
    }

    public void TakeHealth(float damageAmount)
    {
        health -= damageAmount;
        UpdateDisplay();

        if (health <= 0)
            levelController.HandleLoseCondition(); // Load lose screen
    }

    public float GetHealth()
    {
        return health;
    }
}
