using UnityEngine;

public class LevelController : MonoBehaviour
{
    int numberOfAttackers = 0;
    AttackerSpawner[] attackerSpawners;

    void Start()
    {
        attackerSpawners = FindObjectsOfType<AttackerSpawner>();
        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            numberOfAttackers += spawner.attackerPrefabs.Length;
        }
    }
    
    public void AttackerKilled()
    {
        numberOfAttackers--;
        if (numberOfAttackers == 0)
        {
            // End the level
            
        }
    }
}
