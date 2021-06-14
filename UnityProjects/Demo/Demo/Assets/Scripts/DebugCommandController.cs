using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandController : MonoBehaviour
{
    #region Singleton
    private static DebugCommandController instance;
    public static DebugCommandController Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of GameEvents found!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public void KillAll(){
        //EnemyManager.Instance.KillAll();
    }

    public void AddMoney(int amount){
        //player.GetComponent<CharacterInfo>().characterWallet += amount;
    }

    public void AddHealth(int amount){
        //player.GetComponent<CharacterInfo>().Heal(amount);
    }
}
