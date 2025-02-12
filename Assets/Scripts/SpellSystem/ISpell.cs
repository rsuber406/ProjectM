using UnityEngine;

public interface ISpell
{
    void Init(SpellSystem spellSystem);
    bool CanActivate();
    void Activate();
    void Cancel();
}
