using UnityEngine;

public class GridCell : MonoBehaviour
{
    Squad occupyingSquad;

    public void SetOccupyingSquad(Squad squad)
    {
        occupyingSquad = squad;
    }
}
