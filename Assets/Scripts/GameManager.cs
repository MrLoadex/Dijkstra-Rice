using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    [Header("Configuraciones Generales")]
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private float startTime = 15f;
    [SerializeField] private Vector2Int carStartPosition = new Vector2Int(0, 0);
    [SerializeField] private GameObject cityStartPointPrefab;

    [Header("Configuraciones de Partidas")]
    [SerializeField] private NormalMatch gameNormalMatch;
    [SerializeField] private CasualMatch casualMatch;

    [Header("Misiones")]
    [SerializeField] private bool freeGame;
    [SerializeField] private Mission actualMission;

    [Header("Constructor de Ciudad")]
    [SerializeField] private CityBuilder cityBuilder;

    [Header("Prefab de Coches")]
    [SerializeField] private Car playerCarPrefab;
    [SerializeField] private Car iaCarPrefab;

    [Header("Cámara")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 20, 0);

    [Header("Dificultad de IA")]
    [SerializeField] private IAConfiguration iaConfiguration;

    private bool isMobile;
    private Corner[,] corners;
    private Target[,] targets;
    private bool playerWon = true;
    private int actualScore;
    public int ActualScore => actualScore;
    public bool IsMobile => isMobile;

    void Start()
    {
        Time.timeScale = 1;

        isMobile = gameSettings.Mobile;

        if (gameSettings.FreeGame)
        {
            CreateNewFreeGame(casualMatch.IAConfiguration, casualMatch.CityBlocksX, casualMatch.CityBlocksZ);
        }
        else
        {
            continueNormalGame();
        }
    }
    void ConfigureCars()
    {   
        // Instanciar el coche del jugador.
        GameObject playerCar = Instantiate(playerCarPrefab.gameObject, corners[carStartPosition.x, carStartPosition.y].transform.position, Quaternion.identity);
        // Instanciar el coche de la IA.
        GameObject iaCar = Instantiate(iaCarPrefab.gameObject, corners[carStartPosition.x, carStartPosition.y].transform.position - Vector3.forward * 5, Quaternion.identity);

        // Configurar el coche del jugador.
        if (mainCamera == null) mainCamera = Camera.main;
        mainCamera.transform.SetParent(playerCar.transform);
        mainCamera.transform.position = cameraOffset;
        playerCar.GetComponent<Car>().SetStartCorner(corners[0, 0]);
        playerCar.GetComponent<Car>().SetNextCorner(corners[0, 1]);

        // Configurar el coche de la IA.
        iaCar.GetComponent<Car>().SetStartCorner(corners[0, 0]);
        iaCar.GetComponent<Car>().SetNextCorner(corners[0, 1]);
        iaCar.GetComponent<IACar>().SetIAConfiguration(corners, actualMission.GetTargetCorner(), iaConfiguration);
    }
    void OnCityBuilt()
    {
        if (freeGame && actualMission == null) actualMission = gameNormalMatch.Missions[0];
        
        corners = cityBuilder.Corners;
        targets = cityBuilder.Targets;
        actualMission.StartMission(targets, corners);
        UIManager.Instance.ShowStartPanel(iaConfiguration, startTime);
        if (isMobile) UIManager.Instance.ShowMobilePanel();
        //mostrar un punto en la pantalla donde se va a empezar la ciudad
        Vector3 cityStartPosition = corners[carStartPosition.x, carStartPosition.y].transform.position;
        GameObject cityStartPoint = Instantiate(cityStartPointPrefab, cityStartPosition, Quaternion.identity);
        Destroy(cityStartPoint, startTime);
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(startTime);
        SoundManager.Instance.PlayGameMusic();
        ConfigureCars();
    }
    void OnMissionCompleted(CarType carType)
    {
        
        if (carType == CarType.IA)
        {
            playerWon = false;
        }
        else
        {
            if (freeGame)
            {
                if (playerWon) SoundManager.Instance.PlayWinSound();
                else SoundManager.Instance.PlayLoseSound();

                UIManager.Instance.ShowFreeGameOverPanel(playerWon);
                return;
            }
            if (playerWon)
            {
                SoundManager.Instance.PlayWinSound();
                gameNormalMatch.Score += 1;
                gameNormalMatch.LastMission = actualMission;
                gameNormalMatch.Continue = true;
                UIManager.Instance.ShowNextMissionPanel(gameNormalMatch.Score);
            }
            else
            {
                SoundManager.Instance.PlayLoseSound();
                UIManager.Instance.ShowGameOverPanel(gameNormalMatch.Score);
                actualScore = gameNormalMatch.Score;
                gameNormalMatch.Reset();
            }
            //pausar el tiempo
            Time.timeScale = 0;
        }
    }
    void continueNormalGame()
    {
        if (!gameNormalMatch.Continue)
        {
            CreateNewNormalGame();
            return;
        }
        // Si se continua la partida, se carga la siguiente mision.
        freeGame = false;
        UIManager.Instance.ClosePanels();
        
        int lastMissionIndex = Array.IndexOf(gameNormalMatch.Missions, gameNormalMatch.LastMission); // 0
        
        if (lastMissionIndex < gameNormalMatch.Missions.Length - 1) // 0 < 2
        {
            Debug.Log("Nueva mision: " + gameNormalMatch.Missions[lastMissionIndex + 1].name);
            actualMission = gameNormalMatch.Missions[lastMissionIndex + 1]; // gameMatch.Missions[1]
        }
        else
        {
            actualMission = gameNormalMatch.LastMission; // gameMatch.Missions[0]
        }
        iaConfiguration = actualMission.IAConfiguration;
        int randomX = UnityEngine.Random.Range(5, 16);
        int randomZ = UnityEngine.Random.Range(5, 16);
        cityBuilder.BuildCity(randomX, randomZ);
    }
    public void CreateNewFreeGame(IAConfiguration iaConfiguration, int cityBlocksX, int cityBlocksZ)
    {
        this.iaConfiguration = iaConfiguration;
        freeGame = true;
        actualMission = null;
        cityBuilder.BuildCity(cityBlocksX, cityBlocksZ);
    }
    public void CreateNewNormalGame()
    {
        freeGame = false;
        actualMission = gameNormalMatch.Missions[0];
        iaConfiguration = gameNormalMatch.Missions[0].IAConfiguration;
        int randomX = UnityEngine.Random.Range(5, 16);
        int randomZ = UnityEngine.Random.Range(5, 16);
        cityBuilder.BuildCity(randomX, randomZ);
    }
    public Corner[,] GetCorners()
    {
        return corners;
    }
    public Target[,] GetActiveTargets()
    {
        return actualMission.GetTargets();
    }
    public Corner GetTargetCorner()
    {
        return actualMission.GetTargetCorner();
    }
    public void SetIAConfiguration(IAConfiguration iaConfiguration)
    {
        this.iaConfiguration = iaConfiguration;
    }
    public void RestartGame()
    {
        Debug.Log("RestartGame");
        SceneManager.LoadScene("menuScene");
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene("gameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    void OnEnable()
    {
        CityBuilder.CityBuiltEvent += OnCityBuilt;
        Mission.MissionCompletedEvent += OnMissionCompleted;
    }
    void OnDisable()
    {
        CityBuilder.CityBuiltEvent -= OnCityBuilt;
        Mission.MissionCompletedEvent -= OnMissionCompleted;
    }
    private void OnDestroy() {
        CityBuilder.CityBuiltEvent -= OnCityBuilt;
        Mission.MissionCompletedEvent -= OnMissionCompleted;
    }
}