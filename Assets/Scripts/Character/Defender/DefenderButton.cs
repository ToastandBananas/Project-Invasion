using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Squad squadPrefab;
    [SerializeField] Structure structurePrefab;

    AudioManager audioManager;
    DefenderSpawner defenderSpawner;
    GameManager gm;
    Tooltip tooltip;

    void Start()
    {
        audioManager = AudioManager.instance;
        defenderSpawner = DefenderSpawner.instance;
        gm = GameManager.instance;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

        if ((squadPrefab != null && gm.squadData.SquadUnlocked(squadPrefab.squadType) == false)
            || (structurePrefab != null && gm.squadData.StructureUnlocked(structurePrefab.structureType) == false))
        {
            transform.parent.gameObject.SetActive(false);
        }

        SetCostDisplay();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ToggleDefenderButtonTooltip(squadPrefab, structurePrefab, transform.position);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.ToggleDefenderButtonTooltip(null, null, transform.position);
    }

    public void SelectSquadOfDefenders()
    {
        // Deselects the button so that the color goes back to its normal color
        EventSystem.current.SetSelectedGameObject(null);

        if (squadPrefab != null)
        {
            if (squadPrefab != defenderSpawner.squad && defenderSpawner.ghostImageSquad != null)
                Destroy(defenderSpawner.ghostImageSquad.gameObject);

            if (squadPrefab != defenderSpawner.squad)
                defenderSpawner.SetSelectedSquad(squadPrefab);
        }
        else if (structurePrefab != null)
        {
            if (structurePrefab != defenderSpawner.structure && defenderSpawner.ghostImageStructure != null)
                Destroy(defenderSpawner.ghostImageStructure.gameObject);

            if (structurePrefab != defenderSpawner.structure)
                defenderSpawner.SetSelectedStructure(structurePrefab);
        }

        defenderSpawner.gameObject.SetActive(true);

        audioManager.PlaySound(audioManager.buttonClickSounds, "WetClick", Vector3.zero);
    }

    void SetCostDisplay()
    {
        Text goldCostText = transform.parent.Find("Gold Cost Text").GetComponent<Text>();
        if (goldCostText == null)
            Debug.LogError(name + "squad button has no gold cost text.");
        else if (squadPrefab != null)
            goldCostText.text = squadPrefab.GetGoldCost().ToString();
        else if (structurePrefab != null)
            goldCostText.text = structurePrefab.GetGoldCost().ToString();

        Text suppliesCostText = transform.parent.Find("Supplies Cost Text").GetComponent<Text>();
        if (suppliesCostText == null)
            Debug.LogError(name + "squad button has no supplies cost text.");
        else if (squadPrefab != null)
            suppliesCostText.text = squadPrefab.GetSuppliesCost().ToString();
        else if (structurePrefab != null)
            suppliesCostText.text = structurePrefab.GetSuppliesCost().ToString();
    }
}
