using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointInfo : MonoBehaviour
{
    public Vector3 axis;
    public Vector3 startOffset;

    private void Awake()
    {
        startOffset = transform.localPosition;
    }
}
