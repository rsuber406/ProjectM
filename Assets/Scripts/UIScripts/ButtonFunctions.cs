using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{

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

    public void ToggleMasterVolume()
    {
        GameManager.GetInstance().GetSoundManager().ToggleMasterVolume();
    }

    public void MasterVolume()
    {
        GameManager.GetInstance().GetSoundManager().GetMasterVolume();
    }

    public void ToggleSFXVolume()
    {
        GameManager.GetInstance().GetSoundManager().ToggleSFXVolume();
    }

    public void SFXVolume()
    {
        GameManager.GetInstance().GetSoundManager().GetSFXVolume();
    }
    public void ToggleMusicVolume()
    {
        GameManager.GetInstance().GetSoundManager().ToggleMusicVolume();
    }

    public void MusicVolume()
    {
        GameManager.GetInstance().GetSoundManager().GetMusicVolume();
    }
    public void Restart()
    {
        // Talk to game manager to teleport player to where they need to go
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }

 
    
}
