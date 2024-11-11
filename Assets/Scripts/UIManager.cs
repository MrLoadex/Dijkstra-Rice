using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    [Header("Configuraciones de Enemigos")]
    [SerializeField] private IAConfiguration[] enemiesConfigurations;
    [SerializeField] private EnemyCard enemyCardPrefab;

    [Header("Paneles")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject buildCityPanel;
    [SerializeField] private GameObject enemyCardPanel;
    [SerializeField] private GameObject freeGameOverPanel;
    [SerializeField] private GameObject scoresPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject nextMissionPanel;
    [SerializeField] private GameObject EndGameScorePanel;

    
    [Header("Config Ciudad")]
    [SerializeField] private TMP_InputField cityBlocksXInput;
    [SerializeField] private TMP_InputField cityBlocksZInput;
    
    [Header("Config Free Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverFreeGameText;

    [Header("Config Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TMP_InputField nameNextMissionInput;

    [Header("Config Next Mission")]
    [SerializeField] private TextMeshProUGUI scoreNextMissionText;

    [Header("Ventana de Error")]
    [SerializeField] private GameObject errorWindowPrefab;


    private int cityBlocksX;
    private int cityBlocksZ;

    void Start()
    {
        ShowMainPanel();
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
            GameManager.Instance.CreateNewFreeGame(iaConfiguration, cityBlocksX, cityBlocksZ);
        }
    }    
    public void SubmitUserName()
    {
        ScoresManager.Instance.AddScore(nameNextMissionInput.text);
        ShowEndGameScorePanel();
    }
    public void ClosePanels()
    {
        mainPanel.SetActive(false);
        buildCityPanel.SetActive(false);
        enemyCardPanel.SetActive(false);
        scoresPanel.SetActive(false);
        freeGameOverPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        nextMissionPanel.SetActive(false);
        EndGameScorePanel.SetActive(false);
    }
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
    public void ShowMainPanel()
    {
        ClosePanels();
        mainPanel.SetActive(true);
    }
    public void ShowFreeGameOverPanel(bool playerWon)
    {
        freeGameOverPanel.SetActive(true);
        gameOverFreeGameText.text = playerWon ? "You Won" : "You Lose";
    }
    public void ShowScoresPanel()
    {
        ClosePanels();
        scoresPanel.SetActive(true);
    }
    public void ShowGameOverPanel(int score)
    {
        ClosePanels();
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "SCORE: " + score.ToString();
    }
    public void ShowNextMissionPanel(int score)
    {
        ClosePanels();
        nextMissionPanel.SetActive(true);
        scoreNextMissionText.text = "SCORE: " + score.ToString();
    }
    public void ShowEndGameScorePanel()
    {
        ClosePanels();
        EndGameScorePanel.SetActive(true);
    }
}