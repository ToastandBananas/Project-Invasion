using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : MonoBehaviour
{
    public List<Attacker> attackersInRange = new List<Attacker>();

    Squad squad;

    void Start()
    {
        squad = transform.parent.GetComponent<Squad>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker attacker))
        {
            attackersInRange.Add(attacker);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker attacker))
        {
            if (attackersInRange.Contains(attacker))
                attackersInRange.Remove(attacker);
        }
    }
}
