using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject resumeImage;
    public GameObject settingsImage;
    public GameObject quitGameImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited");
    }
}
