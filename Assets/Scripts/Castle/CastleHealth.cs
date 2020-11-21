using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] int health = 5;
    Text healthText;
    LevelController levelController;

    void Start()
    {
        healthText = GetComponent<Text>();
        levelController = FindObjectOfType<LevelController>();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        healthText.text = "   Castle HP: " + health.ToString();
    }

    public void TakeHealth(int damageAmount)
    {
        health -= damageAmount;
        UpdateDisplay();

        if (health <= 0)
            levelController.HandleLoseCondition(); // Load lose screen
    }
}
