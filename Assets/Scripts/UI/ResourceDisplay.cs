using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] int gold = 100;
    [SerializeField] int supplies = 100;

    AudioManager audioManager;
    Text goldText, suppliesText;

    [HideInInspector] public bool shouldGenerateGold = true;
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
    }

    public void SpendGold(int amount)
    {
        if (HaveEnoughGold(amount))
        {
            gold -= amount;
            audioManager.PlayRandomSound(audioManager.goldSounds);
        }
    }

    public bool HaveEnoughGold(int goldAmount)
    {
        return gold >= goldAmount;
    }

    public void AddSupplies(int amount)
    {
        supplies += amount;
    }

    public void SpendSupplies(int amount)
    {
        if (HaveEnoughSupplies(amount))
            supplies -= amount;
    }

    public bool HaveEnoughSupplies(int suppliesAmount)
    {
        return supplies >= suppliesAmount;
    }
}
