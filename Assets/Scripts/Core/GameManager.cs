using UnityEngine;
using UnityEngine.SceneManagement;

public enum ResourceType { Gold, Supplies }
public enum VoiceType { HumanMale, HumanFemale, Undead }
public enum MeleeWeaponType { Blade, Blunt, Piercing }
public enum RangedWeaponType { Bow, Crossbow, Fireball, Spear }
public enum SquadFormation { Line, StaggeredLine, Wedge, Scattered, Wall, Random }
public enum SquadType
{
    Null = 0,
    Laborers = 1,
    Knights = 100,
    Spearmen = 200,
    Archers = 300
}

public enum StructureType
{
    Null = 0,
    WoodenStakes = 100
}

public class GameManager : MonoBehaviour
{
    [HideInInspector] public SquadData squadData;
    
    public static GameManager instance;
    void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        #endregion

        squadData = GetComponent<SquadData>();

        LoadCurrentGame();
    }

    void Update()
    {
        // Reload the scene 
        // --- DEBUG ONLY --- REMOVE
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // --- DEBUG ONLY --- REMOVE
        #endif
    }

    public void SaveCurrentGame()
    {
        // Save Squad Data
        squadData.SaveSquadData();

        // Autosave
        ES3AutoSaveMgr.Current.Save();


        // Save upgrade point total if the player is in the Upgrade Menu
        if (SceneManager.GetSceneByName("Upgrade Menu") == SceneManager.GetActiveScene())
            UpgradeManager.instance.SaveUpgradePoints();
    }

    public void LoadCurrentGame()
    {
        // Load in saved Squad Data, so that any upgrades can be applied to squads as they are spawned
        squadData.LoadSquadData();

        // Load the current level number so that if the player loads their game from the main menu, our Level Loader will know which level to load
        LevelLoader.instance.LoadCurrentLevelNumber(); 

        // Load in autosave data
        if (ES3.FileExists("SaveFile.es3"))
            ES3AutoSaveMgr.Current.Load();

        // Load in upgrade points if the player is in the Upgrade Menu
        if (SceneManager.GetSceneByName("Upgrade Menu") == SceneManager.GetActiveScene())
            UpgradeManager.instance.LoadUpgradePoints();
    }

    public void DeleteAllSaveData()
    {
        ES3.DeleteFile("SaveFile.es3");
        ES3.DeleteFile("SquadData.es3");
    }
}