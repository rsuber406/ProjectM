using UnityEngine;
using System.Collections.Generic;
public class PlayerInteract : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string interactText;
    private List<Color> originalColors = new List<Color>();

    void Start()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    originalColors.Add(renderer.material.color);
                }
            }
        }
    }

    // Update is called once per frame
   private void OnTriggerEnter(Collider collider)
   {
       if (collider.isTrigger) return;
       Interact itemInteractable = collider.gameObject.GetComponent<Interact>();
       if (itemInteractable != null)
       {
           GameManager.GetInstance().interactText.text = interactText;
           Debug.Log(GameManager.GetInstance().interactText.text);
           GameManager.GetInstance().PlayerInteractShow();
           
       }
   }

   private void OnTriggerExit(Collider collider)
   {
       if (collider.isTrigger) return;
       Interact itemInteractable = collider.gameObject.GetComponent<Interact>();
       if (itemInteractable != null)
       {
           GameManager.GetInstance().PlayerInteractHide();
       }
   }

    
}
