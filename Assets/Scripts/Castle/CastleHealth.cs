using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] float baseHealth = 100f;
    [SerializeField] float health;
    Text healthText;
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
        health = baseHealth * PlayerPrefsController.GetDifficulty();

        healthText = GetComponent<Text>();
        levelController = LevelController.instance;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        healthText.text = "   Castle HP: " + health.ToString();
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
