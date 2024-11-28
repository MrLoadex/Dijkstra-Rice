using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CasualMatch", menuName = "ScriptableObjects/GameMatch", order = 1)]
public class CasualMatch : ScriptableObject
{
    [SerializeField] private IAConfiguration iaConfiguration;
    [SerializeField] private int cityBlocksX;
    [SerializeField] private int cityBlocksZ;

    public IAConfiguration IAConfiguration => iaConfiguration;
    public int CityBlocksX => cityBlocksX;
    public int CityBlocksZ => cityBlocksZ;
}
