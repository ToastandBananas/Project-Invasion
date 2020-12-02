using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public List<Defender> defenders;

    void Awake()
    {
        defenders = new List<Defender>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent<Defender>(out Defender defender);
        if (defender != null)
            defenders.Add(defender);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent<Defender>(out Defender defender);
        if (defender != null)
            defenders.Remove(defender);
    }
}
