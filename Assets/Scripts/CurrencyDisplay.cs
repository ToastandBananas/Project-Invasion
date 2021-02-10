using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] int gold = 100;
    Text currencyText;

    #region Singleton
    public static CurrencyDisplay instance;
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
        currencyText = GetComponent<Text>();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        currencyText.text = gold.ToString();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateDisplay();
    }

    public void SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateDisplay();
        }
    }

    public bool HaveEnoughGold(int goldAmount)
    {
        return gold >= goldAmount;
    }
}
