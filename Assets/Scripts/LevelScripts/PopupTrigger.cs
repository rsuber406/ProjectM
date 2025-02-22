using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public int messageIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PopupManager.ShowPopup(messageIndex, 4f);
        }
    }
}
