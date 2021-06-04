using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private static PauseController instance;
    public static PauseController Instance { get { return instance; } }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of PauseController found!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public bool GameIsPaused = false;
}
