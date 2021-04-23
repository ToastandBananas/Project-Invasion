using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] int gold = 100;
    [SerializeField] int supplies = 100;

    AudioManager audioManager;
    Text goldText, suppliesText;

    bool shouldGenerateGold = false;
    float generateGoldWaitTime = 5f;
    float updateDisplayWaitTime = 0.01f;

    #region Singleton
    public static ResourceDisplay instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        audioManager = AudioManager.instance;

        goldText = transform.Find("Gold").GetComponentInChildren<Text>();
        suppliesText = transform.Find("Supplies").GetComponentInChildren<Text>();

        goldText.text = gold.ToString();
        suppliesText.text = supplies.ToString();

        StartCoroutine(GenerateGold());
        StartCoroutine(UpdateGoldDisplay());
        StartCoroutine(UpdateSuppliesDisplay());
    }

    IEnumerator GenerateGold()
    {
        while (shouldGenerateGold)
        {
            yield return new WaitForSeconds(generateGoldWaitTime);
            AddGold(50);
        }
    }

    IEnumerator UpdateGoldDisplay()
    {
        while (true)
        {
            if (goldText.text != gold.ToString())
            {
                yield return new WaitForSeconds(updateDisplayWaitTime);

                int currentGold = int.Parse(goldText.text);

                if (currentGold < gold)
                    currentGold++;
                else
                    currentGold--;
                
                goldText.text = currentGold.ToString();
            }
            else
                yield return null;
        }
    }

    IEnumerator UpdateSuppliesDisplay()
    {
        while (true)
        {
            if (suppliesText.text != supplies.ToString())
            {
                yield return new WaitForSeconds(updateDisplayWaitTime);

                int currentSupplies = int.Parse(suppliesText.text);

                if (currentSupplies < supplies)
                    currentSupplies++;
                else
                    currentSupplies--;

                suppliesText.text = currentSupplies.ToString();
            }
            else
                yield return null;
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        audioManager.PlayRandomSound(audioManager.goldSounds);
        //TextPopup.CreateResourcePopup(new Vector3(7.8f, 5.97f), amount);
    }

    public void SpendGold(int amount)
    {
        if (HaveEnoughGold(amount))
        {
            gold -= amount;
            audioManager.PlayRandomSound(audioManager.goldSounds);
            TextPopup.CreateResourcePopup(new Vector3(7.8f, 5.94f), -amount);
        }
    }

    public bool HaveEnoughGold(int goldAmount)
    {
        return gold >= goldAmount;
    }

    public void AddSupplies(int amount)
    {
        supplies += amount;
        //TextPopup.CreateResourcePopup(suppliesText.transform.position - new Vector3(-1f, 0), amount);
    }

    public void SpendSupplies(int amount)
    {
        if (HaveEnoughSupplies(amount))
        {
            supplies -= amount;
            TextPopup.CreateResourcePopup(new Vector3(9.25f, 5.94f), -amount);
        }
    }

    public bool HaveEnoughSupplies(int suppliesAmount)
    {
        return supplies >= suppliesAmount;
    }
}
