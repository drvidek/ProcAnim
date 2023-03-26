using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    [SerializeField] private JointInfo[] joints;
    [SerializeField] private float samplingDistance, learningRate, stoppingDistance;
    [SerializeField] private Transform target;
    [SerializeField] float[] angles;
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = joints.Length;
    }

    private void Update()
    {
        DoInverseKinematics(target.position, angles);
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < joints.Length; i++)
        {
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i].axis);
            Vector3 nextPoint = prevPoint + (rotation * joints[i].startOffset);
            prevPoint = nextPoint;

            joints[i].transform.position = nextPoint;
            //joints[i].transform.localEulerAngles = angles[i] * joints[i].axis;

            //joints[i].transform.rotation = rotation;
            lineRenderer.SetPosition(i,nextPoint);
        }
    }

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < joints.Length; i++)
        {
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i].axis);
            Vector3 nextPoint = prevPoint + (rotation * joints[i].startOffset);
            prevPoint = nextPoint;
        }
        return prevPoint;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        float angle = angles[i];

        float f_x = DistanceFromTarget(target, angles);

        angles[i] += samplingDistance;

        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / samplingDistance;

        angles[i] = angle;

        return gradient;
    }

    public void DoInverseKinematics(Vector3 target, float[] angles)
    {
        if (DistanceFromTarget(target, angles) < stoppingDistance)
            return;

        for (int i = joints.Length -1; i >= 0; i--)
        {
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= learningRate * gradient;

           // if (DistanceFromTarget(target, angles) < stoppingDistance)
           //     return;
        }
    }
}
