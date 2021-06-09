using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public enum ResourceType { Gold, Supplies }
public enum VoiceType { HumanMale, HumanFemale, Skeleton, Lich, Zombie }
public enum MeleeWeaponType { Blade, Blunt, Piercing }
public enum RangedWeaponType { Bow, Crossbow, Fireball, Spear, HealingOrb }
public enum SquadFormation { Line, StaggeredLine, Wedge, Scattered, Wall, Random }
public enum SquadType
{
    Null = 0,
    Laborers = 1,
    Knights = 100,
    Spearmen = 200,
    Archers = 300,
    Priests = 400
}

public enum StructureType
{
    Null = 0,
    WoodenStakes = 100
}

public class GameManager : MonoBehaviour
{
    public InControlManager inControlManagerPrefab;

    [HideInInspector] public SquadData squadData;
    [HideInInspector] public Transform deadCharactersParent;

    public static GameManager instance;
    void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogWarning("More than one instance of GameManager. Fix me!");
                Destroy(gameObject);
            }
        }
        else
            instance = this;
        #endregion

        squadData = GetComponent<SquadData>();

        LoadCurrentGame();
    }

    void Start()
    {
        // This should only ever happen when testing in the Unity Editor, as there will be an InControlManager in the main menu and it will not be destroyed on load
        if (GameObject.Find("InControl Manager") == null)
        {
            InControlManager inControlManager = Instantiate(inControlManagerPrefab);
            inControlManager.name = "InControl Manager";
        }

        if (SceneManager.GetActiveScene().name == "Upgrade Menu" == false)
            deadCharactersParent = GameObject.Find("Dead Characters").transform;
    }

    void Update()
    {
        // --- DEBUG ONLY --- REMOVE
        // Reload the scene 
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        #endif
        // --- DEBUG ONLY --- REMOVE
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