using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int goldCost = 100;

    CurrencyDisplay currencyDisplay;

    void Start()
    {
        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
    }

    public void AddGold(int amount)
    {
        currencyDisplay.AddGold(amount);
    }

    public int GetGoldCost()
    {
        return goldCost;
    }
}
