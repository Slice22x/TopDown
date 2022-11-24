using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string commandId;
    private string commandDesctiption;
    private string commandFormat;

    public string CommandID { get { return commandId; } }
    public string CommandDescription { get { return commandDesctiption; } }
    public string CommandFormat { get { return commandFormat; } }

    public DebugCommandBase(string ID, string Description, string Format)
    {
        commandId = ID;
        commandDesctiption = Description;
        commandFormat = Format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action Command;

    public DebugCommand(string ID, string Description, string Format, Action Command) : base(ID, Description, Format)
    {
        this.Command = Command;
    }

    public void Invoke()
    {
        Command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> Command;

    public DebugCommand(string ID, string Description, string Format, Action<T1> Command) : base(ID, Description, Format)
    {
        this.Command = Command;
    }

    public void Invoke(T1 value)
    {
        Command.Invoke(value);
    }
}
