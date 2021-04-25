using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [Header("Effects")]
    public Transform textPopup;

    [Header("Resource Deposits")]
    public ResourceDeposit goldDeposit;
    public List<RuntimeAnimatorController> goldDepositAnimatorControllers = new List<RuntimeAnimatorController>();

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
