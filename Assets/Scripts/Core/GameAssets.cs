using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Transform damagePopup;

    #region Singleton
    public static GameAssets instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
        else
            instance = this;
    }
    #endregion
}
