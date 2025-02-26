using System.Collections;
using UnityEngine;

public class ScrollTextTrigger : MonoBehaviour
{
    [SerializeField][Range(1, 10)] int scrollSpeed;
    public GameObject TextToScroll;
    private bool ismoving = false;
    private bool startmoving = false;
    private Transform originalPosition;

    public float x;
    public float y;
    public float z;

    void Start()
    {
        originalPosition.position = TextToScroll.transform.position;
    }
    void Update()
    {
        x = originalPosition.position.x;
        y = originalPosition.position.y;
        z = originalPosition.position.z;
        x = 1; 
        if (MainSceneLogic.GetInstance().CreditsActivateables.activeInHierarchy)
          MoveText();

        else
            TextToScroll.transform.position = new Vector3(originalPosition.position.x, originalPosition.position.y, originalPosition.position.z);
    }

    private void MoveText()
    {
        TextToScroll.transform.position = new Vector3(TextToScroll.transform.position.x, TextToScroll.transform.position.y + scrollSpeed, TextToScroll.transform.position.z);
    }
}