using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;

    public Attacker[] attackerPrefabsWave1;
    [Range(0, 250)] public int[] attackerCountsWave1;
    public Attacker[] attackerPrefabsWave2;
    [Range(0, 250)] public int[] attackerCountsWave2;
    public Attacker[] attackerPrefabsWave3;
    [Range(0, 250)] public int[] attackerCountsWave3;
    public Attacker[] attackerPrefabsWave4;
    [Range(0, 250)] public int[] attackerCountsWave4;
    public Attacker[] attackerPrefabsWave5;
    [Range(0, 250)] public int[] attackerCountsWave5;

    Vector3 randomSpawnOffset;
    bool spawn = true;
    int spawnIndex = 0;
    int waveNumber = 1;
    int wave1Count, wave2Count, wave3Count, wave4Count, wave5Count;

    IEnumerator Start()
    {
        GetWaveEnemyCounts();

        // Shuffle attacker array so attackers spawn in a random order
        ShuffleArray(attackerPrefabsWave1);

        while (spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnAttacker();
        }
    }

    void SpawnAttacker()
    {
        // If the enemy is large (such as a boss), spawn in the center of the lane, otherwise set a random y position
        if (attackerPrefabsWave1[spawnIndex].isLarge == false)
            randomSpawnOffset = new Vector3(0, Random.Range(-0.35f, 0.35f));
        else
            randomSpawnOffset = Vector3.zero;

        if (waveNumber == 1)
        {
            Attacker newAttacker = Instantiate(attackerPrefabsWave1[spawnIndex], transform.position + randomSpawnOffset, transform.rotation);
            newAttacker.transform.SetParent(transform);
            spawnIndex++;
        }

        if (spawnIndex == attackerPrefabsWave1.Length)
            StopSpawning();
    }

    public void StopSpawning()
    {
        spawn = false;
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

    void GetWaveEnemyCounts()
    {
        foreach (int attackerCount in attackerCountsWave1)
        {
            wave1Count += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave2)
        {
            wave2Count += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave3)
        {
            wave3Count += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave4)
        {
            wave4Count += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave5)
        {
            wave5Count += attackerCount;
        }
    }
}
