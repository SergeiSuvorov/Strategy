using Abstractions.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandsQueue
{
    void EnqueueCommand(object command);
    ICommand CurrentCommand { get; }
    void Clear();
}
