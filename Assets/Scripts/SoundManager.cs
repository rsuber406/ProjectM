using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public float masterVol;
    public float SFXVol;
    public float musicVol;

    public float prevMasterVol;
    public float prevSFXVol;
    public float prevMusicVol;

    public bool masterVolMuted;
    public bool SFXVolMuted;
    public bool musicVolMuted;

    public bool toggleMasterVol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        masterVol = prevMasterVol = GameManager.GetInstance().masterSlider.value;
        SFXVol = prevSFXVol = GameManager.GetInstance().masterSlider.value;
        musicVol = prevMusicVol = GameManager.GetInstance().masterSlider.value;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetInstance().GetSoundManager().masterVol > 0)
        {
            GameManager.GetInstance().GetSoundManager().SFXVol = GameManager.GetInstance().GetSoundManager().masterVol * GameManager.GetInstance().SFXSlider.value;
            GameManager.GetInstance().GetSoundManager().musicVol = GameManager.GetInstance().GetSoundManager().masterVol * GameManager.GetInstance().musicSlider.value;
        }

    }

    public void ToggleMasterVolume()
    {
        GetMasterVolume();

        if (!GameManager.GetInstance().GetSoundManager().masterVolMuted)
        {
            GameManager.GetInstance().GetSoundManager().masterVolMuted = true;
            GameManager.GetInstance().masterSlider.value = 0f;
        }
        else
        {
            GameManager.GetInstance().GetSoundManager().masterVolMuted = false;
            GameManager.GetInstance().masterSlider.value = prevMasterVol;
        }
    }

    public void GetMasterVolume()
    {
        masterVol = GameManager.GetInstance().masterSlider.value;

        if (masterVol > 0)
        {
            prevMasterVol = masterVol;
            GameManager.GetInstance().GetSoundManager().masterVolMuted = false;
        }

        else
            GameManager.GetInstance().GetSoundManager().masterVolMuted = true;
    }
    public void ToggleSFXVolume()
    {
        GetSFXVolume();

        if (!GameManager.GetInstance().GetSoundManager().SFXVolMuted)
        {
            GameManager.GetInstance().GetSoundManager().SFXVolMuted = true;
            GameManager.GetInstance().SFXSlider.value = 0f;
        }
        else
        {
            GameManager.GetInstance().GetSoundManager().SFXVolMuted = false;
            GameManager.GetInstance().SFXSlider.value = prevSFXVol;
        }
    }

    public void GetSFXVolume()
    {
        if (masterVol != 0f)
            SFXVol = masterVol * GameManager.GetInstance().SFXSlider.value;

        if (SFXVol > 0)
        {
            prevSFXVol = SFXVol;
            GameManager.GetInstance().GetSoundManager().SFXVolMuted = false;
        }

        else
            GameManager.GetInstance().GetSoundManager().SFXVolMuted = true;
    }
    public void ToggleMusicVolume()
    {
        GetMusicVolume();

        if (!GameManager.GetInstance().GetSoundManager().musicVolMuted)
        {
            GameManager.GetInstance().GetSoundManager().musicVolMuted = true;
            GameManager.GetInstance().musicSlider.value = 0f;
        }
        else
        {
            GameManager.GetInstance().GetSoundManager().musicVolMuted = false;
            GameManager.GetInstance().musicSlider.value = prevMusicVol;
        }
    }

    public void GetMusicVolume()
    {

        if (masterVol != 0f)
            musicVol = masterVol * GameManager.GetInstance().musicSlider.value;

        if (musicVol > 0)
        {
            prevMusicVol = musicVol;
            GameManager.GetInstance().GetSoundManager().musicVolMuted = false;
        }

        else
            GameManager.GetInstance().GetSoundManager().musicVolMuted = true;
    }
}
