using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour // Nodo
{
    //TODO: Al entrar en su collider, debe habilitar la opcion de doblar a la izquierda o a la derecha o continuar recto.
    [Header("Target")]
    [SerializeField] private Target target = null;
    
    [Header("Nodos adyacentes")]
    public Corner cornerXPos = null;
    public Corner cornerXNeg = null;
    public Corner cornerZPos = null;
    public Corner cornerZNeg = null;

    [Header("Pesos")]
    public int WeightXPos = 0; // Peso de la calle X positiva.
    public int WeightXNeg = 0; // Peso de la calle X negativa.
    public int WeightZPos = 0; // Peso de la calle Z positiva.
    public int WeightZNeg = 0; // Peso de la calle Z negativa.



    void Start()
    {
        EstablishWeights();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public (Corner, int) GetLeftCorner(Corner exCorner)
    {
        if (exCorner == cornerXPos)
        {
            return (cornerZNeg, WeightZNeg);
        }
        else if (exCorner == cornerXNeg)
        {
            return (cornerZPos, WeightZPos);
        }
        else if (exCorner == cornerZPos)
        {
            return (cornerXPos, WeightXPos);
        }
        else if (exCorner == cornerZNeg)
        {
            return (cornerXNeg, WeightXNeg);
        }
        return (null, 0);
    }

    public (Corner, int) GetStraightCorner(Corner exCorner)
    {
        if (exCorner == cornerXPos)
        {
            return (cornerXNeg, WeightXNeg);
        }
        else if (exCorner == cornerXNeg)
        {
            return (cornerXPos, WeightXPos);
        }
        else if (exCorner == cornerZPos)
        {
            return (cornerZNeg, WeightZNeg);
        }
        else if (exCorner == cornerZNeg)
        {
            return (cornerZPos, WeightZPos);
        }
        return (null, 0);
    }

    public (Corner, int) GetRightCorner(Corner previousCorner)
    {
        if (previousCorner == cornerXPos)
        {
            return (cornerZPos, WeightZPos);
        }
        else if (previousCorner == cornerXNeg)
        {
            return (cornerZNeg, WeightZNeg);
        }
        else if (previousCorner == cornerZPos)
        {
            return (cornerXNeg, WeightXNeg);
        }
        else if (previousCorner == cornerZNeg)
        {
            return (cornerXPos, WeightXPos);
        }
        return (null, 0);
    }

    public (Corner, int) GetViableCorner(Corner previousCorner)
    {
        int weight = 0;
        Corner returnCorner = null;
        (returnCorner, weight) = GetStraightCorner(previousCorner); // Seguir recto
        if (returnCorner != null)
        {
            return (returnCorner, weight);
        }
        (returnCorner, weight) = GetRightCorner(previousCorner); // Doblar a la derecha
        if (returnCorner != null)
        {
            return (returnCorner, weight);
        }
        (returnCorner, weight) = GetLeftCorner(previousCorner); // Doblar a la izquierda
        if (returnCorner != null)
        {
            return (returnCorner, weight);
        }
        Debug.Log("No hay nodo adyacente viable");
        return (null, 0);
    }

    public int GetWeight(Corner corner)
    {
        if (corner == cornerXPos)
        {
            return WeightXPos;
        }
        else if (corner == cornerXNeg)
        {
            return WeightXNeg;
        }
        else if (corner == cornerZPos)
        {
            return WeightZPos;
        }
        else if (corner == cornerZNeg)
        {
            return WeightZNeg;
        }
        return 0;
    }

    public void EstablishWeights()
    {
        if (cornerXPos != null)
        {
            if (cornerXPos.GetWeight(this) > WeightXPos)
            {
                WeightXPos = cornerXPos.GetWeight(this);
            }
        }
        if (cornerXNeg != null)
        {
            if (cornerXNeg.GetWeight(this) > WeightXNeg)
            {
                WeightXNeg = cornerXNeg.GetWeight(this);
            }
        }
        if (cornerZPos != null)
        {
            if (cornerZPos.GetWeight(this) > WeightZPos)
            {
                WeightZPos = cornerZPos.GetWeight(this);
            }
        }
        if (cornerZNeg != null)
        {
            if (cornerZNeg.GetWeight(this) > WeightZNeg)
            {
                WeightZNeg = cornerZNeg.GetWeight(this);
            }
        }
    }

    public Target GetTarget()
    {
        return target;
    }

    public Corner[] GetNeighbors()
    {
        List<Corner> neighbors = new List<Corner>();
        if (cornerXPos != null) neighbors.Add(cornerXPos);
        if (cornerXNeg != null) neighbors.Add(cornerXNeg);
        if (cornerZPos != null) neighbors.Add(cornerZPos);
        if (cornerZNeg != null) neighbors.Add(cornerZNeg);
        return neighbors.ToArray();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Corner other = (Corner)obj;
        return gameObject.name.Equals(other.gameObject.name);
    }

    public override int GetHashCode()
    {
        return gameObject.name.GetHashCode();
    }
}
