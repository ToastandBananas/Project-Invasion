﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenderButton : MonoBehaviour
{
    // [SerializeField] Defender defenderPrefab;
    [SerializeField] Squad squadPrefab;
    
    DefenderSpawner defenderSpawner;

    void Start()
    {
        defenderSpawner = DefenderSpawner.instance;

        SetCost();
    }

    public void SelectSquadOfDefenders()
    {
        // Deselects the button so that the color goes back to its normal color
        EventSystem.current.SetSelectedGameObject(null);

        // Debug.Log("You clicked on " + transform.parent.name);
        // defenderSpawner.SetSelectedSquad(defenderPrefab);
        defenderSpawner.SetSelectedSquad(squadPrefab);
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