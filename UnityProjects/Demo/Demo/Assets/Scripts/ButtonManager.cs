using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void Restart()
    {
        gm.Restart();
    }

    public void Resume()
    {
        gm.CloseMenu();
    }

    public void EnablePlayerControlsUI()
    {
        
    }
}
