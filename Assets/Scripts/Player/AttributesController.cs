using UnityEngine;

public class AttributesController : MonoBehaviour
{
    public AttributeInfo health;
    public AttributeInfo mana;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health.Reset();
        mana.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
