using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerInteract : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string interactText;
    private List<Color> originalColors = new List<Color>();
    private bool objectEntered = false;

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
            if (!objectEntered)
            {
                ChangeColor();
                objectEntered = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.isTrigger) return;
        Interact itemInteractable = collider.gameObject.GetComponent<Interact>();
        if (itemInteractable != null)
        {
            GameManager.GetInstance().PlayerInteractHide();
            ChangeBackToOriginalColor();
        }
    }


    private void ChangeColor()
    {
        Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)
        {
            foreach (Material mat in render.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = Color.green;
                }
            }
        }

        objectEntered = false;
    }

    private void ChangeBackToOriginalColor()
    {
        Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        int counter = 0;
        foreach (Renderer render in renderers)
        {
            foreach (Material mat in render.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = originalColors[counter];
                }
            }

            counter++;
        }
    }
}