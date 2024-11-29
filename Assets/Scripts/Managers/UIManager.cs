using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
public class UIManager : Singleton<UIManager>
{
    [Header("Paneles")]
    [SerializeField] private GameObject casualGameOverPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject nextMissionPanel;
    [SerializeField] private GameObject endGameScorePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject mobilePanel;
    [SerializeField] private GameObject pausePanel;
    [Header("Config Casual Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverCasualGameText;

    [Header("Config Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TMP_InputField nameGameOverInput;

    [Header("Config Next Mission")]
    [SerializeField] private TextMeshProUGUI scoreNextMissionText;


    [Header("Config Start Panel")]
    [SerializeField] private TextMeshProUGUI startPanelText;
    [SerializeField] private EnemyCard enemyCardStartPanel;

    [SerializeField] private Camera minimapCamera;

    [Header("Constructor de Ciudad")]
    [SerializeField] private CityBuilder cityBuilder;

    private TouchScreenKeyboard keyboard;

    public static Action<bool> TurnLeftActionEvent;
    public static Action<bool> TurnRightActionEvent;
       
    public void SubmitUserName()
    {
        if (nameGameOverInput.text == "") nameGameOverInput.text = "Anónimo";
        else if (nameGameOverInput.text.Length > 10) nameGameOverInput.text = nameGameOverInput.text.Substring(0, 10);
        nameGameOverInput.text = nameGameOverInput.text.Replace(" ", "_");
        ScoresManager.Instance.AddScore(nameGameOverInput.text);
        ShowEndGameScorePanel();
    }
    public void ClosePanels()
    {
        casualGameOverPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        nextMissionPanel.SetActive(false);
        endGameScorePanel.SetActive(false);
        startPanel.SetActive(false);
        mobilePanel.SetActive(false);
        pausePanel.SetActive(false);
    }
    public void ShowFreeGameOverPanel(bool playerWon)
    {
        casualGameOverPanel.SetActive(true);
        gameOverCasualGameText.text = playerWon ? "You Won" : "You Lose";
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
        endGameScorePanel.SetActive(true);
    }
    public void ShowStartPanel(IAConfiguration iaConfiguration, float startPanelTime)
    {
        ClosePanels();
        startPanel.SetActive(true);
        enemyCardStartPanel.Configure(iaConfiguration);
        StartCoroutine(showStartPanelCoroutine(startPanelTime));
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
            bool isMobile = GameManager.Instance.IsMobile;
            if (isMobile) ShowMobilePanel();    
        }
    }
    public void ShowMobilePanel() // Diferente a los otros show porque no cierra otros paneles
    {
        mobilePanel.SetActive(true);
    }
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }
    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open(nameGameOverInput.text, TouchScreenKeyboardType.Default);
    }

    void Update()
    {
        if (keyboard != null && keyboard.active)
        {
            if (nameGameOverInput.text != keyboard.text)
            {
                nameGameOverInput.text = keyboard.text;
            }
        }
    }

    void onCityBuilt()
    {
        int cityBlocks = cityBuilder.CityBlocks;
        minimapCamera.orthographicSize = Mathf.Max(cityBlocks * 4f, 1);
        minimapCamera.transform.position = new Vector3(cityBuilder.CityCenter.x, minimapCamera.transform.position.y, cityBuilder.CityCenter.z);
    }
    public void MoveXMinimapCamera(int x)
    {
        minimapCamera.transform.position = new Vector3(minimapCamera.transform.position.x + x, minimapCamera.transform.position.y, minimapCamera.transform.position.z);
    }
    public void MoveZMinimapCamera(int z)
    {
        minimapCamera.transform.position = new Vector3(minimapCamera.transform.position.x, minimapCamera.transform.position.y, minimapCamera.transform.position.z + z);
    }
    public void ZoomMinimapCamera(float zoom)
    {
        minimapCamera.orthographicSize += zoom;
    }
    public void TurnLeft(bool turnLeft)
    {
        TurnLeftActionEvent?.Invoke(turnLeft);
    }
    public void TurnRight(bool turnRight)
    {
        TurnRightActionEvent?.Invoke(turnRight);
    }
    void OnEnable()
    {
        CityBuilder.CityBuiltEvent += onCityBuilt;
    }
    void OnDisable()
    {
        CityBuilder.CityBuiltEvent -= onCityBuilt;
    }
    void OnDestroy()
    {
        CityBuilder.CityBuiltEvent -= onCityBuilt;
    }
}