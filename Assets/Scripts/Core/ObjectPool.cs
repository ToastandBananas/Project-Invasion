﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool = 50;
    public List<GameObject> pooledObjects = new List<GameObject>();
    
    void Start()
    {
        if (objectToPool.name == "Damage Popup" && PlayerPrefsController.DamagePopupsEnabled() == false)
            amountToPool = 1;

        GameObject temp;
        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool);
            temp.transform.SetParent(transform);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false)
                return pooledObjects[i];
        }

        GameObject temp = Instantiate(objectToPool);
        temp.transform.SetParent(transform);
        pooledObjects.Add(temp);
        return temp;
    }
}
