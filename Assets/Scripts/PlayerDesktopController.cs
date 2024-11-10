using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDesktopController : Controller
{
    public override bool TurnLeft()
    {
        return Input.GetKey(KeyCode.A);
    }
    public override bool TurnRight()
    {
        return Input.GetKey(KeyCode.D);
    }
    public override bool Accelerate()
    {
        return Input.GetKey(KeyCode.W);
    }
}
