using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CasualMatch", menuName = "ScriptableObjects/GameMatch/Casual", order = 2)]
public class CasualMatch : ScriptableObject
{
    public IAConfiguration IAConfiguration;
    public int CityBlocksX;
    public int CityBlocksZ;


    public void Reset()
    {
        IAConfiguration = null;
        CityBlocksX = 0;
        CityBlocksZ = 0;
    }
}
