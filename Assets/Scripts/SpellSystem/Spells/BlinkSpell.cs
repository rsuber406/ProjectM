using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BlinkSpell", menuName = "Spell System/ Spells /BlinkSpell")]
public class BlinkSpell : SpellBase
{
    [Header("Spell Properties")] public float Distance;

    public AudioClip CastAudioClip;
    [Range(0, 1)] public float CastAudioPitch;
    public GameObject ParticleEffects;
    private GameObject effect;

    public override bool CanActivate()
    {
        return spellSystem.HasEnoughMana(cost);
    }

    public override void Activate()
    {
        base.Activate();
        spellSystem.StartCoroutine(CastSpell());
    }

    IEnumerator CastSpell()
    {
        Debug.Log($"Casting {displayName}");

        GameObject player = GameManager.GetInstance().GetPlayer();
        Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();
        AudioSource playerAudioSource = player.GetComponent<AudioSource>();
        PlayerController playerControllerRef = player.GetComponent<PlayerController>();

        // Blink in the cameras forward direction by default
        Transform cameraTransform = GameManager.GetInstance().GetPlayerCamera().transform;
        Vector3 direction = cameraTransform.forward;

        if (playerRigidBody.linearVelocity.magnitude > 0.13)
            direction = playerRigidBody.linearVelocity.normalized;
       
        Vector3 adjustedDirection = playerControllerRef.HandSocket.forward;
        if (playerRigidBody.velocity.z < 0)
        {
            Quaternion adjustRotation = Quaternion.Euler(-15, 0, 0);
            adjustedDirection = adjustRotation * adjustedDirection;
        }
        else
        {
            Quaternion adjustRotation = Quaternion.Euler(15, 0, 0);
            adjustedDirection = adjustRotation * adjustedDirection;
        }
        RaycastHit hit;
        int ignoreLayer = LayerMask.GetMask("Player");
        if (Physics.Raycast(playerControllerRef.HandSocket.position, adjustedDirection, out hit, 10f, ~ignoreLayer))
        {
            if (hit.collider.CompareTag("Stair"))
            {
                Vector3 playerNewPosition = new Vector3(hit.point.x, hit.collider.bounds.max.y, hit.point.z);
                player.transform.position = hit.point;
                playerRigidBody.position = hit.point;
                
            }
            else
            {
                 playerRigidBody.AddForce(direction * Distance, ForceMode.Impulse);
                 player.transform.GetChild(0).rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
                
            }
        }


        float sfxVolume = GameManager.GetInstance().GetSoundManager().audSFX.volume;
        playerAudioSource.PlayOneShot(CastAudioClip, CastAudioPitch * sfxVolume);

        if (ParticleEffects)
        {
            effect = Instantiate(ParticleEffects, player.transform);
        }

        // Cast time , for som reason this is needed before we can call end
        yield return new WaitForSeconds(1);
        End();
        Cancel();
    }

    public override void Cancel()
    {
        Destroy(effect);
    }
}