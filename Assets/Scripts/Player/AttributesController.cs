using System;
using System.Collections;
using UnityEngine;

public class AttributesController : MonoBehaviour
{
    public AttributeInfo health;
    public AttributeInfo mana;
    public AttributeInfo armor;
    public float healthRegenRate;
    public float manaRegenRate;

    private bool isImmuneToDamage;

    public bool IsImmune { get; set; }
    public event Action OnImmune;
    public event Action OnDeath;

    private bool isHealing = false;
    private bool isDead = false;

    void Awake()
    {
        health.Reset();
        mana.Reset();
        armor.Reset();
    }

    void Update()
    {
        // Dont update attributes if not un dungeon...
        if (GameManager.GetInstance().GetGameMode()!= GameMode.Dungeon) return;

        if (!isHealing && !isDead)
            StartCoroutine(RegenerateAttributes());
    }


    private IEnumerator RegenerateAttributes()
    {
        isHealing = true;
        yield return new WaitForSeconds(1f);

        if (health.currentValue < health.maxValue)
        {
            health.AddValue(healthRegenRate);
        }

        if (mana.currentValue < mana.maxValue)
        {
            mana.AddValue(manaRegenRate);
        }

        isHealing = false;
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
            PlayerController controller = GameManager.GetInstance().GetPlayer().GetComponent<PlayerController>();
            controller.DeathSequence();
            isDead = true;
        }
    }

    public void ResetStatsAfterDeath()
    {
        health.Reset();
        mana.Reset();
        isDead = false;
    }
}