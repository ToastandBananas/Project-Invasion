using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenderButton : MonoBehaviour
{
    [SerializeField] Defender defenderPrefab;
    
    DefenderSpawner defenderSpawner;

    void Start()
    {
        defenderSpawner = FindObjectOfType<DefenderSpawner>();

        SetCost();
    }

    public void SelectDefender()
    {
        // Deselects the button so that the color goes back to its normal color
        EventSystem.current.SetSelectedGameObject(null);

        // Debug.Log("You clicked on " + transform.parent.name);
        defenderSpawner.SetSelectedDefender(defenderPrefab);
    }

    void SetCost()
    {
        Text costText = transform.parent.GetComponentInChildren<Text>();
        if (costText == null)
            Debug.LogError(name + " has no cost text.");
        else
            costText.text = " " + defenderPrefab.GetGoldCost().ToString();
    }
}
