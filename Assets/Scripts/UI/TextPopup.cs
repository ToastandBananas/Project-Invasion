using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    public Sprite goldIcon, suppliesIcon;

    SpriteRenderer resourceIcon;
    RectTransform resourceIconRectTransform;

    static ObjectPool textPopupObjectPool;

    TextMeshPro textMesh;
    Color textColor;

    string defenderDefaultHitColor = "FF7700"; // Orangish-Yellow
    string attackerDefaultHitColor = "FF1100"; // Reddish-Orange

    Color positiveValueColor = Color.green; // Green
    Color negativeValueColor = Color.red;   // Red

    static int sortingOrder;

    const float DISAPPEAR_TIMER_MAX = 1f;
    float disappearTimer;
    float disappearSpeed = 3f;
    float increaseScaleAmount = 0.2f;
    float decreaseScaleAmount = 0.25f;
    
    Vector3 moveVector = new Vector3(0.2f, 0.4f);

    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textPopupObjectPool = GameObject.Find("Effects").transform.Find("Text Popups").GetComponent<ObjectPool>();

        resourceIcon = GetComponentInChildren<SpriteRenderer>();
        resourceIconRectTransform = resourceIcon.GetComponent<RectTransform>();
    }

    void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            // First half of the popup's lifetime
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half of the popup's lifetime
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0f)
        {
            // Start disappearing
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <= 0f)
                gameObject.SetActive(false);
        }
    }

    // Create a damage popup
    public static TextPopup CreateDamagePopup(Vector3 position, float damageAmount, bool isCriticalHit, bool isDefenderOrCastle)
    {
        TextPopup damagePopup = textPopupObjectPool.GetPooledObject().GetComponent<TextPopup>();
        
        damagePopup.SetupDamagePopup(position, damageAmount, isCriticalHit, isDefenderOrCastle);

        return damagePopup;
    }

    // Create a heal popup
    public static TextPopup CreateHealPopup(Vector3 position, float healAmount)
    {
        TextPopup healPopup = textPopupObjectPool.GetPooledObject().GetComponent<TextPopup>();

        healPopup.SetupHealPopup(position, healAmount);

        return healPopup;
    }
    
    // Create a resource popup in the resource display for when the player uses resources
    public static TextPopup CreateResourceDisplayPopup(Vector3 position, float amount)
    {
        TextPopup resourcePopup = textPopupObjectPool.GetPooledObject().GetComponent<TextPopup>();

        resourcePopup.SetupResourceDisplayPopup(position, amount);

        return resourcePopup;
    }

    // Create a resource popup next to Laborers when they gain gold or supplies
    public static TextPopup CreateResourceGainPopup(Vector3 position, float amount, ResourceType resourceType)
    {
        TextPopup resourceGainPopup = textPopupObjectPool.GetPooledObject().GetComponent<TextPopup>();

        resourceGainPopup.SetupResourceGainPopup(position, amount, resourceType);

        return resourceGainPopup;
    }

    void SetupDamagePopup(Vector3 position, float damageAmount, bool isCriticalHit, bool isDefenderOrStructure)
    {
        ResetPopup();

        textMesh.SetText(damageAmount.ToString());

        if (isDefenderOrStructure)
        {
            // Defender being hit
            textColor = Utilities.HexToRGBAColor(defenderDefaultHitColor);
        }
        else
        {
            // Attacker being hit
            textColor = Utilities.HexToRGBAColor(attackerDefaultHitColor);
        }

        textMesh.color = textColor;

        if (isCriticalHit)
        {
            // Critical Hit
            textMesh.fontSize = 1.4f;
        }
        else
        {
            // Normal Hit
            textMesh.fontSize = 1f;
        }

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        gameObject.SetActive(true);
        transform.position = position;
    }

    void SetupHealPopup(Vector3 position, float healAmount)
    {
        ResetPopup();

        textMesh.SetText(healAmount.ToString());
        textMesh.color = positiveValueColor;
        textMesh.fontSize = 1f;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        gameObject.SetActive(true);
        transform.position = position;
    }

    void SetupResourceDisplayPopup(Vector3 position, float amount)
    {
        ResetPopup();

        if (amount > 0f) // Gaining resources
        {
            textMesh.SetText("+" + amount.ToString());
            textColor = positiveValueColor;
        }
        else if (amount < 0f) // Using resources
        {
            textMesh.SetText(amount.ToString());
            textColor = negativeValueColor;
        }
        else
        {
            Debug.LogWarning("Using 0 resources...something may be wrong here...Fix me!?");
            return;
        }
        
        textMesh.fontSize = 1.4f;
        textMesh.color = textColor;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        gameObject.SetActive(true);
        transform.position = position;
    }

    void SetupResourceGainPopup(Vector3 position, float amount, ResourceType resourceType)
    {
        ResetPopup();
        
        textMesh.SetText("+" + amount.ToString());
        textColor = positiveValueColor;

        textMesh.fontSize = 1f;
        textMesh.color = textColor;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        if (resourceType == ResourceType.Gold)
        {
            resourceIcon.enabled = true;
            resourceIcon.sprite = goldIcon;
            resourceIconRectTransform.localScale = new Vector3(0.75f, 0.75f);
        }
        else if (resourceType == ResourceType.Supplies)
        {
            resourceIcon.enabled = true;
            resourceIcon.sprite = suppliesIcon;
            resourceIconRectTransform.localScale = new Vector3(0.15f, 0.15f);
        }

        gameObject.SetActive(true);
        transform.position = position;
    }

    void ResetPopup()
    {
        // Reset the size, move vector and disappear timer
        transform.localScale = Vector3.one;
        moveVector = new Vector3(0.2f, 0.4f);
        disappearTimer = DISAPPEAR_TIMER_MAX;
        resourceIcon.enabled = false;
    }
}
