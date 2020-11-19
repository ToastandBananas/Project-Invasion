using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] int health = 5;
    Text healthText;
    LevelLoader levelLoader;

    void Start()
    {
        healthText = GetComponent<Text>();
        levelLoader = FindObjectOfType<LevelLoader>();
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
            levelLoader.LoadLoseScreen(); // Load lose screen
    }
}
