using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    Defender defender;
    GameObject defendersParent;
    const string DEFENDERS_PARENT_NAME = "Defenders";

    CurrencyDisplay currencyDisplay;
    List<Vector2> gridCellsOccupied;

    void Start()
    {
        defendersParent = GameObject.Find(DEFENDERS_PARENT_NAME);
        if (defendersParent == null)
            defendersParent = new GameObject(DEFENDERS_PARENT_NAME);

        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        gridCellsOccupied = new List<Vector2>();
    }

    void OnMouseDown()
    {
        SpawnDefender(GetSquareClicked());
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

    void SpawnDefender(Vector2 coordinates)
    {
        if (!defender)
        {
            return;
        }
        else if (!IsCellOccupied(coordinates))
        {
            if (currencyDisplay.HaveEnoughGold(defender.GetGoldCost()))
            {
                AddCell(coordinates);
                currencyDisplay.SpendGold(defender.GetGoldCost());
                Defender newDefender = Instantiate(defender, SnapToGrid(coordinates), Quaternion.identity);
                newDefender.transform.SetParent(defendersParent.transform);
            }
        }
    }

    public bool IsCellOccupied(Vector2 gridPosition)
    {
        if (gridCellsOccupied.Contains(gridPosition))
        {
            return true;
        }
        return false;
    }
    public void AddCell(Vector2 gridPosition)
    {
        gridCellsOccupied.Add(gridPosition);
    }

    public void RemoveCell(Vector2 gridPosition)
    {
        gridCellsOccupied.Remove(gridPosition);
    }
}
