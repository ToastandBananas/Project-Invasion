using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    [HideInInspector] public Squad squad;
    [HideInInspector] public Squad ghostImageSquad;
    [HideInInspector] public Structure structure;
    [HideInInspector] public Structure ghostImageStructure;

    [HideInInspector] public List<Vector2> gridCellsOccupied = new List<Vector2>();
    [HideInInspector] public List<Vector2> gridCellsWithNodes = new List<Vector2>();
    [HideInInspector] public List<ResourceNode> resourceNodes = new List<ResourceNode>();
    [HideInInspector] public List<Structure> structures = new List<Structure>();

    Structure focusedStructure;

    GameObject defendersParent;
    const string DEFENDERS_PARENT_NAME = "Defenders";
    bool canPlaceSquad = true;
    
    ResourceDisplay currencyDisplay;
    Vector2 mouseHoverTilePos;

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
        currencyDisplay = FindObjectOfType<ResourceDisplay>();

        defendersParent = GameObject.Find(DEFENDERS_PARENT_NAME);
        if (defendersParent == null)
            defendersParent = new GameObject(DEFENDERS_PARENT_NAME);

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (pauseMenu.gamePaused == false)
        {
            if (GameControls.gamePlayActions.deselect.WasPressed)
            {
                ClearSelectedSquad();
                ClearSelectedStructure();
            }

            // Keep ghost image of squad at mouse position and determine if we can place it at the position
            if (ghostImageSquad != null)
            {
                mouseHoverTilePos = GetSquareClicked();

                // If hovering over a resource node with a non-laborer squad (therefore an invalid position)
                if (IsCellOccupiedByResourceNode(mouseHoverTilePos) && ghostImageSquad.squadType != SquadType.Laborers)
                {
                    ShowLeader();
                    SetupInvalidPosition();
                }
                // If hovering over a resource node with a laborer squad (therefore a valid postion)
                else if (IsCellOccupiedByResourceNode(mouseHoverTilePos) && ghostImageSquad.squadType == SquadType.Laborers)
                {
                    // If there's a deposit available
                    if (GetResourceNodeFromCoordinates(mouseHoverTilePos).unoccupiedResourceDeposits.Count > 0)
                        SetupValidPosition();
                    else // If there's no deposits available
                        SetupInvalidPosition();
                }
                // If this is a laborer squad and the player is not hovering over a valid node
                else if (ghostImageSquad.squadType == SquadType.Laborers)
                {
                    SetupInvalidPosition();
                }
                // If hovering over a normal tile with a squad selected
                else if (IsCellOccupied(mouseHoverTilePos) == false && IsWithinBounds())
                {
                    ShowLeader();
                    SetupValidPosition();
                }
                // If hovering over a castle wall tile with a ranged squad selected
                else if (ghostImageSquad.isRangedUnit && IsCellOccupied(mouseHoverTilePos) == false && mouseHoverTilePos.x < 0.5f && mouseHoverTilePos.y <= 5.5f && mouseHoverTilePos.y >= 0.5f)
                {
                    HideLeader();
                    SetupValidPosition();
                }
                else // If hovering over an invalid position
                {
                    ShowLeader();
                    SetupInvalidPosition();
                }
            }
            else if (ghostImageStructure != null) // Keep ghost image of Structure at mouse position and determine if we can place it at the position
            {
                mouseHoverTilePos = GetSquareClicked();

                ghostImageStructure.gameObject.SetActive(true);

                if (focusedStructure != null && focusedStructure != GetStructureFromCoordinates(mouseHoverTilePos))
                    focusedStructure.HideNextStructureGhostImage();

                focusedStructure = null;

                // If hovering over a resource node
                if (IsCellOccupiedByResourceNode(mouseHoverTilePos))
                    SetupInvalidPosition();
                // If hovering over a normal tile
                else if (IsWithinBounds())
                {
                    // If cell is empty
                    if (IsCellOccupied(mouseHoverTilePos) == false)
                        SetupValidPosition();
                    else
                    {
                        focusedStructure = GetStructureFromCoordinates(mouseHoverTilePos);
                        // If there is already a Structure in this cell and it can still hold more Structures (such as with Wooden Stakes)
                        if (focusedStructure != null && focusedStructure.structureType == ghostImageStructure.structureType && focusedStructure.canPlaceMore && focusedStructure.IsAttackersNearby() == false)
                        {
                            focusedStructure.ShowNextStructureGhostImage(currencyDisplay);
                            ghostImageStructure.gameObject.SetActive(false);
                        }
                        else // If there is already a Structure in this cell and it can't hold any more Structures
                            SetupInvalidPosition();
                    }
                }
                else // If hovering over an invalid position
                    SetupInvalidPosition();
            }
        }

        if (GameControls.gamePlayActions.select.WasPressed && canPlaceSquad)
        {
            if (ghostImageSquad != null)
                SpawnSquad(GetSquareClicked());
            else if (ghostImageStructure != null)
                SpawnStructure(GetSquareClicked());
        }
    }

    void FixedUpdate()
    {
        if (pauseMenu.gamePaused == false)
            CheckIfCanAffordSquadOrStructure();
    }

    public void SetSelectedSquad(Squad squadToSelect)
    {
        if (structure != null)
            ClearSelectedStructure();

        squad = squadToSelect;

        // Instantiate the squad
        ghostImageSquad = Instantiate(squad, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void SetSelectedStructure(Structure structureToSelect)
    {
        if (squad != null)
            ClearSelectedSquad();

        structure = structureToSelect;

        // Instantiate the structure
        ghostImageStructure = Instantiate(structure, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
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

    public void ClearSelectedStructure()
    {
        structure = null;
        if (ghostImageStructure != null)
        {
            Destroy(ghostImageStructure.gameObject);
            ghostImageStructure = null;
        }

        if (focusedStructure != null)
            focusedStructure.HideNextStructureGhostImage();

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

    void SpawnSquad(Vector2 coordinates)
    {
        if (ghostImageSquad == null)
            return;

        if (currencyDisplay.HaveEnoughGold(squad.GetGoldCost()) && currencyDisplay.HaveEnoughSupplies(squad.GetSuppliesCost()) && (IsWithinBounds() || IsWithinCastleBounds()))
        {
            if (ghostImageSquad.squadType != SquadType.Laborers && IsCellOccupied(coordinates) == false && IsCellOccupiedByResourceNode(coordinates) == false)
            {
                StartCoroutine(PlaceSquad(coordinates));
            }
            else if (ghostImageSquad.squadType == SquadType.Laborers && IsCellOccupiedByResourceNode(coordinates) && GetResourceNodeFromCoordinates(coordinates).unoccupiedResourceDeposits.Count > 0)
            {
                ResourceNode node = GetResourceNodeFromCoordinates(coordinates);

                bool nodeCompletelyUnoccupied = true;
                foreach (ResourceDeposit goldDeposit in node.resourceDeposits)
                {
                    if (goldDeposit.occupied)
                    {
                        nodeCompletelyUnoccupied = false;
                        break;
                    }
                }

                if (nodeCompletelyUnoccupied)
                {
                    // If there's not any workers on the node yet
                    StartCoroutine(PlaceSquad(coordinates));
                }
                else
                {
                    // If there's already at least one worker on the node and there's still room for more
                    AddLaborerToSquad(node);
                }
            }
        }
    }

    IEnumerator PlaceSquad(Vector2 coordinates)
    {
        // If trying to place a ranged squad on the wall, swap out the current ghost squad for it's wall version
        if (mouseHoverTilePos.x < 1f && squad.castleWallVersionOfSquad != null)
        {
            Squad oldGhostImageSquad = ghostImageSquad;
            ghostImageSquad = Instantiate(squad.castleWallVersionOfSquad, coordinates, Quaternion.identity);
            Destroy(oldGhostImageSquad.gameObject);
            yield return null;
        }

        if (ghostImageSquad.leader != null)
        {
            ghostImageSquad.leader.sr.color = defaultColor;
            if (coordinates.x >= 1f)
                ghostImageSquad.leader.boxCollider.enabled = true;
        }

        for (int i = 0; i < ghostImageSquad.units.Count; i++)
        {
            ghostImageSquad.units[i].sr.color = defaultColor;
            if (coordinates.x >= 1f)
                ghostImageSquad.units[i].boxCollider.enabled = true;
        }

        AddCell(coordinates);
        ghostImageSquad.SetLaneSpawner();
        ghostImageSquad.squadPlaced = true;
        ghostImageSquad.transform.position = coordinates;
        ghostImageSquad.transform.SetParent(defendersParent.transform);

        currencyDisplay.SpendGold(ghostImageSquad.GetGoldCost());
        currencyDisplay.SpendSupplies(ghostImageSquad.GetSuppliesCost());

        if (coordinates.x >= 1f)
            ghostImageSquad.GetComponent<BoxCollider2D>().enabled = true;

        // Create a new ghost image squad in case the player wants to spawn another of the same squad
        ghostImageSquad = Instantiate(squad, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

        StartCoroutine(StartPlacementCooldown());
    }

    void SpawnStructure(Vector2 coordinates)
    {
        if (ghostImageStructure == null)
            return;

        if (currencyDisplay.HaveEnoughGold(structure.GetGoldCost()) && currencyDisplay.HaveEnoughSupplies(structure.GetSuppliesCost()) && IsWithinBounds())
        {
            if (IsCellOccupied(coordinates) == false && IsCellOccupiedByResourceNode(coordinates) == false)
            {
                PlaceStructure(coordinates);
            }
            else if (ghostImageStructure.maxStructureCount > 1) // If you can place multiple structures in one square (such as with Wooden Stakes), place another
            {
                if (focusedStructure.canPlaceMore && focusedStructure.structureType == ghostImageStructure.structureType && focusedStructure.IsAttackersNearby() == false)
                {
                    focusedStructure.BuildNextStructure();

                    currencyDisplay.SpendGold(focusedStructure.GetGoldCost());
                    currencyDisplay.SpendSupplies(focusedStructure.GetSuppliesCost());
                }
            }
        }
    }
    
    void PlaceStructure(Vector2 coordinates)
    {
        AddCell(coordinates);
        structures.Add(ghostImageStructure);

        ghostImageStructure.transform.SetParent(defendersParent.transform);

        currencyDisplay.SpendGold(ghostImageStructure.GetGoldCost());
        currencyDisplay.SpendSupplies(ghostImageStructure.GetSuppliesCost());

        ghostImageStructure.PlaceNewStructure(coordinates);

        // Create a new ghost image squad in case the player wants to spawn another of the same squad
        ghostImageStructure = Instantiate(structure, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

        StartCoroutine(StartPlacementCooldown());
    }

    void AddLaborerToSquad(ResourceNode node)
    {
        Defender newLaborer = Instantiate(ghostImageSquad.units[0], node.transform.position, Quaternion.identity, node.laborerSquadCurrentlyOnNode.unitsParent);
        newLaborer.sr.color = new Color(1f, 1f, 1f, 1f);
        newLaborer.boxCollider.enabled = true;
        newLaborer.squad = node.laborerSquadCurrentlyOnNode;
        newLaborer.squad.units.Add(newLaborer);
        newLaborer.squad.maxUnitCount++;

        currencyDisplay.SpendGold(ghostImageSquad.GetGoldCost());
        currencyDisplay.SpendSupplies(ghostImageSquad.GetSuppliesCost());

        StartCoroutine(StartPlacementCooldown());
    }

    void CheckIfCanAffordSquadOrStructure()
    {
        if (ghostImageSquad != null)
        {
            if (currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost()) && currencyDisplay.HaveEnoughSupplies(ghostImageSquad.GetSuppliesCost())
                && ((ghostImageSquad.leader != null && ghostImageSquad.leader.sr.color.Equals(invalidColor)) 
                || (ghostImageSquad.units.Count > 0 && ghostImageSquad.units[0].sr.color.Equals(invalidColor))))
            {
                SetGhostImageColor_Squads(ghostImageColor);
            }
            else if ((currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost()) == false || currencyDisplay.HaveEnoughSupplies(ghostImageSquad.GetSuppliesCost()) == false)
                && ((ghostImageSquad.leader != null && ghostImageSquad.leader.sr.color.Equals(ghostImageColor))
                || (ghostImageSquad.units.Count > 0 && ghostImageSquad.units[0].sr.color.Equals(ghostImageColor))))
            {
                SetGhostImageColor_Squads(invalidColor);
            }
        }
        else if (ghostImageStructure != null)
        {
            if (currencyDisplay.HaveEnoughGold(ghostImageStructure.GetGoldCost()) && currencyDisplay.HaveEnoughSupplies(ghostImageStructure.GetSuppliesCost()))
                ghostImageStructure.SetGhostImageColor(ghostImageColor);
            else
                ghostImageStructure.SetGhostImageColor(invalidColor);
        }
    }

    void SetGhostImageColor_Squads(Color color)
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

    void ShowLeader()
    {
        if (ghostImageSquad.leader != null)
            ghostImageSquad.leader.gameObject.SetActive(true);
    }

    void HideLeader()
    {
        // For castle wall squads
        if (ghostImageSquad.leader != null)
            ghostImageSquad.leader.gameObject.SetActive(false);
    }

    void SetupValidPosition()
    {
        if (ghostImageSquad != null)
        {
            ghostImageSquad.transform.position = mouseHoverTilePos;

            if (currencyDisplay.HaveEnoughGold(ghostImageSquad.GetGoldCost()) && currencyDisplay.HaveEnoughSupplies(ghostImageSquad.GetSuppliesCost()))
                SetGhostImageColor_Squads(ghostImageColor);
        }
        else if (ghostImageStructure != null)
        {
            ghostImageStructure.transform.position = mouseHoverTilePos;

            if (currencyDisplay.HaveEnoughGold(ghostImageStructure.GetGoldCost()) && currencyDisplay.HaveEnoughSupplies(ghostImageStructure.GetSuppliesCost()))
                ghostImageStructure.SetGhostImageColor(ghostImageColor);
        }
    }

    void SetupInvalidPosition()
    {
        Vector2 hoverPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        hoverPos = Camera.main.ScreenToWorldPoint(hoverPos);

        if (ghostImageSquad != null)
        {
            ghostImageSquad.transform.position = hoverPos;
            SetGhostImageColor_Squads(invalidColor);
        }
        else if (ghostImageStructure != null)
        {
            ghostImageStructure.transform.position = hoverPos;
            ghostImageStructure.SetGhostImageColor(invalidColor);
        }
    }

    bool IsWithinBounds()
    {
        if (mouseHoverTilePos.x >= 0.5f && mouseHoverTilePos.x <= 7.5f && mouseHoverTilePos.y >= 0.5f && mouseHoverTilePos.y <= 5.5f)
            return true;

        return false;
    }
    
    bool IsWithinCastleBounds()
    {
        if (mouseHoverTilePos.x < 1f && mouseHoverTilePos.y >= 0.5f && mouseHoverTilePos.y <= 5.5f)
            return true;

        return false;
    }

    public bool IsCellOccupied(Vector2 gridPosition)
    {
        if (gridCellsOccupied.Contains(gridPosition))
            return true;

        return false;
    }

    public bool IsCellOccupiedByResourceNode(Vector2 gridPosition)
    {
        if (gridCellsWithNodes.Contains(gridPosition))
            return true;

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

    public void AddNode(Vector2 gridPosition)
    {
        gridCellsWithNodes.Add(new Vector2(Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y)));
    }

    public void RemoveNode(Vector2 gridPosition)
    {
        gridCellsWithNodes.Remove(new Vector2(Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y)));
    }

    public ResourceNode GetResourceNodeFromCoordinates(Vector3 coordinates)
    {
        for (int i = 0; i < resourceNodes.Count; i++)
        {
            if (new Vector3(Mathf.RoundToInt(resourceNodes[i].transform.position.x), Mathf.RoundToInt(resourceNodes[i].transform.position.y)) == coordinates)
                return resourceNodes[i];
        }

        return null;
    }

    public Structure GetStructureFromCoordinates(Vector3 coordinates)
    {
        for (int i = 0; i < structures.Count; i++)
        {
            if (new Vector3(Mathf.RoundToInt(structures[i].transform.position.x), Mathf.RoundToInt(structures[i].transform.position.y)) == coordinates)
                return structures[i];
        }

        return null;
    }
}
