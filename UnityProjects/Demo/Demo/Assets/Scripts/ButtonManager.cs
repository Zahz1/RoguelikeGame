using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public void Resume()
    {
        GameEvents.Instance.MenuTriggerExit();
        InputListener.Instance.menuOpen = false;
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    } 
}
