using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] int gold = 100;
    [SerializeField] int supplies = 100;

    AudioManager audioManager;
    Text goldText, suppliesText;

    #region Singleton
    public static ResourceDisplay instance;
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
        audioManager = AudioManager.instance;

        goldText = transform.Find("Gold").GetComponentInChildren<Text>();
        suppliesText = transform.Find("Supplies").GetComponentInChildren<Text>();

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        goldText.text = gold.ToString();
        suppliesText.text = supplies.ToString();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateDisplay();
    }

    public void SpendGold(int amount)
    {
        if (HaveEnoughGold(amount))
        {
            gold -= amount;
            UpdateDisplay();
            audioManager.PlayRandomSound(audioManager.goldSounds);
        }
    }

    public bool HaveEnoughGold(int goldAmount)
    {
        return gold >= goldAmount;
    }

    public void AddSupplies(int amount)
    {
        supplies += amount;
        UpdateDisplay();
    }

    public void SpendSupplies(int amount)
    {
        if (HaveEnoughSupplies(amount))
        {
            supplies -= amount;
            UpdateDisplay();
        }
    }

    public bool HaveEnoughSupplies(int suppliesAmount)
    {
        return supplies >= suppliesAmount;
    }
}
