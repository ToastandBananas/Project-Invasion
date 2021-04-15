using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    static ObjectPool damagePopupObjectPool;

    TextMeshPro textMesh;
    Color textColor;

    string defenderDefaultHitColor = "FF7700"; // Orangish-Yellow
    string attackerDefaultHitColor = "FF1100"; // Reddish-Orange

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
        damagePopupObjectPool = GameObject.Find("Effects").transform.Find("Damage Popups").GetComponent<ObjectPool>();
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

    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCriticalHit, bool isDefender)
    {
        DamagePopup damagePopup = damagePopupObjectPool.GetPooledObject().GetComponent<DamagePopup>();
        
        damagePopup.Setup(position, damageAmount, isCriticalHit, isDefender);

        return damagePopup;
    }

    public void Setup(Vector3 position, float damageAmount, bool isCriticalHit, bool isDefender)
    {
        // Reset the size and disappear timer
        transform.localScale = Vector3.one;
        disappearTimer = 0f;

        textMesh.SetText(damageAmount.ToString());

        if (isDefender)
        {
            // Defender being hit
            textColor = Utilities.HexToRGBAColor(defenderDefaultHitColor);
        }
        else
        {
            // Attacker being hit
            textColor = Utilities.HexToRGBAColor(attackerDefaultHitColor);
        }

        if (isCriticalHit)
        {
            // Critical Hit
            textMesh.fontSize = 1.4f;
            // textColor = Utilities.HexToColor(redColor);
        }
        else
        {
            // Normal Hit
            textMesh.fontSize = 1f;
            // textColor = Utilities.HexToColor(yellowColor);
        }

        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        gameObject.SetActive(true);
        transform.position = position;
    }
}
