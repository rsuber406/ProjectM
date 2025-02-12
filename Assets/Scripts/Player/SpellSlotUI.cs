using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    public SpellSlotEnum spellslot;
    public Image background;
    public Image border;

    private Image originalBackground;
    public void Init(SpellBase spell)
    {
        originalBackground = background;
        background.sprite = spell.icon;
    }

    public void ClearSlot()
    {
        background = originalBackground;
    }
}
