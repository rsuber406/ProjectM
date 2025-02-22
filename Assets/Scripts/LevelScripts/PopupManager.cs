using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    private static PopupManager instance;
    private Coroutine mainCoroutine;

    private string[] messages = {
        "Test1",
        "Test2",
        "Test3",
        "Test4",
        "Test5"
    };

    private void Awake()
    {
        popupPanel.SetActive(false);
    }

    public static void ShowPopup(int messageIndex, float duration = 4f)
    {
         instance.popupText.text = instance.messages[messageIndex];
         instance.popupPanel.SetActive(true);

         instance.StopCoroutine(instance.mainCoroutine);

         instance.mainCoroutine = instance.StartCoroutine(instance.HidePopup(duration));

    }

    private IEnumerator HidePopup(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupPanel.SetActive(false);
    }
}