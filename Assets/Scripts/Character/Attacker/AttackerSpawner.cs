using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    float minSpawnDelay, maxSpawnDelay;
    float maxSpawnPoints;

    [Header("Wave 1")]
    [SerializeField] int maxSpawnPointsWave1 = 100;
    [SerializeField] float minSpawnDelayWave1 = 2f;
    [SerializeField] float maxSpawnDelayWave1 = 5f;
    public List<Attacker> attackerPrefabsWave1 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave1 = new List<int>();

    [Header("Wave 2")]
    [SerializeField] int maxSpawnPointsWave2 = 100;
    [SerializeField] float minSpawnDelayWave2 = 2f;
    [SerializeField] float maxSpawnDelayWave2 = 5f;
    public List<Attacker> attackerPrefabsWave2 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave2 = new List<int>();

    [Header("Wave 3")]
    [SerializeField] int maxSpawnPointsWave3 = 100;
    [SerializeField] float minSpawnDelayWave3 = 2f;
    [SerializeField] float maxSpawnDelayWave3 = 5f;
    public List<Attacker> attackerPrefabsWave3 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave3 = new List<int>();

    [Header("Wave 4")]
    [SerializeField] int maxSpawnPointsWave4 = 100;
    [SerializeField] float minSpawnDelayWave4 = 2f;
    [SerializeField] float maxSpawnDelayWave4 = 5f;
    public List<Attacker> attackerPrefabsWave4 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave4 = new List<int>();

    [Header("Wave 5")]
    [SerializeField] int maxSpawnPointsWave5 = 100;
    [SerializeField] float minSpawnDelayWave5 = 2f;
    [SerializeField] float maxSpawnDelayWave5 = 5f;
    public List<Attacker> attackerPrefabsWave5 = new List<Attacker>();
    [Range(0, 250)] public List<int> attackerCountsWave5 = new List<int>();
    
    [HideInInspector] public int totalAttackerCount = 0;
    [HideInInspector] public bool startNextWaveDelay = false;

    Vector3 randomSpawnOffset;
    bool spawn = true;

    LevelController levelController;

    void Awake()
    {
        GetWaveEnemyCounts();
    }

    IEnumerator Start()
    {
        levelController = LevelController.instance;

        SetSpawnDelays();

        while (spawn)
        {
            if (startNextWaveDelay)
            {
                yield return new WaitForSeconds(levelController.waveDelay);
                startNextWaveDelay = false;
                SetSpawnDelays();
            }
            
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            if (levelController.waveNumber == 1)
                StartCoroutine(SpawnAttackers(attackerPrefabsWave1, attackerCountsWave1));
            else if (levelController.waveNumber == 2)
                StartCoroutine(SpawnAttackers(attackerPrefabsWave2, attackerCountsWave2));
            else if (levelController.waveNumber == 3)
                StartCoroutine(SpawnAttackers(attackerPrefabsWave3, attackerCountsWave3));
            else if (levelController.waveNumber == 4)
                StartCoroutine(SpawnAttackers(attackerPrefabsWave4, attackerCountsWave4));
            else if (levelController.waveNumber == 5)
                StartCoroutine(SpawnAttackers(attackerPrefabsWave5, attackerCountsWave5));

            levelController.CheckIfWaveComplete();
        }
    }

    IEnumerator SpawnAttackers(List<Attacker> attackerPrefabsList, List<int> attackerCountsList)
    {
        if (attackerPrefabsList.Count > 0 && attackerCountsList.Count > 0)
        {
            SetMaxSpawnPoints();

            int spawnPoints = 0;
            for (int i = 0; i < maxSpawnPoints; i += spawnPoints)
            {
                // Choose a random enemy to spawn from our list of enemies for this wave
                int randomIndex = Random.Range(0, attackerPrefabsList.Count);

                if (randomIndex <= attackerCountsList.Count - 1 && attackerCountsList[randomIndex] > 0)
                {
                    // If the enemy is large (such as a boss), spawn in the center of the lane, otherwise set a random y position
                    if (attackerPrefabsList[randomIndex].isLarge == false)
                        randomSpawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.35f, 0.35f));
                    else
                        randomSpawnOffset = Vector3.zero;

                    // Spawn the new attacker
                    Attacker newAttacker = Instantiate(attackerPrefabsList[randomIndex], transform.position + randomSpawnOffset, transform.rotation);
                    newAttacker.transform.SetParent(transform);
                    newAttacker.myAttackerSpawner = this;
                    attackerCountsList[randomIndex]--;
                    spawnPoints = newAttacker.spawnPoints;

                    // If the maximum amount of this attacker has been spawned, remove it from our list of attackers for this wave
                    if (attackerCountsList[randomIndex] <= 0)
                    {
                        attackerPrefabsList.Remove(attackerPrefabsList[randomIndex]);
                        attackerCountsList.Remove(attackerCountsList[randomIndex]);
                    }
                }

                // Wait a tiny bit so that attackers don't spawn at exactly the same time
                yield return new WaitForSeconds(Random.Range(0.1f, 0.75f));
            }
        }
        else if (attackerPrefabsList.Count > 0 && attackerCountsList.Count == 0)
            Debug.LogError("Attacker Prefabs assigned for " + name + " in wave number " + levelController.waveNumber + ", but the Attacker Counts List has not been populated. Fix me!");
        else if (attackerPrefabsList.Count == 0 && attackerCountsList.Count > 0)
            Debug.LogError("Attacker Counts for " + name + " in wave number " + levelController.waveNumber + " are greater than 0, but there are no Attacker Prefabs assigned. Fix me!");
    }

    public void StartNextWaveDelay()
    {
        startNextWaveDelay = true;
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
    
    void SetMaxSpawnPoints()
    {
        if (levelController.waveNumber == 1)
            maxSpawnPoints = maxSpawnPointsWave1;
        else if (levelController.waveNumber == 2)
            maxSpawnPoints = maxSpawnPointsWave2;
        else if (levelController.waveNumber == 3)
            maxSpawnPoints = maxSpawnPointsWave3;
        else if (levelController.waveNumber == 4)
            maxSpawnPoints = maxSpawnPointsWave4;
        else if (levelController.waveNumber == 5)
            maxSpawnPoints = maxSpawnPointsWave5;
    }
}
