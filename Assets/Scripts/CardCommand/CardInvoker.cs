using System.Collections.Generic;
using UnityEngine;

public class CardInvoker
{
    public Queue<ICommand> historyQueue = new Queue<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        historyQueue.Enqueue(command);
    }

    //public static void UndoCommand()
    //{
    //    if (historyStack.Count > 0)
    //    {
    //        ICommand activeCommand = historyStack.Pop();
    //        activeCommand.Undo();
    //    }
    //}
}
