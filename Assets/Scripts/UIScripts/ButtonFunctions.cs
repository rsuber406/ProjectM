using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] GameObject hoverImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Resume()
    {
        GameManager.GetInstance().ResumeGame();
    }

    public void Settings()
    {
        GameManager.GetInstance().SettingsMenu();
    }
    public void Back()
    {
        GameManager.GetInstance().PauseMenu();
    }

    public void TabAudio()
    {
        GameManager.GetInstance().AudioTab();
    }

    public void VolumeMaster()
    {
        GameManager.GetInstance().GetSoundManager().GetMasterVolume();
    }


    public void VolumeSFX()
    {
        GameManager.GetInstance().GetSoundManager().GetSFXVolume();
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

    public void Exit()
    {
        GameManager.GetInstance().SavePlayerData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }
}
