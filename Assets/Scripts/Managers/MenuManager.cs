using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButton;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject scoresPanel;
    [SerializeField] private GameObject buildCityPanel;
    [SerializeField] private GameObject enemiesPanel;

    [Header("Error Window")]
    [SerializeField] private GameObject errorWindowPrefab;

    [Header("Enemies Config")]
    [SerializeField] private IAConfiguration[] enemiesConfigurations;
    [SerializeField] private EnemyCard enemyCardPrefab;

    [Header("Match Config")]
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private NormalMatch normalMatch;
    [SerializeField] private CasualMatch casualMatch;

    [Header("Build City Config")]
    [SerializeField] private TMP_InputField cityBlocksXInput;
    [SerializeField] private TMP_InputField cityBlocksZInput;

    [Header("Enemy Panel Config")]
    [SerializeField] private Transform enemysContent;


    private void Start() 
    {
        ConfigureCityBlocksInput();
        gameSettings.Reset();
        ShowMainPanel();
    }

    

    private void Update() {
        if (Input.touchCount > 0)
        {
            gameSettings.Mobile = true;
        }
    }

    public void ClosePanels()
    {
        mainPanel.SetActive(false);
        scoresPanel.SetActive(false);
        buildCityPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        mainMenuButton.SetActive(true);
    }
    public void ShowMainPanel()
    {
        ClosePanels();
        mainMenuButton.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void ShowScoresPanel()
    {

        ClosePanels();
        scoresPanel.SetActive(true);
    }
    public void ShowBuildCityPanel()
    {
        ClosePanels();
        buildCityPanel.SetActive(true);
    }
    public void ShowEnemiesPanel()
    {
        ClosePanels();
        //limpiar el panel de enemigos
        foreach (Transform child in enemysContent)
        {
            child.GetComponent<EnemyCard>().SelectEnemyEvent -= setEnemyConfiguration;
            Destroy(child.gameObject);
        }
        //crear los enemigos en el panel
        foreach (var enemy in enemiesConfigurations)
        {
            var enemyCard = Instantiate(enemyCardPrefab, enemysContent);
            enemyCard.SelectEnemyEvent += setEnemyConfiguration;
            enemyCard.Configure(enemy);
        }
        enemiesPanel.SetActive(true);
    }
    public void ShowErrorWindow(string message, Transform parent)
    {
        var errorWindow = Instantiate(errorWindowPrefab, parent);
        //centrar el error window
        errorWindow.transform.SetParent(parent, false);
        //errorWindow.transform.localPosition = Vector3.zero;
        errorWindow.GetComponent<ErrorWindow>().Configure(message);
    }
    void ConfigureCityBlocksInput()
    {
        cityBlocksXInput.contentType = TMP_InputField.ContentType.Standard;
        cityBlocksXInput.inputType = TMP_InputField.InputType.Standard;
        cityBlocksXInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
        cityBlocksXInput.text = "1";
        cityBlocksXInput.characterLimit = 2;

        cityBlocksZInput.contentType = TMP_InputField.ContentType.Standard;
        cityBlocksZInput.inputType = TMP_InputField.InputType.Standard;
        cityBlocksZInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
        cityBlocksZInput.text = "1";
        cityBlocksZInput.characterLimit = 2;
    }
    public void CreateNormalGame()
    {
        gameSettings.FreeGame = false;
        normalMatch.Reset();
        SceneManager.LoadScene("GameScene");
    }
    void setEnemyConfiguration(IAConfiguration iaConfiguration)
    {
        casualMatch.IAConfiguration = iaConfiguration;
        CreateCasualGame();
    }
    public void SetCityBlocks()
    {
        if (int.TryParse(cityBlocksXInput.text, out int x) && int.TryParse(cityBlocksZInput.text, out int z))
        {
            casualMatch.CityBlocksX = x;
            casualMatch.CityBlocksZ = z;
        }
        else
        {
            ShowErrorWindow("Invalid input", buildCityPanel.transform);
            return;
        }
        if (x <= 0 || z <= 0)
        {
            ShowErrorWindow("Your city must have at least 1 block", buildCityPanel.transform);
            return;
        }
        else if (x > 30 || z > 30)
        {
            ShowErrorWindow("Your city must have at most 30 blocks", buildCityPanel.transform);
            return;
        }
        casualMatch.CityBlocksX = x;
        casualMatch.CityBlocksZ = z;
        ShowEnemiesPanel();
    }
    public void CreateCasualGame()
    {
        if (casualMatch.IAConfiguration == null)
        {
            ShowErrorWindow("No enemy selected", enemiesPanel.transform);
            return;
        }
        if (casualMatch.CityBlocksX == 0 || casualMatch.CityBlocksZ == 0)
        {
            ShowErrorWindow("Invalid city blocks", enemiesPanel.transform);
            return;
        }
        gameSettings.FreeGame = true;
        SceneManager.LoadScene("GameScene");
    }
    public void AddXToCityBlocksToInput(int num)
    {
        if (int.TryParse(cityBlocksXInput.text, out int x))
        {
            cityBlocksXInput.text = (x + num).ToString();
        }
        else
        {
            cityBlocksXInput.text = num.ToString();
        }
        if (int.Parse(cityBlocksXInput.text) < 0)
        {
            cityBlocksXInput.text = "1";
        }
    }
    public void AddZToCityBlocksToInput(int num)
    {
        if (int.TryParse(cityBlocksZInput.text, out int z))
        {
            cityBlocksZInput.text = (z + num).ToString();
        }
        else
        {
            cityBlocksZInput.text = num.ToString();
        }
        if (int.Parse(cityBlocksZInput.text) < 0)
        {
            cityBlocksZInput.text = "1";
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OnDestroy()
    {
        if (enemiesPanel != null)
        {
            if (enemiesPanel.GetComponentInChildren<EnemyCard>() != null)
            {
                foreach (var enemyCard in enemiesPanel.GetComponentsInChildren<EnemyCard>())
                {
                if (enemyCard != null)
                {
                        enemyCard.SelectEnemyEvent -= setEnemyConfiguration;
                    }
                }
            }
        }
    }
    public void OnDisable()
    {
        if (enemiesPanel != null)
        {
            if (enemiesPanel.GetComponentInChildren<EnemyCard>() != null)
            {
            foreach (var enemyCard in enemiesPanel.GetComponentsInChildren<EnemyCard>())
            {
                    if (enemyCard != null)
                    {
                        enemyCard.SelectEnemyEvent -= setEnemyConfiguration;
                    }
                }
            }
        }
    }
}
