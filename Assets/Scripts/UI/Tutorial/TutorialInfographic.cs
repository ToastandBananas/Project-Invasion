using UnityEngine;

public class TutorialInfographic : MonoBehaviour
{
    public void Start()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
            HideInfographic();
    }

    public void ShowInfographic()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideInfographic()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
