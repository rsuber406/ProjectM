using UnityEngine;
using TMPro;
using System.Collections;

public class PopupTrigger : MonoBehaviour
{
    public GameObject popupPanel;
    public GameObject popupTrigger;
    public TextMeshProUGUI popupText;

    private Coroutine mainCoroutine;
    public int messageIndex;


    private string[] messages = {
        "Welcome to Dungeon Delvers! You are in a Djinn's Lair and must plunder it for glory!",
        "His Dungeon is full of monsters that want to kill you\nFight back using your fireball! [Mouse1 or 1].",
        "You Found some Loot!\nEquip it from your Bag [B].",
        "Sometimes there are too many monsters to fight\nUse your Blink [2] or Dash [Spacebar] to evade enemies.",
        "Other times the way through is not immediately clear..\nExplore a bit, find some treasure and find an exit portal!",
        "The exit is just up ahead! Use your Shield ability [3] to block incoming damage",
        "Well Done! Up ahead is the Hub. A safe space to recuperate. From there you can Delve Further into the Dungeon."
    };

    public void ShowPopup(int messageIndex, float duration = 4f)
    {
        popupText.text = messages[messageIndex];
        popupPanel.SetActive(true);


        mainCoroutine = StartCoroutine(HidePopup(duration));

    }

    private IEnumerator HidePopup(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupPanel.SetActive(false);
        popupTrigger.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player"))
        {
            ShowPopup(messageIndex, 5f);
        }
    }
}
