using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] float baseHealth = 200f;
    [SerializeField] float currentHealth;
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
        currentHealth = baseHealth * PlayerPrefsController.GetDifficultyMultiplier_CastleHealth();
        maxHealth = currentHealth;

        healthText = GetComponentInChildren<Text>();
        healthBar = transform.Find("Health Bar").GetComponent<RectTransform>();
        maskStartingWidth = healthBar.sizeDelta.x;
        levelController = LevelController.instance;

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        healthText.text = Mathf.RoundToInt(currentHealth).ToString() + " / " + maxHealth.ToString();
        if (currentHealth > 0)
            healthBar.sizeDelta = new Vector2(maskStartingWidth * (currentHealth / maxHealth), healthBar.sizeDelta.y);
        else
            healthBar.sizeDelta = new Vector2(0f, healthBar.sizeDelta.y);
    }

    public void TakeHealth(float damageAmount)
    {
        // Adjust the damage amount based off of current difficulty settings
        float finalDamageAmount = damageAmount;
        finalDamageAmount *= PlayerPrefsController.GetDifficultyMultiplier_EnemyAttackDamage();

        if (finalDamageAmount < 1f)
            finalDamageAmount = 1f;
        else
            finalDamageAmount = Mathf.RoundToInt(finalDamageAmount);

        currentHealth -= finalDamageAmount;

        UpdateDisplay();

        if (currentHealth <= 0)
            levelController.HandleLoseCondition(); // Load lose screen
    }

    public void OnDifficultyChanged()
    {
        float currentHealthPercent = currentHealth / maxHealth;
        maxHealth = baseHealth * PlayerPrefsController.GetDifficultyMultiplier_CastleHealth();
        currentHealth = maxHealth * currentHealthPercent;
        UpdateDisplay();
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
