using UnityEngine;
public class loadHub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //get rid of current map at 0,0,0. It will never be Hub
            MainSceneLogic.MSInstance.loadHub();
            //Move Player to 0, 0, -32.20
            //GameManager.instance.manaBar;
            GameManager.instance.TeleportPlayer(0f, 0f, -32.20f);
        }
    }
}