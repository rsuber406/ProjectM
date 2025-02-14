using UnityEngine;

public class AttributesController : MonoBehaviour
{
    public AttributeInfo health;
    public AttributeInfo mana;
    public AttributeInfo armor;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health.Reset();
        mana.Reset();
        armor.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float CalculateDamageReduction(float armor, float k)
    {
        return armor / (armor + k);
    }
    public void TakeDamage(float baseDamage)
    {
        float damageReduction = CalculateDamageReduction(armor.currentValue, 100f);
        
        float actualDamage = baseDamage * (1f - damageReduction);
        actualDamage = Mathf.Max(actualDamage, 0f);
        health.ReduceValue(actualDamage);
        
        Debug.Log($"Took {actualDamage} damage. Health left: {health.currentValue}");
        
    }
}
