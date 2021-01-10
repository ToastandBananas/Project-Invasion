﻿using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject winCanvas, loseCanvas;

    [Header("Wave Info")]
    public int waveNumber = 1;
    [SerializeField] int maxWaves = 5;
    public float waveDelay = 10f;

    int numberOfAttackers = 0;
    AudioSource audioSource;
    AttackerSpawner[] attackerSpawners;

    void Start()
    {
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        attackerSpawners = FindObjectsOfType<AttackerSpawner>();
        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            // Tally up the total number of attackers that will be in this level
            numberOfAttackers += spawner.totalAttackerCount;
        }
    }
    
    public void AttackerKilled()
    {
        numberOfAttackers--;
        if (numberOfAttackers <= 0)
        {
            // End/win the level
            StartCoroutine(HandleWinCondition());
        }
    }

    public void HandleLoseCondition()
    {
        loseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    IEnumerator HandleWinCondition()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        FindObjectOfType<LevelLoader>().LoadNextScene();
    }

    public void CheckIfWaveComplete()
    {
        bool waveComplete = true;

        if (waveNumber == 1)
        {
            foreach (AttackerSpawner spawner in attackerSpawners)
            {
                if (spawner.attackerPrefabsWave1.Count > 0)
                {
                    waveComplete = false;
                    break;
                }
            }
        }
        else if (waveNumber == 2)
        {
            foreach (AttackerSpawner spawner in attackerSpawners)
            {
                if (spawner.attackerPrefabsWave2.Count > 0)
                {
                    waveComplete = false;
                    break;
                }
            }
        }
        else if (waveNumber == 3)
        {
            foreach (AttackerSpawner spawner in attackerSpawners)
            {
                if (spawner.attackerPrefabsWave3.Count > 0)
                {
                    waveComplete = false;
                    break;
                }
            }
        }
        else if (waveNumber == 4)
        {
            foreach (AttackerSpawner spawner in attackerSpawners)
            {
                if (spawner.attackerPrefabsWave4.Count > 0)
                {
                    waveComplete = false;
                    break;
                }
            }
        }
        else if (waveNumber == 5)
        {
            foreach (AttackerSpawner spawner in attackerSpawners)
            {
                if (spawner.attackerPrefabsWave5.Count > 0)
                {
                    waveComplete = false;
                    break;
                }
            }
        }

        if (waveComplete)
        {
            waveNumber++;
            foreach (AttackerSpawner spawner in attackerSpawners)
            {
                spawner.nextWaveDelayed = true;
                if (waveNumber > maxWaves)
                    spawner.StopSpawning();
            }
        }
    }
}
