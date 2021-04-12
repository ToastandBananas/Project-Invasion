﻿using UnityEngine;

public class UpgradeTreeSelector : MonoBehaviour
{
    public GameObject upgradeTree;

    GameObject upgradeTreeParent;
    AudioManager audioManager;

    void Start()
    {
        upgradeTreeParent = GameObject.Find("Upgrade Trees");
        audioManager = AudioManager.instance;
    }

    public void SelectSkillTree()
    {
        for (int i = 0; i < upgradeTreeParent.transform.childCount; i++)
        {
            if (upgradeTreeParent.transform.GetChild(i).gameObject.activeSelf)
            {
                upgradeTreeParent.transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }

        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
        upgradeTree.SetActive(true);
    }
}
