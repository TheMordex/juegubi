using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICommand
{
    void Execute();
    void Undo(); 
}