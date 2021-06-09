using UnityEngine;

public class UpgradeTreeSelector : MonoBehaviour
{
    public SquadType squadType;
    public StructureType structureType;
    public GameObject upgradeTree;

    Transform upgradeTreesParent;
    Transform upgradeIconsParent;
    AudioManager audioManager;

    void Start()
    {
        upgradeTreesParent = GameObject.Find("Upgrade Trees").transform;
        upgradeIconsParent = upgradeTree.transform.Find("Upgrade Icons");

        audioManager = AudioManager.instance;

        SetSquadTypeForIcons();

        if (GameManager.instance.squadData.SquadUnlocked(squadType) == false && GameManager.instance.squadData.StructureUnlocked(structureType) == false)
            gameObject.SetActive(false);
    }

    public void SelectSkillTree()
    {
        for (int i = 0; i < upgradeTreesParent.transform.childCount; i++)
        {
            if (upgradeTreesParent.transform.GetChild(i).gameObject.activeSelf)
            {
                upgradeTreesParent.transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }

        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
        upgradeTree.SetActive(true);
    }

    void SetSquadTypeForIcons()
    {
        for (int i = 0; i < upgradeIconsParent.childCount; i++)
        {
            if (upgradeIconsParent.GetChild(i).name == "Lines")
                continue;

            for (int j = 0; j < upgradeIconsParent.GetChild(i).childCount; j++)
            {
                upgradeIconsParent.GetChild(i).GetChild(j).TryGetComponent(out UpgradeIcon upgradeIcon);
                upgradeIcon.squadType = squadType;
                upgradeIcon.structureType = structureType;
            }
        }
    }
}
