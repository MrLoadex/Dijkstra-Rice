using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalMatch", menuName = "ScriptableObjects/GameMatch/Normal", order = 1)]
public class NormalMatch : ScriptableObject
{
    [SerializeField] private Mission[] missions;
    public int Score;
    public Mission LastMission;
    public bool Continue;

    public Mission[] Missions => missions;

    public void Reset()
    {
        Score = 0;
        LastMission = null;
        Continue = false;
    }
}
