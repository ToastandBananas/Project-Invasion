using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] float baseHealth = 100f;
    [SerializeField] float health;

    float maxHealth;
    Text healthText;
    RectTransform healthBar;
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
        healthBar = transform.Find("Health Bar").GetComponent<RectTransform>();
        maskStartingWidth = healthBar.sizeDelta.x;
        levelController = LevelController.instance;

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        healthText.text = Mathf.RoundToInt(health).ToString() + " / " + maxHealth.ToString();
        if (health > 0)
            healthBar.sizeDelta = new Vector2(maskStartingWidth * (health / maxHealth), healthBar.sizeDelta.y);
        else
            healthBar.sizeDelta = new Vector2(0f, healthBar.sizeDelta.y);
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
