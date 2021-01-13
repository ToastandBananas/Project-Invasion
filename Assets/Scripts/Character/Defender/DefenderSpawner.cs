using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    Defender defender;
    Squad squad;
    GameObject defendersParent;
    const string DEFENDERS_PARENT_NAME = "Defenders";

    CurrencyDisplay currencyDisplay;
    List<Vector2> gridCellsOccupied;

    #region Singleton
    public static DefenderSpawner instance;

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
        defendersParent = GameObject.Find(DEFENDERS_PARENT_NAME);
        if (defendersParent == null)
            defendersParent = new GameObject(DEFENDERS_PARENT_NAME);

        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        gridCellsOccupied = new List<Vector2>();
    }

    void OnMouseDown()
    {
        SpawnSquad(GetSquareClicked());
    }

    public void SetSelectedSquad (Squad squadToSelect)
    {
        // defender = defenderToSelect;
        squad = squadToSelect;
    }

    void AttemptToPlaceDefenderAt(Vector2 gridPos)
    {
        int squadGoldCost = squad.GetGoldCost();

        // If we have enough currency, spawn defender and spend currency
        if (currencyDisplay.HaveEnoughGold(squadGoldCost))
        {
            SpawnSquad(gridPos);
            currencyDisplay.SpendGold(squadGoldCost);
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

    void SpawnSquad(Vector2 coordinates)
    {
        if (!squad)
        {
            return;
        }
        else if (!IsCellOccupied(coordinates))
        {
            if (currencyDisplay.HaveEnoughGold(squad.GetGoldCost()))
            {
                AddCell(coordinates);
                
                currencyDisplay.SpendGold(squad.GetGoldCost());
                Squad newSquad = Instantiate(squad, SnapToGrid(coordinates), Quaternion.identity);
                newSquad.transform.SetParent(defendersParent.transform);
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
