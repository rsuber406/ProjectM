using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private AudioSource aud;

    public TMP_Text masterVolumeText;
    public TMP_Text SFXVolumeText;
    public TMP_Text musicVolumeText;

    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider musicSlider;

    public float masterVol;
    public float SFXVol;
    public float musicVol;


    [Header ("----- Player Sounds -----")]
    [SerializeField] AudioClip[] playerFootsteps;


    public bool isPlayingSteps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aud = GetComponent<AudioSource>();
        ResetVolume();
        masterVol = SFXVol = musicVol = masterSlider.value;

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

    public IEnumerator PlaySteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)], SFXVol);
        
        yield return new WaitForSeconds(0.5f);

        isPlayingSteps = false;
    }

}
