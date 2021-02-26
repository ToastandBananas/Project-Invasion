﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Squad squadPrefab;
    
    DefenderSpawner defenderSpawner;
    Tooltip tooltip;

    void Start()
    {
        defenderSpawner = DefenderSpawner.instance;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

        SetCost();
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
    }

    void SetCost()
    {
        Text costText = transform.parent.GetComponentInChildren<Text>();
        if (costText == null)
            Debug.LogError(name + " has no cost text.");
        else
            costText.text = " " + squadPrefab.GetGoldCost().ToString();
    }
}
