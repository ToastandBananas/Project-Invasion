using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] float baseHealth = 3;
    [SerializeField] float health;
    Text healthText;
    LevelController levelController;

    void Start()
    {
        health = baseHealth - PlayerPrefsController.GetDifficulty();

        healthText = GetComponent<Text>();
        levelController = FindObjectOfType<LevelController>();
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
}
