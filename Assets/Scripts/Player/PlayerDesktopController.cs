using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDesktopController : Controller
{
    public override bool TurnLeft()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }
    public override bool TurnRight()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }
    public override bool Accelerate()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }
}
