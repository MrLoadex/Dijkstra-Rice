using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Straight,
    Right
}

public class IAController : Controller
{
    private Corner currentCorner;

    private IAModel iaModel;
    private List<Corner> path;

    public IAController(Corner[,] corners, Corner cornerTarget, IAConfiguration iaConfiguration)
    {
        iaModel = new IAModel(corners, cornerTarget, iaConfiguration);
        path = iaModel.SearchShortestPath();
        if (path == null || path.Count == 0)
        {
            Debug.LogError("No se encontró un camino.");
        }
        IACar.EnterCornerEvent += OnEnterCorner;
        IACar.ExitCornerEvent += OnExitCorner;
    }

    private void OnEnterCorner(Controller controller, Corner corner)
    {
        if (controller != this || corner == currentCorner)
        {
            return;
        }
        if (path.Count > 0)
        {
            path.RemoveAt(0);
            if (path.Count > 0)
            {
                currentCorner = corner;
                NextCornerEvent?.Invoke(path[0], corner.GetWeight(path[0]));
            }
        }
    }

    private void OnExitCorner(Controller controller)
    {
        if (controller != this)
        {
            return;
        }
        currentCorner = null;
    }

    #region Controller

    public override bool TurnLeft()
    {
        // Implementar lógica para girar a la izquierda si es necesario
        return false; // Esta lógica ya no es necesaria con el nuevo enfoque
    }

    public override bool TurnRight()
    {
        // Implementar lógica para girar a la derecha si es necesario
        return false; // Esta lógica ya no es necesaria con el nuevo enfoque
    }

    public override bool Accelerate()
    {
        // Implementar lógica para acelerar si es necesario
        return false; // Esta lógica ya no es necesaria con el nuevo enfoque
    }
    #endregion

    ~IAController() {
        IACar.EnterCornerEvent -= OnEnterCorner;
        IACar.ExitCornerEvent -= OnExitCorner;
    }
}

