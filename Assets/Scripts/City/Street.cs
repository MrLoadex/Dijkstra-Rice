using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Street : MonoBehaviour
{
    [SerializeField] private TextMeshPro velocityTMP;
    [SerializeField] private TextMeshPro minimapVelocityTMP;
    public int Weight = 0;

    void Start()
    {
        
    }

    public void UpdateValue()
    {
        int velocity = (int)(100 * Mathf.Exp(-(0.3f * Weight)));
        velocityTMP.text = velocity.ToString();
        minimapVelocityTMP.text = velocity.ToString();
    }

    public void Updat()
    {

    }
}
