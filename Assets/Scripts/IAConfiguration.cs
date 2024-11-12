using UnityEngine;

[CreateAssetMenu(fileName = "IAConfiguration", menuName = "IAConfiguration", order = 0)]
public class IAConfiguration : ScriptableObject
{
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private string enemyName;
    [SerializeField] private string nickname;
    [Range(0, 10)]
    [SerializeField] private float maxVelocity = 10;
    [Range(0, 100)]
    [SerializeField] private int errorPercentage = 0;

    public Sprite EnemySprite => enemySprite;
    public string EnemyName => enemyName;
    public string Nickname => nickname;
    public float MaxVelocity => maxVelocity;
    public int ErrorPercentage => errorPercentage;
}
