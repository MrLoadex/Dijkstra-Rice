using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private ScoreGUI scoreGUI1;
    [SerializeField] private ScoreGUI scoreGUI2;
    [SerializeField] private ScoreGUI scoreGUI3;
    [SerializeField] private ScoreGUI scoreGUI4;
    [SerializeField] private ScoreGUI scoreGUI5;

    void OnEnable()
    {
        ScoresManager.UpdatedScoresEvent += onUpdatedScoresEvent;
        ScoresManager.Instance.RequestCheckActualScores();
    }

    void OnDisable()
    {
        ScoresManager.UpdatedScoresEvent -= onUpdatedScoresEvent;
    }

    void onUpdatedScoresEvent((int, string)[] scores)
    {
        scoreGUI1.name.text = scores[0].Item2;
        scoreGUI1.score.text = scores[0].Item1.ToString();
        scoreGUI2.name.text = scores[1].Item2;
        scoreGUI2.score.text = scores[1].Item1.ToString();
        scoreGUI3.name.text = scores[2].Item2;
        scoreGUI3.score.text = scores[2].Item1.ToString();
        scoreGUI4.name.text = scores[3].Item2;
        scoreGUI4.score.text = scores[3].Item1.ToString();
        scoreGUI5.name.text = scores[4].Item2;
        scoreGUI5.score.text = scores[4].Item1.ToString();
    }


}

[System.Serializable]
struct ScoreGUI
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI score;
}
