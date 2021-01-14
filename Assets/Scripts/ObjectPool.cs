using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject arrow;
    public int amountOfArrowsToPool = 50;
    public List<GameObject> pooledArrows = new List<GameObject>();

    Transform projectilesParent;
    const string PROJECTILES_PARENT_NAME = "Projectiles";

    #region Singleton
    public static ObjectPool instance;

    void Awake()
    {
        instance = this;
    }
    #endregion
    
    void Start()
    {
        projectilesParent = GameObject.Find(PROJECTILES_PARENT_NAME).transform;
        if (projectilesParent == null)
            projectilesParent = new GameObject(PROJECTILES_PARENT_NAME).transform;

        GameObject temp;
        for (int i = 0; i < amountOfArrowsToPool; i++)
        {
            temp = Instantiate(arrow);
            temp.transform.SetParent(projectilesParent);
            temp.SetActive(false);
            pooledArrows.Add(temp);
        }
    }

    public GameObject GetPooledArrow()
    {
        for (int i = 0; i < pooledArrows.Count; i++)
        {
            if (pooledArrows[i].activeInHierarchy == false)
                return pooledArrows[i];
        }

        GameObject temp = Instantiate(arrow);
        temp.transform.SetParent(projectilesParent);
        temp.SetActive(false);
        pooledArrows.Add(temp);
        return temp;
    }
}
