using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class EnemyCard : MonoBehaviour
{
    public Action<IAConfiguration> SelectEnemyEvent;

    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI nickname;
    [SerializeField] private TextMeshProUGUI velocity;
    [SerializeField] private TextMeshProUGUI intelligence;

    private IAConfiguration iaConfiguration;

    public void Configure(IAConfiguration iaConfiguration)
    {
        this.iaConfiguration = iaConfiguration;
        enemyName.text = iaConfiguration.EnemyName;
        nickname.text = iaConfiguration.Nickname;
        velocity.text = "Velocity: " + Mathf.Min(iaConfiguration.MaxVelocity, 10).ToString();
        intelligence.text = "Intelligence: " + (100 - iaConfiguration.ErrorPercentage).ToString();
    }

    public void SelectEnemy()
    {
        SelectEnemyEvent?.Invoke(iaConfiguration);
    }


}
