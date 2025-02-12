using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public GameObject spellbar;

    public bool enableSpellBarOnStart;
    
    void Start()
    {
        if (spellbar && enableSpellBarOnStart)
        {
            EnableSpellBar();
        }
        else
        {
            DisableSpellBar();
        }
    }

    void EnableSpellBar()
    {
        spellbar.SetActive(true);
    }
    
    void DisableSpellBar()
    {
        spellbar.SetActive(false);
    }
}
