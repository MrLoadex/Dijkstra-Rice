using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameMatch", menuName = "ScriptableObjects/GameMatch", order = 1)]
public class GameMatch : ScriptableObject
{
    public bool IsMobile; // asquerosidad, pero no tengo tiempo para arreglarlo
    [SerializeField] private Mission[] missions;
    public int Score;
    public Mission LastMission;
    public bool Continue;

    public Mission[] Missions => missions;
}
