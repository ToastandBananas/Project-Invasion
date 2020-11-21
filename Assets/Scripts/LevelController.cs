using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject winCanvas, loseCanvas;

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
            numberOfAttackers += spawner.attackerPrefabs.Length;
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
        winCanvas.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        FindObjectOfType<LevelLoader>().LoadNextScene();
    }
}
