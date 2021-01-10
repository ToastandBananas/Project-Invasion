using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    float minSpawnDelay = 1f;
    float maxSpawnDelay = 5f;

    [Header("Wave 1")]
    [SerializeField] float minSpawnDelayWave1 = 1f;
    [SerializeField] float maxSpawnDelayWave1 = 5f;
    public List<Attacker> attackerPrefabsWave1 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave1 = new List<int>();

    [Header("Wave 2")]
    [SerializeField] float minSpawnDelayWave2 = 1f;
    [SerializeField] float maxSpawnDelayWave2 = 5f;
    public List<Attacker> attackerPrefabsWave2 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave2 = new List<int>();

    [Header("Wave 3")]
    [SerializeField] float minSpawnDelayWave3 = 1f;
    [SerializeField] float maxSpawnDelayWave3 = 5f;
    public List<Attacker> attackerPrefabsWave3 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave3 = new List<int>();

    [Header("Wave 4")]
    [SerializeField] float minSpawnDelayWave4 = 1f;
    [SerializeField] float maxSpawnDelayWave4 = 5f;
    public List<Attacker> attackerPrefabsWave4 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave4 = new List<int>();

    [Header("Wave 5")]
    [SerializeField] float minSpawnDelayWave5 = 1f;
    [SerializeField] float maxSpawnDelayWave5 = 5f;
    public List<Attacker> attackerPrefabsWave5 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave5 = new List<int>();
    
    public int totalAttackerCount = 0;
    public bool nextWaveDelayed = false;

    Vector3 randomSpawnOffset;
    bool spawn = true;

    LevelController levelController;

    void Awake()
    {
        GetWaveEnemyCounts();
    }

    IEnumerator Start()
    {
        levelController = FindObjectOfType<LevelController>();

        SetSpawnDelays();

        while (spawn)
        {
            if (nextWaveDelayed)
            {
                yield return new WaitForSeconds(levelController.waveDelay);
                nextWaveDelayed = false;
                SetSpawnDelays();
            }

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            if (levelController.waveNumber == 1)
                SpawnAttacker(attackerPrefabsWave1, attackerCountsWave1);
            else if (levelController.waveNumber == 2)
                SpawnAttacker(attackerPrefabsWave2, attackerCountsWave2);
            else if (levelController.waveNumber == 3)
                SpawnAttacker(attackerPrefabsWave3, attackerCountsWave3);
            else if (levelController.waveNumber == 4)
                SpawnAttacker(attackerPrefabsWave4, attackerCountsWave4);
            else if (levelController.waveNumber == 5)
                SpawnAttacker(attackerPrefabsWave5, attackerCountsWave5);
        }
    }

    void SpawnAttacker(List<Attacker> attackerPrefabsList, List<int> attackerCountsList)
    {
        if (attackerPrefabsList.Count > 0)
        {
            int randomIndex = Random.Range(0, attackerPrefabsList.Count);

            // If the enemy is large (such as a boss), spawn in the center of the lane, otherwise set a random y position
            if (attackerPrefabsList[randomIndex].isLarge == false)
                randomSpawnOffset = new Vector3(0, Random.Range(-0.35f, 0.35f));
            else
                randomSpawnOffset = Vector3.zero;

            Attacker newAttacker = Instantiate(attackerPrefabsList[randomIndex], transform.position + randomSpawnOffset, transform.rotation);
            newAttacker.transform.SetParent(transform);
            attackerCountsList[randomIndex]--;

            if (attackerCountsList[randomIndex] <= 0)
            {
                attackerPrefabsList.Remove(attackerPrefabsList[randomIndex]);
                attackerCountsList.Remove(attackerCountsList[randomIndex]);
            }

            if (attackerPrefabsList.Count == 0)
                levelController.CheckIfWaveComplete();
        }
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
            totalAttackerCount += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave2)
        {
            totalAttackerCount += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave3)
        {
            totalAttackerCount += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave4)
        {
            totalAttackerCount += attackerCount;
        }

        foreach (int attackerCount in attackerCountsWave5)
        {
            totalAttackerCount += attackerCount;
        }
    }

    void SetSpawnDelays()
    {
        if (levelController.waveNumber == 1)
        {
            minSpawnDelay = minSpawnDelayWave1;
            maxSpawnDelay = maxSpawnDelayWave1;
        }
        else if (levelController.waveNumber == 2)
        {
            minSpawnDelay = minSpawnDelayWave2;
            maxSpawnDelay = maxSpawnDelayWave2;
        }
        else if (levelController.waveNumber == 3)
        {
            minSpawnDelay = minSpawnDelayWave3;
            maxSpawnDelay = maxSpawnDelayWave3;
        }
        else if (levelController.waveNumber == 4)
        {
            minSpawnDelay = minSpawnDelayWave4;
            maxSpawnDelay = maxSpawnDelayWave4;
        }
        else if (levelController.waveNumber == 5)
        {
            minSpawnDelay = minSpawnDelayWave5;
            maxSpawnDelay = maxSpawnDelayWave5;
        }
    }
}
