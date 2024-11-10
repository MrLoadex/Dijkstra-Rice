using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Street : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    public int Weight = 0;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
    }

    public void UpdateValue()
    {
        int velocity = (int)(100 * Mathf.Exp(-(0.3f * Weight)));
        textMesh.text = velocity.ToString();
    }

    public void Updat()
    {

    }
}
