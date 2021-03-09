using UnityEngine;

public enum MeleeWeaponType { Blade, Blunt, Piercing }
public enum RangedWeaponType { Bow, Crossbow, Fireball, Spear }
// public enum DamageType { Blunt, Slash, Piercing, Fire }
public enum SquadType { Knights, Spearmen, Archers }

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

    public void SaveCurrentGame()
    {
        squadData.SaveSquadData();
        ES3AutoSaveMgr.Current.Save();
    }

    public void LoadCurrentGame()
    {
        squadData.LoadSquadData();
        LevelLoader.instance.LoadCurrentLevelNumber();
        if (ES3.FileExists("SaveFile.es3"))
            ES3AutoSaveMgr.Current.Load();
    }

    public void DeleteAllSaveData()
    {
        ES3.DeleteFile("SaveFile.es3");
        ES3.DeleteFile("SquadData.es3");
    }
}