using UnityEngine;
using System;

public class PlayerCar : Car
{

    void Start()
    {
        // Detectar el tipo de controlador según el dispositivo
        if (GameManager.Instance.IsMobile)
        {
            controller = new MobileController();
        }
        else
        {
            controller = new PlayerDesktopController();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (actualCorner == null) return;
        
        Corner posibleCorner = null;
        int weight = 0;
        if (controller.TurnRight())
        {
            (posibleCorner, weight) = actualCorner.GetRightCorner(previousCorner);
        }
        else if (controller.TurnLeft())
        {
            (posibleCorner, weight) = actualCorner.GetLeftCorner(previousCorner);
        }
        else if (controller.Accelerate())
        {
            (posibleCorner, weight) = actualCorner.GetStraightCorner(previousCorner);
        }

        if (posibleCorner != null)
        {
            releaseCurrentNode();
            nextCorner = posibleCorner;
            setVelocity(weight);
        }
        else if (Vector3.Distance(actualCorner.transform.position, this.transform.position) < 0.1)
        {
            if (nextCorner == actualCorner || nextCorner == null)
            {
                setNextAviableCorner();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Corner")
        {
            Corner corner = other.gameObject.GetComponent<Corner>();
            actualCorner = corner;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Corner")
        {
            actualCorner = null;
        }
    }

    void setNextAviableCorner()
    {
        int weight = 0;
        if (actualCorner == null)
        {
            Debug.Log("El coche no está en el nodo actual");
            return;
        }

        (nextCorner, weight) = actualCorner.GetViableCorner(previousCorner);
        setVelocity(weight);
        releaseCurrentNode();
    }

    void releaseCurrentNode()
    {
        previousCorner = actualCorner;
        actualCorner = null;
    }

}
