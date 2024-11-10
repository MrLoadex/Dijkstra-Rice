using TMPro;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;

    public void SetErrorText(string text)
    {
        errorText.text = text;
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }
}
