using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    Defender defender;
    CurrencyDisplay currencyDisplay;

    void Start()
    {
        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
    }

    void OnMouseDown()
    {
        AttemptToPlaceDefenderAt(GetSquareClicked());
    }

    public void SetSelectedDefender (Defender defenderToSelect)
    {
        defender = defenderToSelect;
    }

    void AttemptToPlaceDefenderAt(Vector2 gridPos)
    {
        int defenderGoldCost = defender.GetGoldCost();

        // If we have enough currency, spawn defender and spend currency
        if (currencyDisplay.HaveEnoughGold(defenderGoldCost))
        {
            SpawnDefender(gridPos);
            currencyDisplay.SpendGold(defenderGoldCost);
        }
    }

    Vector2 GetSquareClicked()
    {
        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);
        Vector2 gridPos = SnapToGrid(worldPos);
        return gridPos;
    }

    Vector2 SnapToGrid(Vector2 rawWorldPos)
    {
        float newX = Mathf.RoundToInt(rawWorldPos.x);
        float newY = Mathf.RoundToInt(rawWorldPos.y);
        return new Vector2(newX, newY);
    }

    void SpawnDefender(Vector2 gridPos)
    {
        Defender newDefender = Instantiate(defender, gridPos, Quaternion.identity);
    }
}
