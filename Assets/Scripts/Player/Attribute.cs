using System;
using UnityEngine;

public enum Attribute
{
    Health,
    Mana,
    Armor
}

[Serializable]
public struct AttributeInfo
{
    public Attribute attribute;
    
    public float currentValue;
    public float maxValue;
    public float minValue;

    public void Reset()
    {
        currentValue = maxValue;
    }

    public void AddValue(float value)
    {
        currentValue += value;
        Mathf.Clamp(currentValue, minValue, maxValue);
    }

    public void ReduceValue(float value)
    {
        currentValue -= value;
        Mathf.Clamp(currentValue, minValue, maxValue);
    }
}