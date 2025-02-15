using System;
using UnityEngine;

public class AttributesController : MonoBehaviour
{
    public AttributeInfo health;
    public AttributeInfo mana;
    public AttributeInfo armor;

    private bool isImmuneToDamage;
    
    public bool IsImmune { get; set; }
    public event Action OnImmune;
    public event Action OnDeath;
    void Awake()
    {
        health.Reset();
        mana.Reset();
        armor.Reset();
    }
    
    public static float CalculateDamageReduction(float armor, float k)
    {
        return armor / (armor + k);
    }
    public void TakeDamage(float baseDamage)
    {
        if (isImmuneToDamage)
        {
            Debug.Log($"Player Immune To damage; Health left: {health.currentValue}");
            OnImmune?.Invoke();
            return;
        }
        
        float damageReduction = CalculateDamageReduction(armor.currentValue, 100f);
        
        float actualDamage = baseDamage * (1f - damageReduction);
        actualDamage = Mathf.Max(actualDamage, 0f);
        health.ReduceValue(actualDamage);
        
        Debug.Log($"Took {actualDamage} damage. Health left: {health.currentValue}");

        if (health.currentValue <= 0f)
        {
            // The player has died, flash that menu
            OnDeath?.Invoke();
            GameManager.GetInstance().LossMenu();
        }
    }
}
