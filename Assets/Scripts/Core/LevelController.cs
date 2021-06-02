using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject winCanvas, loseCanvas;

    [Header("Wave Info")]
    [SerializeField] int maxWaves = 5;
    public float waveDelay = 10f;

    [Header("Squad Unlock")]
    public SquadType squadUnlock;

    [Header("Rewards")]
    public int upgradePointsReward = 100;

    [HideInInspector] public int waveNumber = 1;
    [HideInInspector] public int numberOfAttackers = 0;

    [HideInInspector] public bool levelLost;

    AudioManager audioManager;
    AttackerSpawner[] attackerSpawners;
    SquadData squadData;

    #region Singleton
    public static LevelController instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogWarning("More than one instance of LevelController. Fix me!");
                Destroy(gameObject);
            }
        }
        else
            instance = this;
    }
    #endregion

    void Start()
    {
        levelLost = false;

        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);

        audioManager = AudioManager.instance;
        squadData = GameManager.instance.squadData;

        ApplySquadUnlock();

        attackerSpawners = FindObjectsOfType<AttackerSpawner>();
        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            // Tally up the total number of attackers that will be in this level...once it reaches 0, we will know that the level is complete
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
        if (levelLost == false)
        {
            levelLost = true;

            audioManager.PlaySound(audioManager.failSounds, audioManager.failSounds[0].soundName, Vector3.zero);

            // Deactivate all TextPopups since they stay in front of the level lost canvas
            GameObject.Find("Text Popups").SetActive(false);

            loseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator HandleWinCondition()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);

        ApplyUpgradePointReward();

        int randomIndex = Random.Range(0, audioManager.victorySounds.Length);
        audioManager.PlaySound(audioManager.victorySounds, audioManager.victorySounds[randomIndex].soundName, Vector3.zero);

        yield return new WaitForSeconds(audioManager.victorySounds[randomIndex].clip.length);

        LevelLoader.instance.LoadUpgradeMenuScene();
    }

    void ApplyUpgradePointReward()
    {
        ES3.Save("upgradePoints", ES3.Load("upgradePoints", 0) + upgradePointsReward);
    }

    void ApplySquadUnlock()
    {
        if (squadUnlock != SquadType.Null)
            squadData.UnlockSquad(squadUnlock);
    }

    public void CheckIfWaveComplete()
    {
        bool waveComplete = true;

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            if (spawner.transform.childCount > 0)
            {
                waveComplete = false;
                return;
            }
        }

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
                if (waveNumber > maxWaves)
                    spawner.StopSpawning();
                else
                    spawner.StartNextWaveDelay();
            }
        }
    }
}
