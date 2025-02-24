using System.Collections;
using UnityEngine;

public class ScrollTextTrigger : MonoBehaviour
{

    public GameObject TexttoScroll;
    bool ismoving = false;
    bool startmoving = false;
    Transform originalPosition;

    private void Start()
    {
        originalPosition = TexttoScroll.transform;
    }
    private void Update()
    {
        if(!ismoving && startmoving)
        {
            StartCoroutine(moveText());
        }

        if (!startmoving)
        {
            TexttoScroll.transform.position = originalPosition.position;
        }
    }

    IEnumerator moveText()
    {
        ismoving = true;
        TexttoScroll.transform.position += new Vector3(0, 1f, 0);
        ismoving = false;
        yield return new WaitForSeconds(0.13f);
    }

    public void StartScrolling()
    {
        if (startmoving)
        {
            startmoving = false;
        }
        else
        {
            startmoving = true;
        }    
    }
}