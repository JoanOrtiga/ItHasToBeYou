using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleSolver
{
    /// <summary>
    /// Is puzzle solved?
    /// </summary>
    /// <returns></returns>
    bool Solved();
}
