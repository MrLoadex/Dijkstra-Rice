using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameMatch gameMatch;

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

    private Corner[,] corners;
    private Target[,] targets;
    private bool playerWon = true;
    private int actualScore;
    public int ActualScore => actualScore;

    void Start()
    {
        // Si se continua la partida, se carga la siguiente mision.
        if (gameMatch.Continue)
        {
            freeGame = false;
            UIManager.Instance.ClosePanels();
            
            int lastMissionIndex = Array.IndexOf(gameMatch.Missions, gameMatch.LastMission);
            
            if (lastMissionIndex < gameMatch.Missions.Length - 1)
            {
                actualMission = gameMatch.Missions[lastMissionIndex + 1];
            }
            else
            {
                actualMission = gameMatch.LastMission;
            }
            int randomX = UnityEngine.Random.Range(5, 16);
            int randomZ = UnityEngine.Random.Range(5, 16);
            cityBuilder.BuildCity(randomX, randomZ);
        }
        // Si no se continua la partida, se carga la primera mision.
        else 
        {
            Debug.Log("Comenzando nueva partida");
        }
    }
    void ConfigureCars()
    {
        // Instanciar el coche del jugador.
        GameObject playerCar = Instantiate(playerCarPrefab.gameObject, corners[0, 0].transform.position, Quaternion.identity);
        // Instanciar el coche de la IA.
        GameObject iaCar = Instantiate(iaCarPrefab.gameObject, corners[0, 0].transform.position - Vector3.forward * 10, Quaternion.identity);

        // Configurar el coche del jugador.
        if (mainCamera == null) mainCamera = Camera.main;
        mainCamera.transform.SetParent(playerCar.transform);
        mainCamera.transform.position = cameraOffset;
        playerCar.GetComponent<Car>().SetStartCorner(corners[0, 0]);
        playerCar.GetComponent<Car>().SetNextCorner(corners[1, 0]);

        // Configurar el coche de la IA.
        iaCar.GetComponent<Car>().SetStartCorner(corners[0, 0]);
        iaCar.GetComponent<Car>().SetNextCorner(corners[1, 0]);
        Debug.Log("IA: " + iaConfiguration.EnemyName);
        iaCar.GetComponent<IACar>().SetIAConfiguration(corners, actualMission.GetTargetCorner(), iaConfiguration);
    }
    void OnCityBuilt()
    {
        if (freeGame && actualMission == null) actualMission = gameMatch.Missions[0];
        corners = cityBuilder.Corners;
        targets = cityBuilder.Targets;
        actualMission.StartMission(targets, corners);
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
                UIManager.Instance.ShowFreeGameOverPanel(playerWon);
                return;
            }
            if (playerWon)
            {
                gameMatch.Score += 1;
                gameMatch.LastMission = actualMission;
                gameMatch.Continue = true;
                UIManager.Instance.ShowNextMissionPanel(gameMatch.Score);
            }
            else
            {
                UIManager.Instance.ShowGameOverPanel(gameMatch.Score);
                actualScore = gameMatch.Score;
                gameMatch.Score = 0;
                gameMatch.Continue = false;
                gameMatch.LastMission = null;
            }
        }
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
        actualMission = gameMatch.Missions[0];
        iaConfiguration = gameMatch.Missions[0].IAConfiguration;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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