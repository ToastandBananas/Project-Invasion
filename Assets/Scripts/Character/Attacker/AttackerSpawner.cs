using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;

    public Attacker[] attackerPrefabs;

    bool spawn = true;
    int spawnIndex = 0;

    IEnumerator Start()
    {
        // Shuffle attacker array so attackers spawn in a random order
        ShuffleArray(attackerPrefabs);

        while (spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnAttacker();
        }
    }

    public void StopSpawning()
    {
        spawn = false;
    }

    void SpawnAttacker()
    {
        Attacker newAttacker = Instantiate(attackerPrefabs[spawnIndex], transform.position, transform.rotation);
        newAttacker.transform.SetParent(transform);
        spawnIndex++;

        if (spawnIndex == attackerPrefabs.Length)
            StopSpawning();
    }

    void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int random = Random.Range(0, i + 1);
            T temp = arr[i];
            arr[i] = arr[random];
            arr[random] = temp;
        }
    }
}
