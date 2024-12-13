using System.Collections.Generic;
using UnityEngine;
using System;

public class CityBuilder : MonoBehaviour
{
    public static Action CityBuiltEvent;

    [Header("Prefabs")]
    [SerializeField] private GameObject cornerPrefab;
    [SerializeField] private GameObject streetPrefab; // Prefab de la calle
    [SerializeField] private List<GameObject> buildingPrefabs; // Lista de prefabs de edificios

    [Header("Variables")]
    [SerializeField] private int blocksX = 5;
    [SerializeField] private int blocksZ = 5;

    //randomizar los pesos de las calles
    [SerializeField] private int minWeight = 1;
    [SerializeField] private int maxWeight = 5;

    
    private Corner[,] corners;
    private Target[,] targets;
    
    public Corner[,] Corners => corners;
    public Target[,] Targets => targets;
    public int CityBlocks => blocksX + blocksZ;
    public Vector3 CityCenter => corners[blocksX / 2, blocksZ / 2].transform.position;
    public void BuildCity(int x, int z)
    {
        blocksX = x;
        blocksZ = z;
        InitializeCorners();
        ConnectCorners();
        InstantiateStreets();
        InstantiateBuildings();
        CityBuiltEvent?.Invoke();
    }

    void InitializeCorners()
    {
        // Initialize the corner matrix
        corners = new Corner[blocksX + 1, blocksZ + 1];
        targets = new Target[blocksX + 1, blocksZ + 1];

        // Instantiate corners in a grid
        for (int x = 0; x <= blocksX; x++)
        {
            for (int z = 0; z <= blocksZ; z++)
            {
                Vector3 position = new Vector3(x * 10, 0, z * 10);
                GameObject cornerObj = Instantiate(cornerPrefab, position, Quaternion.identity);
                cornerObj.name = $"Corner_{x}_{z}"; // Asignar nombre personalizado
                corners[x, z] = cornerObj.GetComponent<Corner>();
                targets[x, z] = corners[x, z].GetTarget();
            }
        }
    }

    void ConnectCorners()
    {
        // Connect the corners
        for (int x = 0; x <= blocksX; x++)
        {
            for (int z = 0; z <= blocksZ; z++)
            {
                Corner current = corners[x, z];

                // Connect to the positive X corner
                if (x < blocksX)
                {
                    current.cornerXPos = corners[x + 1, z];
                    current.cornerXPos.WeightXPos = UnityEngine.Random.Range(minWeight, maxWeight);
                    current.cornerXPos.EstablishWeights();
                }

                // Connect to the negative X corner
                if (x > 0)
                {
                    current.cornerXNeg = corners[x - 1, z];
                    current.cornerXNeg.WeightXNeg = UnityEngine.Random.Range(minWeight, maxWeight);
                    current.cornerXNeg.EstablishWeights();
                }

                // Connect to the positive Z corner
                if (z < blocksZ)
                {
                    current.cornerZPos = corners[x, z + 1];
                    current.cornerZPos.WeightZPos = UnityEngine.Random.Range(minWeight, maxWeight);
                    current.cornerZPos.EstablishWeights();
                }

                // Connect to the negative Z corner
                if (z > 0)
                {
                    current.cornerZNeg = corners[x, z - 1];
                    current.cornerZNeg.WeightZNeg = UnityEngine.Random.Range(minWeight, maxWeight);
                    current.cornerZNeg.EstablishWeights();
                }
            }
        }
    }

    void InstantiateStreets()
    {
        // Instantiate streets between corners in the X direction
        for (int x = 0; x < blocksX; x++)
        {
            for (int z = 0; z <= blocksZ; z++)
            {
                Vector3 start = corners[x, z].transform.position;
                Vector3 end = corners[x + 1, z].transform.position;
                Vector3 midpoint = (start + end) / 2;
                Quaternion rotation = Quaternion.LookRotation(end - start);

                // Instantiate the street
                GameObject streetObj = Instantiate(streetPrefab, midpoint, rotation);
                streetObj.name = $"Street_X_{x}_{z}"; // Asignar nombre personalizado
                Street street = streetObj.GetComponent<Street>();
                street.Weight = corners[x, z].WeightXPos + corners[x + 1, z].WeightXNeg;
                street.UpdateValue();
            }
        }

        // Instantiate streets between corners in the Z direction
        for (int x = 0; x <= blocksX; x++)
        {
            for (int z = 0; z < blocksZ; z++)
            {
                Vector3 start = corners[x, z].transform.position;
                Vector3 end = corners[x, z + 1].transform.position;
                Vector3 midpoint = (start + end) / 2;
                Quaternion rotation = Quaternion.LookRotation(end - start);

                // Instantiate the street
                GameObject streetObj = Instantiate(streetPrefab, midpoint, rotation);
                streetObj.name = $"Street_Z_{x}_{z}"; // Asignar nombre personalizado
                Street street = streetObj.GetComponent<Street>();
                street.Weight = corners[x, z].WeightZPos + corners[x, z + 1].WeightZNeg;
                street.UpdateValue();
            }
        }
    }

    void InstantiateBuildings()
    {
        // Iterate over each block to instantiate at least one building
        for (int x = 0; x < blocksX; x++)
        {
            for (int z = 0; z < blocksZ; z++)
            {
                // Calculate the center of the block
                Vector3 cornerA = corners[x, z].transform.position;
                Vector3 cornerB = corners[x + 1, z].transform.position;
                Vector3 cornerC = corners[x, z + 1].transform.position;
                Vector3 cornerD = corners[x + 1, z + 1].transform.position;

                Vector3 center = (cornerA + cornerB + cornerC + cornerD) / 4 + Vector3.up * 0.5f; // Adjust height according to prefab

                // Randomly select a building prefab from the list
                if (buildingPrefabs.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, buildingPrefabs.Count);
                    GameObject selectedBuilding = buildingPrefabs[randomIndex];
                    GameObject buildingObj = Instantiate(selectedBuilding, center, Quaternion.identity);
                    buildingObj.name = $"Building_{x}_{z}"; // Asignar nombre personalizado
                }
                else
                {
                    Debug.LogWarning("The list of building prefabs is empty.");
                }
            }
        }
    }

    public Target getTarget(int x, int z)
    {
        return targets[x, z];
    }
}
