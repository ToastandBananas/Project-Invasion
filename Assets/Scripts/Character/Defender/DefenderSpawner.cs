using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    [HideInInspector] public Squad squad;
    [HideInInspector] public Squad ghostImageSquad;

    GameObject defendersParent;
    const string DEFENDERS_PARENT_NAME = "Defenders";
    bool canPlaceSquad = true;

    Defender defender;
    CurrencyDisplay currencyDisplay;
    List<Vector2> gridCellsOccupied;
    Vector2 mouseHoverTilePos;
    int defaultSortingOrder = 5;
    int castleWallSortingOrder = 15;

    Color defaultColor = new Color(1f, 1f, 1f, 1f); // White
    Color invalidColor = new Color(1f, 0f, 0f, 0.4f); // Red and opaque
    Color ghostImageColor = new Color(1f, 1f, 1f, 0.4f); // White and opaque

    PauseMenu pauseMenu;

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
        pauseMenu = PauseMenu.instance;
        defendersParent = GameObject.Find(DEFENDERS_PARENT_NAME);
        if (defendersParent == null)
            defendersParent = new GameObject(DEFENDERS_PARENT_NAME);

        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        gridCellsOccupied = new List<Vector2>();

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (pauseMenu.gamePaused == false)
        {
            if (Input.GetMouseButtonDown(1))
                ClearSelectedSquad();

            // Keep ghost image of squad at mouse position
            if (ghostImageSquad != null)
            {
                mouseHoverTilePos = GetSquareClicked();

                if (mouseHoverTilePos.x < 0.5f)
                    ghostImageSquad.SetSortingOrder(castleWallSortingOrder);
                else
                    ghostImageSquad.SetSortingOrder(defaultSortingOrder);

                if (IsCellOccupied(mouseHoverTilePos) == false && mouseHoverTilePos.x >= 0.5f && mouseHoverTilePos.x <= 7.5f && mouseHoverTilePos.y >= 0.5f && mouseHoverTilePos.y <= 5.5f)
                {
                    if (ghostImageSquad.leader != null)
                        ghostImageSquad.leader.gameObject.SetActive(true);

                    ghostImageSquad.transform.position = mouseHoverTilePos;
                    //ghostImageSquad.SetLaneSpawner();
                    if (currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost()))
                        SetGhostImageColor(ghostImageColor);
                }
                else if (ghostImageSquad.isRangedUnit && IsCellOccupied(mouseHoverTilePos) == false && mouseHoverTilePos.x < 0.5f)
                {
                    if (ghostImageSquad.leader != null)
                        ghostImageSquad.leader.gameObject.SetActive(false);

                    ghostImageSquad.transform.position = mouseHoverTilePos;

                    //ghostImageSquad.SetLaneSpawner();
                    if (currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost()))
                        SetGhostImageColor(ghostImageColor);
                }
                else
                {
                    if (ghostImageSquad.leader != null)
                        ghostImageSquad.leader.gameObject.SetActive(true);

                    Vector2 hoverPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    hoverPos = Camera.main.ScreenToWorldPoint(hoverPos);
                    ghostImageSquad.transform.position = hoverPos;
                    SetGhostImageColor(invalidColor);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (pauseMenu.gamePaused == false)
            CheckIfCanAffordSquad();
    }

    void OnMouseDown()
    {
        if (canPlaceSquad && pauseMenu.gamePaused == false)
            StartCoroutine(SpawnSquad(GetSquareClicked()));
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

        gameObject.SetActive(false);
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
        float newX = 0;
        float newY = 0;

        newY = Mathf.RoundToInt(rawWorldPos.y);

        if (rawWorldPos.x >= 0.5f)
            newX = Mathf.RoundToInt(rawWorldPos.x);
        else
        {
            if (newY == 3)
                newX = -0.45f;
            else
                newX = -0.4f;
        }

        return new Vector2(newX, newY);
    }

    IEnumerator SpawnSquad(Vector2 coordinates)
    {
        if (!ghostImageSquad)
        {
            yield break;
        }
        else if (!IsCellOccupied(coordinates))
        {
            if (currencyDisplay.HaveEnoughGold(squad.GetGoldCost()))
            {
                if (coordinates.x < 1f)
                {
                    Squad oldGhostImageSquad = ghostImageSquad;
                    ghostImageSquad = Instantiate(squad.castleWallVersionOfSquad, coordinates, Quaternion.identity);
                    Destroy(oldGhostImageSquad.gameObject);
                    yield return null;
                }

                if (ghostImageSquad.leader != null)
                {
                    ghostImageSquad.leader.sr.color = new Color(1f, 1f, 1f, 1f);
                    if (coordinates.x >= 1f)
                        ghostImageSquad.leader.GetComponent<BoxCollider2D>().enabled = true;
                }

                for (int i = 0; i < ghostImageSquad.units.Count; i++)
                {
                    ghostImageSquad.units[i].sr.color = new Color(1f, 1f, 1f, 1f);
                    if (coordinates.x >= 1f)
                        ghostImageSquad.units[i].GetComponent<BoxCollider2D>().enabled = true;
                }

                AddCell(coordinates);
                ghostImageSquad.SetLaneSpawner();
                ghostImageSquad.squadPlaced = true;
                ghostImageSquad.transform.position = coordinates;
                ghostImageSquad.transform.SetParent(defendersParent.transform);

                currencyDisplay.SpendGold(ghostImageSquad.GetGoldCost());
                if (coordinates.x >= 1f)
                    ghostImageSquad.GetComponent<BoxCollider2D>().enabled = true;

                // Create a new ghost image squad in case the player wants to spawn another of the same squad
                ghostImageSquad = Instantiate(squad, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

                StartCoroutine(StartPlacementCooldown());
            }
        }
    }

    void CheckIfCanAffordSquad()
    {
        if (ghostImageSquad != null)
        {
            if (currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost())
                && ((ghostImageSquad.leader != null && ghostImageSquad.leader.sr.color.Equals(invalidColor)) 
                || (ghostImageSquad.units.Count > 0 && ghostImageSquad.units[0].sr.color.Equals(invalidColor))))
            {
                SetGhostImageColor(ghostImageColor);
            }
            else if (currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost()) == false
                && ((ghostImageSquad.leader != null && ghostImageSquad.leader.sr.color.Equals(ghostImageColor)) 
                || (ghostImageSquad.units.Count > 0 && ghostImageSquad.units[0].sr.color.Equals(ghostImageColor))))
            {
                SetGhostImageColor(invalidColor);
            }
        }
    }

    void SetGhostImageColor(Color color)
    {
        if ((ghostImageSquad.leader != null && ghostImageSquad.leader.sr.color.Equals(color) == false)
            || (ghostImageSquad.units.Count > 0 && ghostImageSquad.units[0].sr.color.Equals(color) == false)) // If the color isn't already the color we're trying to set it to
        {
            if (ghostImageSquad.leader != null)
                ghostImageSquad.leader.sr.color = color;

            for (int i = 0; i < ghostImageSquad.units.Count; i++)
            {
                ghostImageSquad.units[i].sr.color = color;
            }
        }
    }

    IEnumerator StartPlacementCooldown()
    {
        canPlaceSquad = false;
        yield return new WaitForSeconds(0.1f);
        canPlaceSquad = true;
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
