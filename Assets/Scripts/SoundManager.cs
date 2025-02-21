using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public PlayerController player;
    private AudioSource aud;

    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider musicSlider;

    public TMP_Text masterVolumeText;
    public TMP_Text SFXVolumeText;
    public TMP_Text musicVolumeText;

    public float masterVol;
    public float SFXVol;
    public float musicVol;


    [Header ("----- Player Sounds -----")]
    [SerializeField] AudioClip[] playerFootsteps;
    public bool isPlayingSteps;
    [SerializeField] AudioClip[] playerHurtSounds;
    [SerializeField] AudioClip[] playerDeathSounds;
    [SerializeField] AudioClip[] playerDodgeSounds;

    [Header("----- Spell Cast Sounds -----")]
    [SerializeField] AudioClip fireSpell;
    [SerializeField] AudioClip blinkSpell;
    [SerializeField] AudioClip shieldSpell;

    [Header("----- UI Sounds -----")]
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip ambience;


    private float ambienceTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aud = GetComponent<AudioSource>();
        ResetVolume();
        masterVol = SFXVol = musicVol = masterSlider.value;
        masterVolumeText.text = SFXVolumeText.text = musicVolumeText.text = (100f).ToString("F0");

    }

    // Update is called once per frame
    void Update()
    {

        musicVol = masterVol * musicSlider.value;
        SFXVol = masterVol * SFXSlider.value;

    }


    public void GetMasterVolume()
    {
        masterVol = masterSlider.value;
        masterVolumeText.text = (masterSlider.value * 100f).ToString("F0");
    }


    public void GetSFXVolume()
    {
        SFXVol = masterVol * SFXSlider.value;
        SFXVolumeText.text = (SFXSlider.value * 100f).ToString("F0");
    }

    public void GetMusicVolume()
    {
        musicVol = masterVol * musicSlider.value;
        musicVolumeText.text = (musicSlider.value * 100f).ToString("F0");
    }

    private void ResetVolume()
    {
        masterSlider.value = 1f;
        SFXSlider.value = musicSlider.value = masterSlider.value;
    }

    public IEnumerator PlayerSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)], SFXVol);
        
        if (!player.inCombat)
            yield return new WaitForSeconds(0.4f);
        else
            yield return new WaitForSeconds(0.35f);

        isPlayingSteps = false;
    }

    public void PlayerHurt()
    {
        aud.PlayOneShot(playerHurtSounds[Random.Range(0, playerHurtSounds.Length)], SFXVol);
    }

    public void PlayerDeath()
    {
        aud.PlayOneShot(playerDeathSounds[Random.Range(0, playerDeathSounds.Length)], SFXVol);
    }

    public void PlayerDodge()
    {
        aud.PlayOneShot(playerDodgeSounds[Random.Range(0, playerDodgeSounds.Length)], SFXVol);
    }

    public void FireSpell()
    {
        aud.PlayOneShot(fireSpell, SFXVol);
    }

    public void BlinkSpell()
    {
        aud.PlayOneShot(blinkSpell, SFXVol);
    }

    public void ShieldSpell()
    {
        aud.PlayOneShot(shieldSpell, SFXVol);
    }

    public void LossJingle()
    {
        aud.PlayOneShot(lose, musicVol);
    }

    public void Ambience()
    {
/*
        if (ambienceTimer > 0)
        {
            if (Time.deltaTime == 0)
            {
                ambienceTimer = 55.5f;
                aud.Pause();
            }
            else
            {
                ambienceTimer -= Time.deltaTime;
                return;
            }

            aud.PlayOneShot(ambience, musicVol);
        }
        else
            ambienceTimer = 55.5f;
*/

    }
}
