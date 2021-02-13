using UnityEngine;

public enum MeleeWeaponType { Blade, Blunt, Piercing }
public enum RangedWeaponType { Bow, Crossbow, Fireball, Spear }
public enum SquadType { Knights, Spearmen, Archers }

public class GameManager : MonoBehaviour
{
    [HideInInspector] public SquadData squadData;

    
    public static GameManager instance;
    void Awake()
    {
        squadData = GetComponent<SquadData>();

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
        ES3AutoSaveMgr.Current.Load();
    }
}