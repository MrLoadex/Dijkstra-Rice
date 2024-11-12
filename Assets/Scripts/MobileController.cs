using UnityEngine.UI;
using UnityEngine;

public class MobileController : Controller
{

    bool turnLeft = false;
    bool turnRight = false;
    bool accelerate = false;

    public MobileController() {
        UIManager.TurnLeftActionEvent += SetTurnLeft;
        UIManager.TurnRightActionEvent += SetTurnRight;
        UIManager.AccelerateActionEvent += SetAccelerate;
    }

    ~MobileController() {
        UIManager.TurnLeftActionEvent -= SetTurnLeft;
        UIManager.TurnRightActionEvent -= SetTurnRight;
        UIManager.AccelerateActionEvent -= SetAccelerate;
    }

    private void SetTurnLeft()
    {
        turnRight = false;
        accelerate = false;
        turnLeft = true;
    }
    private void SetTurnRight()
    {
        turnLeft = false;
        accelerate = false;
        turnRight = true;
    }
    private void SetAccelerate()
    {
        turnLeft = false;
        turnRight = false;
        accelerate = true;
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
