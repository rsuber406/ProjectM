using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    public SpellSlotEnum spellslot;
    public Image background;
    public Image border;

    public TMP_Text CoolDownTimer;
    public GameObject CoolDownLayer;

    private Image originalBackground;
    private RuntimeSpell trackedSpell;
    public void Init(RuntimeSpell inSpell)
    {
        trackedSpell = inSpell;
        
        originalBackground = background;
        background.sprite = inSpell.spell.icon;
        
        CoolDownLayer.SetActive(false);
    }

    public void ClearSlot()
    {
        background = originalBackground;
    }

    private void Update()
    {
        if (trackedSpell.IsCooldownActive() && !CoolDownLayer.activeSelf)
        {
            CoolDownLayer.SetActive(true);
        }
        else if (!trackedSpell.IsCooldownActive())
        {
            CoolDownLayer.SetActive(false);
        }

        CoolDownTimer.text = $"{trackedSpell.GetRemainingCooldown():N0}'s";
    }
}
