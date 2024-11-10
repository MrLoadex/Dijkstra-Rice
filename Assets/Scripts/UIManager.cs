using UnityEngine;
using TMPro;
using System;
public class UIManager : Singleton<UIManager>
{
    public static Action<IAConfiguration, int, int> GameConfiguredEvent;

    [Header("Configuraciones de Enemigos")]
    [SerializeField] private IAConfiguration[] enemiesConfigurations;
    [SerializeField] private EnemyCard enemyCardPrefab;

    [Header("Paneles")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject buildCityPanel;
    [SerializeField] private GameObject enemyCardPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Config Ciudad")]
    [SerializeField] private TMP_InputField cityBlocksXInput;
    [SerializeField] private TMP_InputField cityBlocksZInput;
    
    [Header("Config Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("Ventana de Error")]
    [SerializeField] private GameObject errorWindowPrefab;

    private int cityBlocksX;
    private int cityBlocksZ;

    public void ShowBuildCityPanel()
    {
        ClosePanels();
        buildCityPanel.SetActive(true);
    }

    public void ShowEnemyCardPanel()
    {
        ClosePanels();
        //limpiar el panel de enemigos
        foreach (Transform child in enemyCardPanel.transform)
        {
            Destroy(child.gameObject);
        }
        enemyCardPanel.SetActive(true);
        foreach (var enemyConfiguration in enemiesConfigurations)
        {
            var enemyCard = Instantiate(enemyCardPrefab, enemyCardPanel.transform);
            enemyCard.SelectEnemyEvent += SetEnemyConfiguration;
            enemyCard.Configure(enemyConfiguration);
        }

    }

    public void CollectCityBlocks()
    {
        cityBlocksX = int.Parse(cityBlocksXInput.text);
        cityBlocksZ = int.Parse(cityBlocksZInput.text);
    }

    void SetEnemyConfiguration(IAConfiguration iaConfiguration)
    {
        //ocultar paneles
        ClosePanels();
        if (iaConfiguration == null)
        {
            //instanciar la ventana de error
            var errorWindow = Instantiate(errorWindowPrefab, transform);
            errorWindow.GetComponent<ErrorWindow>().SetErrorText("Error: IA Configuration is null");
            return;
        }
        else if (cityBlocksX == 0 || cityBlocksZ == 0)
        {
            var errorWindow = Instantiate(errorWindowPrefab, transform);
            errorWindow.GetComponent<ErrorWindow>().SetErrorText("Error: City blocks are not set");
            return;
        }
        else
        {
            GameConfiguredEvent?.Invoke(iaConfiguration, cityBlocksX, cityBlocksZ);
        }
    }

    void ClosePanels()
    {
        mainPanel.SetActive(false);
        buildCityPanel.SetActive(false);
        enemyCardPanel.SetActive(false);
    }

    public void ShowGameOverPanel(bool playerWon)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = playerWon ? "You Won" : "You Lose";
    }
}