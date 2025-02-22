using UnityEngine;
using TMPro;
using System.Collections;

public class PopupTrigger : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    private Coroutine mainCoroutine;
    public int messageIndex;


    private string[] messages = {
        "Test1",
        "Test2",
        "Test3",
        "Test4",
        "Test5"
    };

    public void ShowPopup(int messageIndex, float duration = 4f)
    {
        popupText.text = messages[messageIndex];
        popupPanel.SetActive(true);

        //StopCoroutine(mainCoroutine);

        mainCoroutine = StartCoroutine(HidePopup(duration));

    }

    private IEnumerator HidePopup(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPopup(messageIndex, 4f);
        }
    }
}
