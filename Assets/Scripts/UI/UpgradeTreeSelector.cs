using UnityEngine;

public class UpgradeTreeSelector : MonoBehaviour
{
    public GameObject upgradeTree;

    GameObject upgradeTreeParent;

    void Start()
    {
        upgradeTreeParent = GameObject.Find("Upgrade Trees");
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

        upgradeTree.SetActive(true);
    }
}
