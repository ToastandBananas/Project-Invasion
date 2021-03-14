using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro textMesh;
    Color textColor;
    string redColor = "FF2B00";
    string yellowColor = "FFC500";

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
                Destroy(gameObject);
        }
    }

    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.instance.damagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }

    public void Setup(float damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());

        if (isCriticalHit == false)
        {
            // Normal Hit
            textMesh.fontSize = 0.8f;
            textColor = Utilities.HexToColor(yellowColor);
        }
        else
        {
            // Critical Hit
            textMesh.fontSize = 1f;
            textColor = Utilities.HexToColor(redColor);
        }

        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }
}
