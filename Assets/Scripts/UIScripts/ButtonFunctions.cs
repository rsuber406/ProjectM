using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] GameObject hoverImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Resume()
    {
        GameManager.GetInstance().ResumeGame();
        GameManager.GetInstance().GetSoundManager().MenuClick();
    }

    public void Settings()
    {
        GameManager.GetInstance().SettingsMenu();
        GameManager.GetInstance().GetSoundManager().MenuClick();
    }
    public void Back()
    {
        GameManager.GetInstance().PauseMenu();
        GameManager.GetInstance().GetSoundManager().MenuClick();
    }

    public void TabAudio()
    {
        GameManager.GetInstance().AudioTab();
        GameManager.GetInstance().GetSoundManager().MenuClick();
    }

    public void TabControls()
    {
        GameManager.GetInstance().ControlsTab();
        GameManager.GetInstance().GetSoundManager().MenuClick();
    }

    public void TabGraphics()
    {
        GameManager.GetInstance().GraphicsTab();
        GameManager.GetInstance().GetSoundManager().MenuClick();
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

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
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

}
