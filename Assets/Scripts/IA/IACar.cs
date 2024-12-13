using UnityEngine;
using System;

public class IACar : Car
{
    public static Action<Controller, Corner> EnterCornerEvent;
    public static Action<Controller> ExitCornerEvent;
    public IAConfiguration iaConfiguration;

    // Update is called once per frame
    protected override void Update()
    {
        if (nextCorner == actualCorner) return;
        base.Update();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Corner")
        {
            Corner corner = other.gameObject.GetComponent<Corner>();
            actualCorner = corner;
            EnterCornerEvent?.Invoke(controller, corner);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Corner")
        {
            ExitCornerEvent?.Invoke(controller);
        }
    }

    void OnNextCorner(Corner corner, int weight)
    {
        nextCorner = corner;
        setVelocity(weight);
    }

    public void SetIAConfiguration(Corner[,] corners, Corner cornerTarget, IAConfiguration iaConfiguration)
    {
        this.iaConfiguration = iaConfiguration;
        maxVelocity = iaConfiguration.MaxVelocity;
        controller = new IAController(corners, cornerTarget, iaConfiguration);
        controller.NextCornerEvent += OnNextCorner;
    }

    #region EVENTS

    void OnDisable()
    {
        controller.NextCornerEvent -= OnNextCorner;
    }

    void OnDestroy() {
        controller.NextCornerEvent -= OnNextCorner;
    }
    #endregion
}
