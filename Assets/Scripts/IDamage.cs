using UnityEngine;

public interface IDamage
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  public  void TakeDamage(int amount, DamageSourceType type);
}
