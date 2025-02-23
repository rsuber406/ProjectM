using UnityEngine;
public class loadHub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainSceneLogic.MSInstance.loadScreen.SetActive(true);
            //Move Player to 0, 0, -32.20
            MainSceneLogic.MSInstance.mapnum = 0;
            GameManager.instance.resetPlayerStats();
            GameManager.GetInstance().SetGameMode(GameMode.Hub);
            GameManager.GetInstance().SetPlayerCompleteTutorial(true);
            GameManager.instance.TeleportPlayer(0f, 0f, -32.20f);
            MainSceneLogic.MSInstance.loadScreen.SetActive(false);
        }
    }
}