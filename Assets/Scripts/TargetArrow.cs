using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    Transform target;

    private void Start() {
        target = GameManager.Instance.GetTargetCorner().transform;
    }

    private void Update() {
        if (target == null) return;

        transform.LookAt(target);
    }
}
