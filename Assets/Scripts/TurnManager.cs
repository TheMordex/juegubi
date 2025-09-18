using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    private Queue<ICommand> commandQueue = new Queue<ICommand>();

    public void AddCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
    }

    public void ExecuteTurn()
    {
        while (commandQueue.Count > 0)
        {
            ICommand command = commandQueue.Dequeue();
            command.Execute();
        }
    }
}
