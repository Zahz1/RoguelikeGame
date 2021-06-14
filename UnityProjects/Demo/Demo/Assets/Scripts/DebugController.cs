using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private bool displayConsole = false;
    string input;

    public List<DebugCommand> commandList;
    public static DebugCommand KILL_ALL;
    public static DebugCommand ADD_MONEY;
    public static DebugCommand ADD_HEALTH;
    private void Awake(){

        KILL_ALL = new DebugCommand("kill_all", "Remove all entities frome the scene.", "kill_all", () => 
        {
            DebugCommandController.Instance.KillAll();
        });

        ADD_MONEY = new DebugCommand("add_money", "Add specified amount of money to player.", "add_money", () =>
        {
            DebugCommandController.Instance.AddMoney(100);
        });

        ADD_HEALTH = new DebugCommand("add_health", "Add specified amount of health to player", "add_health", () =>
        {
            DebugCommandController.Instance.AddHealth(100);
        });

        commandList = new List<DebugCommand>
        {
            KILL_ALL,
            ADD_MONEY,
            ADD_HEALTH
        };
    }

    private void Start(){
        GameEvents.Instance.OnDisplayDebugConsoleEnter += OnToggleDebug;
    }

    public void OnToggleDebug(){
        Debug.Log("Open/Close debug console");
        displayConsole = !displayConsole;
    }

    public void OnReturn(){
        HandleInput();
        input = "";
    }

    private void OnGUI(){
        if(!displayConsole){ return; }

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("InputField");

        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width-20f, 20f), input);
        GUI.FocusControl("InputField");
    }

    private void HandleInput(){
        
        for(int i = 0; i < commandList.Count; i++)
        {

            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.commandId))
            {

                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }
}
