using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField]
    private GameObject focus;
    [SerializeField]
    private InteractableType type;
    [SerializeField]
    private Interactable interactable;

    private void Update()
    {
        if(InputListener.Instance.GetInteract())
        {
            if(focus != null)
            {
                Interact();
            }
        }
    }

    public void SetFocus(GameObject interactable)
    {
        //Get GameObject of interactable, called in Interactable
        focus = interactable.gameObject;
        type = focus.GetComponent<Interactable>().GetInteractableType();
    }

    public void RemoveFocus()
    {
        if(focus != null)
        {
            focus.GetComponent<Outline>().enabled = false;
            focus = null;
        }
    }

    private void Interact()
    {
        switch (type) {
            case InteractableType.HealthShrine:
                interactable = focus.GetComponent<HealthShrineController>();
                interactable.Interact();
                interactable = null;
                break;
        }
        
    }

    public bool CheckFocus()
    {
        if(focus != null)
        {
            return true;
        }
        return false;
    }

    public GameObject GetFocus()
    {
        return focus;
    }
}
