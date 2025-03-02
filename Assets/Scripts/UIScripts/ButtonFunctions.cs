using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] GameObject hoverImage;

    public bool tabPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Resume()
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(1);
        GameManager.GetInstance().ResumeGame();
    }

    public void Settings()
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        GameManager.GetInstance().SettingsMenu();
    }
    public void Back()
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(1);
        GameManager.GetInstance().PauseMenu();
    }

    public void TabAudio()
    {
        tabPressed = true;
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        GameManager.GetInstance().GetAudioTab();
    }

    public void TabControls()
    {
        tabPressed = true;
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        GameManager.GetInstance().GetControlsTab();
    }

    public void TabGraphics()
    {
        tabPressed = true;
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        GameManager.GetInstance().GetGraphicsTab();
    }

    public void VolumeMaster()
    {
        GameManager.GetInstance().GetSoundManager().GetMasterVolume();
    }

    public void VolumeSFX()
    {
        GameManager.GetInstance().GetSoundManager().GetSFXVolume();
    }

    public void VolumeAmbience()
    {
        GameManager.GetInstance().GetSoundManager().GetAmbienceVolume();
    }

    public void VolumeMusic()
    {
        GameManager.GetInstance().GetSoundManager().GetMusicVolume();
    }

    public void Restart()
    {
        GameManager.GetInstance().Respawn();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(2);
        hoverImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabPressed = false;
        hoverImage.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (tabPressed)
            return;

        hoverImage.SetActive(false);
    }
 
    public void Exit()
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(1);
        GameManager.GetInstance().SavePlayerData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }

}
