using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty { Easy, Medium, Hard }

public class GameManager : Singleton<GameManager>
{
    public static Action GameConfiguredEvent;

    [Header("Mision")]
    [SerializeField] private Mission mission;

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
        iaCar.GetComponent<IACar>().SetIAConfiguration(corners, mission.GetTargetCorner(), iaConfiguration);
    }
    void OnCityBuilt()
    {
        corners = cityBuilder.Corners;
        targets = cityBuilder.Targets;
        mission.StartMission(targets, corners);
        ConfigureCars();
        GameConfiguredEvent?.Invoke();
    }
    void OnMissionCompleted(CarType carType)
    {
        
        if (carType == CarType.IA)
        {
            playerWon = false;
        }
        else
        {
            UIManager.Instance.ShowGameOverPanel(playerWon);
        }
    }
    public Corner[,] GetCorners()
    {
        return corners;
    }
    public Target[,] GetActiveTargets()
    {
        return mission.GetTargets();
    }
    public Corner GetTargetCorner()
    {
        return mission.GetTargetCorner();
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
        mission.MissionCompletedEvent += OnMissionCompleted;
    }

    void OnDisable()
    {
        CityBuilder.CityBuiltEvent -= OnCityBuilt;
        mission.MissionCompletedEvent -= OnMissionCompleted;
    }

    private void OnDestroy() {
        CityBuilder.CityBuiltEvent -= OnCityBuilt;
        mission.MissionCompletedEvent -= OnMissionCompleted;
    }
}
