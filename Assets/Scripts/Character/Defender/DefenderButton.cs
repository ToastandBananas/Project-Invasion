using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Squad squadPrefab;

    AudioManager audioManager;
    DefenderSpawner defenderSpawner;
    Tooltip tooltip;

    void Start()
    {
        audioManager = AudioManager.instance;
        defenderSpawner = DefenderSpawner.instance;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

        if (GameManager.instance.squadData.SquadUnlocked(squadPrefab.squadType) == false)
            transform.parent.gameObject.SetActive(false);

        SetCostDisplay();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ToggleSquadTooltip(squadPrefab, transform.position);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.ToggleSquadTooltip(null, transform.position);
    }

    public void SelectSquadOfDefenders()
    {
        // Deselects the button so that the color goes back to its normal color
        EventSystem.current.SetSelectedGameObject(null);

        // Debug.Log("You clicked on " + transform.parent.name);
        if (squadPrefab != defenderSpawner.squad && defenderSpawner.ghostImageSquad != null)
            Destroy(defenderSpawner.ghostImageSquad.gameObject);

        if (squadPrefab != defenderSpawner.squad)
            defenderSpawner.SetSelectedSquad(squadPrefab);

        defenderSpawner.gameObject.SetActive(true);

        audioManager.PlaySound(audioManager.buttonClickSounds, "WetClick", Vector3.zero);
    }

    void SetCostDisplay()
    {
        Text goldCostText = transform.parent.Find("Gold Cost Text").GetComponent<Text>();
        if (goldCostText == null)
            Debug.LogError(name + "squad button has no gold cost text.");
        else
            goldCostText.text = squadPrefab.GetGoldCost().ToString();

        Text suppliesCostText = transform.parent.Find("Supplies Cost Text").GetComponent<Text>();
        if (suppliesCostText == null)
            Debug.LogError(name + "squad button has no supplies cost text.");
        else
            suppliesCostText.text = squadPrefab.GetSuppliesCost().ToString();
    }
}
