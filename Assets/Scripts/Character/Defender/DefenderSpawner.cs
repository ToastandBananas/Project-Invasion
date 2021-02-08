using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    Defender defender;
    [HideInInspector] public Squad squad;
    [HideInInspector] public Squad ghostImageSquad;
    GameObject defendersParent;
    const string DEFENDERS_PARENT_NAME = "Defenders";

    CurrencyDisplay currencyDisplay;
    List<Vector2> gridCellsOccupied;
    Vector2 mouseHoverTilePos;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ClearSelectedSquad();

        // Keep ghost image of squad at mouse position
        if (ghostImageSquad != null)
        {
            mouseHoverTilePos = GetSquareClicked();
            if (IsCellOccupied(mouseHoverTilePos) == false && mouseHoverTilePos.x >= 0.5f && mouseHoverTilePos.x <= 7.5f && mouseHoverTilePos.y >= 0.5f && mouseHoverTilePos.y <= 5.5f)
            {
                ghostImageSquad.transform.position = SnapToGrid(mouseHoverTilePos);
                ghostImageSquad.SetLaneSpawner();
            }
            else
            {
                Vector2 hoverPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                hoverPos = Camera.main.ScreenToWorldPoint(hoverPos);
                ghostImageSquad.transform.position = hoverPos;
            }
        }
    }

    void OnMouseDown()
    {
        SpawnSquad(GetSquareClicked());
    }

    public void SetSelectedSquad(Squad squadToSelect)
    {
        squad = squadToSelect;

        // Instantiate the squad
        ghostImageSquad = Instantiate(squad, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void ClearSelectedSquad()
    {
        squad = null;
        if (ghostImageSquad != null)
        {
            Destroy(ghostImageSquad.gameObject);
            ghostImageSquad = null;
        }
    }

    /*void AttemptToPlaceDefenderAt(Vector2 gridPos)
    {
        int squadGoldCost = squad.GetGoldCost();

        // If we have enough currency, spawn defender and spend currency
        if (currencyDisplay.HaveEnoughGold(squadGoldCost))
        {
            SpawnSquad(gridPos);
            currencyDisplay.SpendGold(squadGoldCost);
        }
    }*/

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
        if (!ghostImageSquad)
        {
            return;
        }
        else if (!IsCellOccupied(coordinates))
        {
            if (currencyDisplay.HaveEnoughGold(squad.GetGoldCost()))
            {
                ghostImageSquad.squadPlaced = true;
                ghostImageSquad.leader.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                ghostImageSquad.leader.GetComponent<BoxCollider2D>().enabled = true;
                for (int i = 0; i < ghostImageSquad.units.Count; i++)
                {
                    ghostImageSquad.units[i].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    ghostImageSquad.units[i].GetComponent<BoxCollider2D>().enabled = true;
                }

                AddCell(coordinates);

                currencyDisplay.SpendGold(ghostImageSquad.GetGoldCost());
                ghostImageSquad.transform.SetParent(defendersParent.transform);
                ghostImageSquad.GetComponent<BoxCollider2D>().enabled = true;

                // Create a new ghost image squad in case the player wants to spawn another of the same squad
                ghostImageSquad = Instantiate(squad, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            }
        }
        /*else if (!IsCellOccupied(coordinates))
        {
            if (currencyDisplay.HaveEnoughGold(squad.GetGoldCost()))
            {
                AddCell(coordinates);
                
                currencyDisplay.SpendGold(squad.GetGoldCost());
                Squad newSquad = Instantiate(squad, SnapToGrid(coordinates), Quaternion.identity);
                newSquad.transform.SetParent(defendersParent.transform);
            }
        }*/
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
