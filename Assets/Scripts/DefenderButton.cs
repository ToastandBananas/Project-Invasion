using UnityEngine;
using System.Collections.Generic;

public class DefenderButton : MonoBehaviour
{
    List<DefenderButton> buttons = new List<DefenderButton>();

    void Start()
    {
        var buttons = FindObjectsOfType<DefenderButton>();
    }

    public void SelectDefender()
    {
        foreach(DefenderButton button in buttons)
        {
            button.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
        }

        Debug.Log("You clicked on " + transform.parent.name);
    }
}
