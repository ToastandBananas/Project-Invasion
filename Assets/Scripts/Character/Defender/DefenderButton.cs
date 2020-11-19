using UnityEngine;
using UnityEngine.EventSystems;

public class DefenderButton : MonoBehaviour
{
    [SerializeField] Defender defenderPrefab;
    
    DefenderSpawner defenderSpawner;

    void Start()
    {
        defenderSpawner = FindObjectOfType<DefenderSpawner>();
    }

    public void SelectDefender()
    {
        // Deselects the button so that the color goes back to its normal color
        EventSystem.current.SetSelectedGameObject(null);

        // Debug.Log("You clicked on " + transform.parent.name);
        defenderSpawner.SetSelectedDefender(defenderPrefab);
    }
}
