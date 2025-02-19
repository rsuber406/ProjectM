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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetVolume();

        masterVol = prevMasterVol = SFXVol = prevSFXVol = musicVol = prevMusicVol = GameManager.GetInstance().masterSlider.value;

    }

    // Update is called once per frame
    void Update()
    {

        musicVol = masterVol * GameManager.GetInstance().musicSlider.value;
        SFXVol = masterVol * GameManager.GetInstance().SFXSlider.value;

    }

    public void ToggleMasterVolume()
    {
        if (!masterVolMuted)
        {
            masterVolMuted = true;
            GameManager.GetInstance().masterSlider.value = 0f;
        }
        else
        {
            masterVolMuted = false;
            GameManager.GetInstance().masterSlider.value = prevMasterVol;
        }

    }

    public void GetMasterVolume()
    {
        masterVol = GameManager.GetInstance().masterSlider.value;
        prevMasterVol = masterVol;

        if (masterVol > 0)
        {
            masterVolMuted = false;
        }

        else
            masterVolMuted = true;
    }
    public void ToggleSFXVolume()
    {
        if (!SFXVolMuted)
        {
            SFXVolMuted = true;
            GameManager.GetInstance().SFXSlider.value = 0f;
        }
        else
        {
            SFXVolMuted = false;
            GameManager.GetInstance().SFXSlider.value = prevSFXVol;
        }
    }

    public void GetSFXVolume()
    {
        if (masterVol > 0f)
            SFXVol = masterVol * GameManager.GetInstance().SFXSlider.value;
            
        prevSFXVol = GameManager.GetInstance().SFXSlider.value;

        if (SFXVol > 0)
        {
            SFXVolMuted = false;
        }

        else
            SFXVolMuted = true;
    }
    public void ToggleMusicVolume()
    {
        if (!musicVolMuted)
        {
            musicVolMuted = true;
            GameManager.GetInstance().musicSlider.value = 0f;
        }
        else
        {
            musicVolMuted = false;
            GameManager.GetInstance().musicSlider.value = prevMusicVol;
        }
    }

    public void GetMusicVolume()
    {
        if (masterVol > 0f)
            musicVol = masterVol * GameManager.GetInstance().musicSlider.value;

        prevMusicVol = GameManager.GetInstance().musicSlider.value;

        if (musicVol > 0)
        {
            musicVolMuted = false;
        }

        else
            musicVolMuted = true;
    }

    private void ResetVolume()
    {
        GameManager.GetInstance().masterSlider.value = 1f;
        GameManager.GetInstance().SFXSlider.value = GameManager.GetInstance().musicSlider.value = GameManager.GetInstance().masterSlider.value;
    }

   
}
