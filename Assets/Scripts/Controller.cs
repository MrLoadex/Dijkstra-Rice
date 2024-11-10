using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Controller
{
    public Action<Corner, int> NextCornerEvent; // Corner, weight
    public abstract bool TurnLeft();
    public abstract bool TurnRight();
    public abstract bool Accelerate();
}
