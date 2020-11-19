using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;

    [SerializeField] Attacker[] attackerPrefabs;

    bool spawn = true;


    IEnumerator Start()
    {
        while (spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnAttacker();
        }
    }

    void SpawnAttacker()
    {
        Attacker newAttacker = Instantiate(attackerPrefabs[Random.Range(0, attackerPrefabs.Length)], transform.position, transform.rotation);
        newAttacker.transform.SetParent(transform);
    }
}
