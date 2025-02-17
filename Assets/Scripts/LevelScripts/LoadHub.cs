using UnityEngine;
public class loadHub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Move Player to 0, 0, -32.20
            MainSceneLogic.MSInstance.mapnum = 0;
            //GameManager.instance.manaBar;
            GameManager.instance.TeleportPlayer(0f, 0f, -32.20f);
        }
    }
}