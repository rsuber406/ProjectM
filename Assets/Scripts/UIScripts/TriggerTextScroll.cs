using UnityEngine;

public class ScrollTextTrigger : MonoBehaviour
{
    public Animator textAnimator;

    public void PlayScrollAnimation()
    {
        textAnimator.SetTrigger("StartScroll");
    }
}