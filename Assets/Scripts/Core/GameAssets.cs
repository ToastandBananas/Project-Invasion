using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [Header("Effects")]
    public Transform textPopup;

    [Header("Resource Deposit Animator Controllers")]
    public List<RuntimeAnimatorController> goldDepositAnimatorControllers = new List<RuntimeAnimatorController>();
    public List<ResourceDeposit> goldDeposits = new List<ResourceDeposit>();

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
