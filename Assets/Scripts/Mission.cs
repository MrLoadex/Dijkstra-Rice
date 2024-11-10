using UnityEngine;
using System.Collections;
using System;

public enum CarType
{
    User,
    IA
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject
{
    public Action<CarType> MissionCompletedEvent;

    [SerializeField] private bool activeUserTarget;
    [SerializeField] private bool activeIATarget;

    [SerializeField] private bool randomizeTargets;

    private bool completedUserTarget;
    private bool completedIATarget;
    private bool isCompleted;
    private Target[,] targets;
    private Corner targetCorner;

    public bool ActiveUserTarget => activeUserTarget;
    public bool ActiveIATarget => activeIATarget;
    public bool CompletedUserTarget => completedUserTarget;
    public bool CompletedIATarget => completedIATarget;
    public bool IsCompleted => isCompleted;

    public void StartMission(Target[,] targets, Corner[,] corners)
    {
        if (targets == null || corners == null)
        {
            Debug.LogError("Targets o Corners no están inicializados.");
            return;
        }

        this.targets = targets;
        completedUserTarget = false;
        completedIATarget = false;
        if (randomizeTargets)
        {
            ActiveRandomTarget(corners);
        }
        else
        {
            ActiveLastTarget(corners);
        }
    }

    void ActiveRandomTarget(Corner[,] corners)
    {
        int randomX = UnityEngine.Random.Range(0, targets.GetLength(0));
        int randomY = UnityEngine.Random.Range(0, targets.GetLength(1));
        targets[randomX, randomY].Activate();
        targets[randomX, randomY].targetCompletedEvent += OnTargetCompleted;
        targetCorner = corners[randomX, randomY];
        
        activeUserTarget = true;
        activeIATarget = true;
    }

    void ActiveLastTarget(Corner[,] corners)
    {
        targets[targets.GetLength(0) - 1, targets.GetLength(1) - 1].Activate();
        targets[targets.GetLength(0) - 1, targets.GetLength(1) - 1].targetCompletedEvent += OnTargetCompleted;
        targetCorner = corners[corners.GetLength(0) - 1, corners.GetLength(1) - 1];
        
        activeUserTarget = true;
        activeIATarget = true;
    }

    public Target[,] GetTargets()
    {
        return targets;
    }

    void OnTargetCompleted(CarType targetType)
    {
        if (targetType == CarType.User)
        {
            completedUserTarget = true;
            MissionCompletedEvent?.Invoke(CarType.User);
        }
        else
        {
            completedIATarget = true;
            MissionCompletedEvent?.Invoke(CarType.IA);
        }
        if (completedUserTarget && completedIATarget)
        {
            isCompleted = true;
        }
    }

    public Corner GetTargetCorner()
    {
        return targetCorner;
    }
}
