using System.Collections;
using UnityEngine;

public class SquadHighlighter : MonoBehaviour
{
    #region Singleton
    public static SquadHighlighter instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogWarning("More than one instance of SquadHighlighter. Fix me!");
                Destroy(gameObject);
            }
        }
        else
            instance = this;
    }
    #endregion

    public Sprite validHighlightSprite, invalidHighlightSprite;
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public bool isActive;
    [HideInInspector] public Squad selectedSquad;

    void Start()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isActive && selectedSquad == null)
        {
            Vector2 mouseWorldPos = Utilities.GetMouseWorldPosition();
            transform.position = new Vector2(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y));
            spriteRenderer.sprite = invalidHighlightSprite;
        }
    }

    public void ShowValidHighlight()
    {
        if (isActive == false)
            EnableHighlighter();

        transform.position = selectedSquad.transform.position;
        spriteRenderer.sprite = validHighlightSprite;
    }

    public void ShowInvalidHighlight()
    {
        if (isActive == false)
            EnableHighlighter();

        transform.position = selectedSquad.transform.position;
        spriteRenderer.sprite = invalidHighlightSprite;
    }

    public void EnableHighlighter()
    {
        isActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DisableHighlighter()
    {
        isActive = false;
        selectedSquad = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public IEnumerator DelayDisableHighlighter()
    {
        yield return new WaitForSeconds(0.1f);
        if (selectedSquad == null)
            DisableHighlighter();
    }
}
