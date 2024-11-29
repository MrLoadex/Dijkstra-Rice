using UnityEngine.UI;
using UnityEngine;

public class PlayerMobileController : Controller
{

    bool turnLeft = false;
    bool turnRight = false;
    bool accelerate = false;

    public PlayerMobileController() {
        UIManager.TurnLeftActionEvent += SetTurnLeft;
        UIManager.TurnRightActionEvent += SetTurnRight;
    }

    ~PlayerMobileController() {
        UIManager.TurnLeftActionEvent -= SetTurnLeft;
        UIManager.TurnRightActionEvent -= SetTurnRight;
    }

    private void SetTurnLeft(bool turnLeft)
    {
        turnRight = false;
        accelerate = false;
        this.turnLeft = turnLeft;
    }
    private void SetTurnRight(bool turnRight)
    {
        turnLeft = false;
        accelerate = false;
        this.turnRight = turnRight;
    }
    public override bool TurnLeft()
    {
        return turnLeft;
    }
    public override bool TurnRight()
    {
        return turnRight;
    }
    public override bool Accelerate()
    {
        return accelerate;
    }
}
