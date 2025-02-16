using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject spellbar;
    [SerializeField] private GameObject spellActivationMessageObj;
    [SerializeField] private float spellActivationTimer;
    public bool enableSpellBarOnStart;
    
    private TMP_Text spellActivationMessage;
    private SpellSystem playerSpellSystem;
    
    void Start()
    {
        GameObject go = GameManager.GetInstance().GetPlayer();
        playerSpellSystem = go.GetComponentInChildren<SpellSystem>();
        
        playerSpellSystem.OnSpellOnCoolDown += () =>
        {
            StartCoroutine(ShowSpellActivationMessage("Spell On Cooldown"));
        };
        playerSpellSystem.OnSpellSystemBusy += () =>
        {
            StartCoroutine(ShowSpellActivationMessage("Cannot Activate Spell Now"));
        };
        
        playerSpellSystem.OnInsufficientMana += () =>
        {
            StartCoroutine(ShowSpellActivationMessage("Not enough mana"));
        };
        
        if (spellActivationMessageObj)
        {
            spellActivationMessage = spellActivationMessageObj.GetComponent<TMP_Text>();
            spellActivationMessageObj.SetActive(false);
        }
        
        PlayerAnimation animationControllerRef = go.GetComponentInChildren<PlayerAnimation>();
        animationControllerRef.onActionModeEnabled += EnableCrossHair;
        animationControllerRef.onActionModeDisabled += DisableCrossHair;
        DisableCrossHair();
        
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
    
    void EnableCrossHair()
    {
        crossHair.SetActive(true);
    }
    
    void DisableCrossHair()
    {
        crossHair.SetActive(false);
    }

    IEnumerator ShowSpellActivationMessage(string message)
    {
        if (!spellActivationMessage)
        {
            Debug.LogError("Unassigned SpellActivation Message Game Object or TMP");
            yield return null;
        }
        spellActivationMessage.text = message;
        spellActivationMessageObj.SetActive(true);
        
        yield return new WaitForSeconds(spellActivationTimer);
        
        spellActivationMessageObj.SetActive(false);
        spellActivationMessage.text = "";
    }
}
