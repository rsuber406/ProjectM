using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public PlayerController player;
    private GameMode mode;

    public AudioSource audSFX;      // 1st audio source
    public AudioSource audMusic;    // 2nd audio source
    public AudioSource audAmbience; // 3rd audio source

    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider ambienceSlider;
    public Slider musicSlider;

    public TMP_Text masterVolumeText;
    public TMP_Text SFXVolumeText;
    public TMP_Text ambienceVolumeText;
    public TMP_Text musicVolumeText;

    private float masterVol;
    private float sfxVol;
    private float ambienceVol;
    private float musicVol; 

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

    private bool paused;
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
        UpdateAudioSources();

        if (!GameManager.GetInstance().enableDebug)
        {
            GetMusic();
            PlayAmbience();
        }
    }

    private void ResetVolume()
    {
        SFXSlider.value = ambienceSlider.value = musicSlider.value = masterSlider.value = 1f;
        audSFX.volume = audMusic.volume = 0.5f;
        audAmbience.volume = 0.75f;
        masterVolumeText.text = SFXVolumeText.text = ambienceVolumeText.text = musicVolumeText.text = (100f).ToString("F0");
    }

    public void UpdateAudioSources()
    {
        audSFX.volume = sfxVol;
        audAmbience.volume = ambienceVol;
        audMusic.volume = musicVol;
    }

    public void UpdateVolumes()
    {
        sfxVol = (SFXSlider.value / 1.33f) * masterVol;
        ambienceVol = (ambienceSlider.value / 1.33f) * masterVol;
        musicVol = (musicSlider.value / 2f) * masterVol;
    }

    public void GetMasterVolume()
    {
        masterVol = masterSlider.value;
        masterVolumeText.text = (masterSlider.value * 100f).ToString("F0");
        UpdateVolumes();
    }

    public void GetSFXVolume()
    {
        sfxVol = (SFXSlider.value / 1.33f) * masterVol;
        SFXVolumeText.text = (SFXSlider.value * 100f).ToString("F0");
    }

    public void GetAmbienceVolume()
    {
        ambienceVol = (ambienceSlider.value / 1.33f) * masterVol;
        ambienceVolumeText.text = (ambienceSlider.value * 100f).ToString("F0");
    }

    public void GetMusicVolume()
    {
        musicVol = (musicSlider.value / 2f) * masterVol;
        musicVolumeText.text = (musicSlider.value * 100f).ToString("F0");
    }

    public IEnumerator PlayerSteps()
    {
        isPlayingSteps = true;
        audSFX.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)], audSFX.volume);
        
        if (!player.inCombat)
            yield return new WaitForSeconds(0.4f);
        else
            yield return new WaitForSeconds(0.35f);

        isPlayingSteps = false;
    }

    public void PlayerHurt()
    {
        audSFX.PlayOneShot(playerHurtSounds[Random.Range(0, playerHurtSounds.Length)], audSFX.volume);
    }

    public void PlayerDeath()
    {
        audSFX.PlayOneShot(playerDeathSounds[Random.Range(0, playerDeathSounds.Length)], audSFX.volume);
    }

    public void PlayerDodge()
    {
        audSFX.PlayOneShot(playerDodgeSounds[Random.Range(0, playerDodgeSounds.Length)], audSFX.volume);
    }

    public void LossJingle()
    {
        audMusic.Stop();
        audMusic.PlayOneShot(lose, audMusic.volume);
    }

    public void Pickup()
    {
        audSFX.PlayOneShot(pickup, audSFX.volume);
    }

    public void MenuClick(int index)
    {
        // 0 is regualr select, 1 is backwards select, 2 is hover noise
        audSFX.PlayOneShot(menuSelect[index], audSFX.volume);
    }

    public void Ambience()
    {
        if (!audAmbience.isPlaying)
            audAmbience.PlayOneShot(ambience, audAmbience.volume);
    }

    public void PlayAmbience()
    {
        if (mode != GameMode.MainMenu && mode != GameMode.Hub)
            Ambience();
        else
            audAmbience.Stop();
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

    public void GetMusic()
    {
        if (GameManager.GetInstance().GetGameState() == GameState.Paused)
        {
            if (GameManager.GetInstance().GetGameMode() == GameMode.MainMenu)
            {
                GetGameModeMusic();
                return;
            }
            else
            {
                if (audMusic.isPlaying)
                {
                    audMusic.Pause();
                    paused = true;
                }
            }
        }
        else
        {
            if (!paused)
                GetGameModeMusic();

            else
            {
                audMusic.UnPause();
                paused = false;
            }
        }
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
            audMusic.PlayOneShot(titleMusic, audMusic.volume);
    }
    public void TutorialMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(tutorialMusic, audMusic.volume);
    }
    public void HubMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(hubMusic, audMusic.volume);
    }
    public void DungeonMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(dungeonMusic, audMusic.volume);
    }
    public void BossMusic()
    {
        if (!audMusic.isPlaying)
            audMusic.PlayOneShot(bossMusic, audMusic.volume);
    }

}
