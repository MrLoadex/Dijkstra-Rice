using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
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
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject mobilePanel;

    
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

    [Header("Config Start Panel")]
    [SerializeField] private TextMeshProUGUI startPanelText;
    [SerializeField] private EnemyCard enemyCardStartPanel;

    [Header("Config Mobile")]
    [SerializeField] private Button turnLeftButton;
    [SerializeField] private Button turnRightButton;
    public static Action TurnLeftActionEvent;
    public static Action TurnRightActionEvent;
    public static Action AccelerateActionEvent;

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
        if (nameNextMissionInput.text == "") nameNextMissionInput.text = "Anónimo";
        else if (nameNextMissionInput.text.Length > 10) nameNextMissionInput.text = nameNextMissionInput.text.Substring(0, 10);
        nameNextMissionInput.text = nameNextMissionInput.text.Replace(" ", "_");
        ScoresManager.Instance.AddScore(nameNextMissionInput.text);
        ShowEndGameScorePanel();
    }
    public void TurnLeft()
    {
        TurnLeftActionEvent?.Invoke();
    }
    public void TurnRight()
    {
        TurnRightActionEvent?.Invoke();
    }
    public void Accelerate()
    {
        AccelerateActionEvent?.Invoke();
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
        startPanel.SetActive(false);
        mobilePanel.SetActive(false);
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
    public void ShowStartPanel(IAConfiguration iaConfiguration)
    {
        ClosePanels();
        startPanel.SetActive(true);
        enemyCardStartPanel.Configure(iaConfiguration);
        StartCoroutine(showStartPanelCoroutine(3));
    }
    IEnumerator showStartPanelCoroutine(float seconds)
    {
        if (seconds == 3) //Esto esta aca porque si no, el panel no se activa. Debe ser porque se esta llamando a cerrar paneles desde otro lado. Pero me da paja averiguar desde donde.
        {
            yield return new WaitForSeconds(0.1f);
            startPanel.SetActive(true);
        }
        if (seconds > 0)
        {
            startPanelText.text = seconds.ToString();
            SoundManager.Instance.PlayReadySound();
            yield return new WaitForSeconds(1); // 3 / 2 / 1
            StartCoroutine(showStartPanelCoroutine(seconds-1));
        }
        else
        {
            startPanelText.text = "GO!";
            SoundManager.Instance.PlayStartSound();
            yield return new WaitForSeconds(0.3f);
            ClosePanels();
            ShowMobilePanel();
        }
    }
    public void ShowMobilePanel() // Diferente a los otros show porque no cierra otros paneles
    {
        mobilePanel.SetActive(true);
    }
}