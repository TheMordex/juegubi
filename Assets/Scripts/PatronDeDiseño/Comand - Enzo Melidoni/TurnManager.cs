using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    private Queue<ICommand> commandQueue = new Queue<ICommand>();

    private bool isHeroTurn = true; 

    public bool IsHeroTurn()
    {
        return isHeroTurn;
    }

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

    public void NextTurn()
    {
        isHeroTurn = !isHeroTurn;
    }
}