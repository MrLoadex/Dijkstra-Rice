using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAConfiguration", menuName = "IAConfiguration", order = 0)]
public class IAConfiguration : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private string nickname;
    [Range(0, 12)]
    [SerializeField] private float maxVelocity = 10;
    [Range(0, 100)]
    [SerializeField] private int errorPercentage = 0;

    public string EnemyName => enemyName;
    public string Nickname => nickname;
    public float MaxVelocity => maxVelocity;
    public int ErrorPercentage => errorPercentage;
}
