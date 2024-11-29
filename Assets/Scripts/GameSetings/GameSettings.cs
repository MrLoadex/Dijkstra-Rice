using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public bool FreeGame;
    public bool Mobile;


    public void Reset()
    {
        FreeGame = false;
        Mobile = false;
    }
}

