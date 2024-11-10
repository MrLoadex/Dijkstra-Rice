using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour //nodo
{
    public Action<CarType> targetCompletedEvent;
    
    [SerializeField] private bool isUserActive = false;
    [SerializeField] private bool isIAActive = false;
    public bool IsUserActive => isUserActive;
    public bool IsIAActive => isIAActive;
    void Start()
    {
        if (IsUserActive)
        {
            GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f); // Verde transparente
        }
        else if (IsIAActive)
        {
            GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f); // Rojo transparente
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Complete(CarType.User);
        }
        else if (other.CompareTag("IACar"))
        {
            Complete(CarType.IA);
        }
    }

    public void Activate()
    {
        isUserActive = true;
        isIAActive = true;
        GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f); // Verde transparente
        GetComponent<Renderer>().enabled = true;
    }

    void Deactivate(CarType targetType)
    {
        if (targetType == CarType.User)
        {
            isUserActive = false;
            GetComponent<Renderer>().enabled = false;
        }
        else if (targetType == CarType.IA)
        {
            isIAActive = false;
        }
    }

    void Complete(CarType targetType)
    {
        Deactivate(targetType);
        targetCompletedEvent?.Invoke(targetType);
    }
}
