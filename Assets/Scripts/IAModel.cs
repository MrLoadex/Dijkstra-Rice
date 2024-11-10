using System.Collections.Generic;
using UnityEngine;

class IAModel
{
    Dijkstra dijkstra;
    Corner cornerTarget;
    Corner[,] corners;
    IAConfiguration iaConfiguration;

    public IAModel(Corner[,] corners, Corner cornerTarget, IAConfiguration iaConfiguration)
    {
        this.cornerTarget = cornerTarget;
        this.corners = corners;
        this.iaConfiguration = iaConfiguration;
        dijkstra = new Dijkstra();
    }

    public List<Corner> SearchShortestPath()
    {
        if (corners == null || cornerTarget == null)
        {
            Debug.LogError("No se han configurado los nodos");
            return null;
        }
        return dijkstra.FindShortestPath(corners[1, 0], cornerTarget, corners, iaConfiguration.ErrorPercentage);
    }
}