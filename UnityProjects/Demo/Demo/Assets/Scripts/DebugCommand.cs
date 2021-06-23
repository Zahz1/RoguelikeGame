using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    
    public string commandId { get; private set; }
    public string commandDescription { get; private set; }
    public string commandFormat { get; private set; }

    public DebugCommandBase(string id, string description, string format){
        this.commandId = id;
        this.commandDescription = description;
        this.commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand(string id, string description, string format, Action command) : base (id, description, format)
    {
        this.command = command;
    }
    public void Invoke(){
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;
    
    public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}
