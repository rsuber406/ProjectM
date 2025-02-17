using UnityEngine;
[CreateAssetMenu]
public class SpellScriptable : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int manaCost;
    public int damage;
    public int range;
    public int speed;
    public int radius;
    public ParticleSystem particleEffect;
    public AudioClip soundEffect;
}
