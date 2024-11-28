using TMPro;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;

    public void Configure(string text)
    {
        errorText.text = text;
    }

    void Start()
    {
        Debug.Log("ErrorWindow Start");
        Destroy(gameObject, 1.5f);
    }

    private void OnDestroy()
    {
        Debug.Log("ErrorWindow Destroy");
    }
}
