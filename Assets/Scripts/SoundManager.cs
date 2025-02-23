using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework.Constraints;

public class SoundManager : MonoBehaviour
{
    public PlayerController player;
    private GameMode mode;

    // this is the audio source order related to the components in GameManager
    public AudioSource audSFX;
    public AudioSource audMusic;
    public AudioSource audAmbience;

    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider ambienceSlider;
    public Slider musicSlider;

    public TMP_Text masterVolumeText;
    public TMP_Text SFXVolumeText;
    public TMP_Text ambienceVolumeText;
    public TMP_Text musicVolumeText;

    public float masterVol;
    public float SFXVol;
    public float ambienceVol;
    public float musicVol;


    [Header ("----- Player Sounds -----")]
    [SerializeField] AudioClip[] playerFootsteps;
    public bool isPlayingSteps;
    [SerializeField] AudioClip[] playerHurtSounds;
    [SerializeField] AudioClip[] playerDeathSounds;
    [SerializeField] AudioClip[] playerDodgeSounds;

    [Header("----- UI Sounds -----")]
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip[] menuSelect;    

    [Header("----- In Game Sounds -----")]
    [SerializeField] AudioClip ambience;
    [SerializeField] AudioClip pickup;

    [Header("-----  Music -----")]
    [SerializeField] AudioClip titleMusic;
    [SerializeField] AudioClip tutorialMusic;
    [SerializeField] AudioClip hubMusic;
    [SerializeField] AudioClip dungeonMusic;
    [SerializeField] AudioClip bossMusic;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetVolume();
        player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerController>();
        mode = GameManager.GetInstance().GetGameMode();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVolumes();

        if (!GameManager.GetInstance().enableDebug)
        {
            GetGameModeMusic();
            PlayAmbience();
        }

    }
    private void ResetVolume()
    {
        SFXSlider.value = ambienceSlider.value = musicSlider.value = masterSlider.value = 1f;
        audSFX.volume = audAmbience.volume = audMusic.volume = 0.5f;
        masterVolumeText.text = SFXVolumeText.text = ambienceVolumeText.text = musicVolumeText.text = (100f).ToString("F0");
    }

    public void UpdateVolumes()
    {
        SFXVol = masterVol * audSFX.volume;
        musicVol = masterVol * audMusic.volume;
        ambienceVol = masterVol * audAmbience.volume;
    }

    public void GetMasterVolume()
    {
        masterVol = masterSlider.value;
        masterVolumeText.text = (masterSlider.value * 100f).ToString("F0");
    }

    public void GetSFXVolume()
    {
        audSFX.volume = SFXSlider.value / 2;
        SFXVolumeText.text = (SFXSlider.value * 100f).ToString("F0");
    }

    public void GetAmbienceVolume()
    {
        audAmbience.volume = ambienceSlider.value / 2;
        ambienceVolumeText.text = (ambienceSlider.value * 100f).ToString("F0");
    }

    public void GetMusicVolume()
    {
        audMusic.volume = musicSlider.value / 2;
        musicVolumeText.text = (musicSlider.value * 100f).ToString("F0");
    }

    public IEnumerator PlayerSteps()
    {
        isPlayingSteps = true;
        audSFX.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)], SFXVol);
        
        if (!player.inCombat)
            yield return new WaitForSeconds(0.4f);
        else
            yield return new WaitForSeconds(0.35f);

        isPlayingSteps = false;
    }

    public void PlayerHurt()
    {
        audSFX.PlayOneShot(playerHurtSounds[Random.Range(0, playerHurtSounds.Length)], SFXVol);
    }

    public void PlayerDeath()
    {
        audSFX.PlayOneShot(playerDeathSounds[Random.Range(0, playerDeathSounds.Length)], SFXVol);
    }

    public void PlayerDodge()
    {
        audSFX.PlayOneShot(playerDodgeSounds[Random.Range(0, playerDodgeSounds.Length)], SFXVol);
    }

    public void LossJingle()
    {
        audMusic.PlayOneShot(lose, musicVol);
    }

    public void Pickup()
    {
        audSFX.PlayOneShot(pickup, SFXVol);
    }

    public void MenuClick(int index)
    {
        // 0 is regualr select, 1 is backwards select, 2 is hover noise
        audSFX.PlayOneShot(menuSelect[index], SFXVol);
    }

    public void Ambience()
    {
        if (!audAmbience.isPlaying)
            audAmbience.PlayOneShot(ambience, ambienceVol);
    }

    public void PlayAmbience()
    {
        if (mode != GameMode.MainMenu && mode != GameMode.Hub)
            Ambience();
    }

    public void GetGameModeMusic()
    {
        if (mode != GameManager.GetInstance().GetGameMode())
        {
            audMusic.Stop();
            mode = GameManager.GetInstance().GetGameMode();
        }

        MusicSelect();
    }

    public void MusicSelect()
    {
        switch (mode)
        {
            case GameMode.MainMenu:

                TitleMusic();
                
                break;
            case GameMode.Tutorial:

                TutorialMusic();

                break;
            case GameMode.Hub:
                
                HubMusic();
                
                break;
            case GameMode.Dungeon:

                DungeonMusic();
                
                break;
            case GameMode.Boss:

                BossMusic();
                
                break;
        }
    }

    public void TitleMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(titleMusic, musicVol);
    }
    public void TutorialMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(tutorialMusic, musicVol);
    }
    public void HubMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(hubMusic, musicVol);
    }
    public void DungeonMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(dungeonMusic, musicVol);
    }
    public void BossMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(bossMusic, musicVol);
    }

}
