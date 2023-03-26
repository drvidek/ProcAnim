using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicsController : MonoBehaviour
{
    [SerializeField] private int _chainLength = 2;
    [SerializeField] private Transform _target, _pole;

    [SerializeField] private int _iterations;

    [SerializeField] private float _snappingDist = 0.001f;

    [Range(0, 1)]
    [SerializeField] private float _snapBackStrength;

    [SerializeField] private LineRenderer _lineRenderer;

    protected float[] _bonesLength;
    protected float _completeLength;
    protected Transform[] _joints;
    protected Vector3[] _positions;

    private void Awake()
    {
        _lineRenderer.GetComponent<LineRenderer>();
        Initialise();
    }

    void Initialise()
    {
        //set the size of the arrways based on our IK rig size
        _joints = new Transform[_chainLength + 1];
        _positions = new Vector3[_chainLength + 1];
        _bonesLength = new float[_chainLength];

        _completeLength = 0;

        _lineRenderer.positionCount = _chainLength + 1;


        //get the tip bone
        Transform currentBone = transform;
        //working our way from the tip to the root...
        for (int i = _joints.Length - 1; i >= 0; i--)
        {
            //store the current bone
            _joints[i] = currentBone;

            //if this is our tip, it has no bone, so we can ignore it
            if (i != _joints.Length - 1)
            {
                //otherwise set the length of the bone from this joint to the previous joint
                _bonesLength[i] = (_joints[i + 1].position - currentBone.position).magnitude;
                //and add that length to the total length of this rig
                _completeLength += _bonesLength[i];
            }

            currentBone = currentBone.parent;
        }

    }

    private void LateUpdate()
    {
        ResolveIK();
        HandleLineRenderer();
    }

    void ResolveIK()
    {
        //if we don't have a target, don't move
        if (_target == null)
            return;

        //if our bone rig stops matching the chain length, re-initialise the rig
        if (_bonesLength.Length != _chainLength)
            Initialise();

        //store our current jount positions
        for (int i = 0; i < _joints.Length; i++)
        {
            _positions[i] = _joints[i].position;
        }

        //if the target is too far away (based on the total legnth of the rig and the distance of the target from the root joint)
        if ((_target.position - _joints[0].position).sqrMagnitude >= _completeLength * _completeLength)
        {
            //get the direction to the target from the root
            Vector3 direction = (_target.position - _positions[0]).normalized;
            //for every joint, just stretch it torward that direction
            for (int i = 1; i < _positions.Length; i++)
            {
                //set the current joint position based on the direction to the target and the length of the last bone
                _positions[i] = _positions[i - 1] + direction * _bonesLength[i - 1];
            }
        }
        //if the target is within reach
        else
        {
            //defines how many times to repeat the kinematics
            for (int iteration = 0; iteration < _iterations; iteration++)
            {
                //from the tip backward, not touching the root, forcefully pull the joints towards the target
                for (int i = _positions.Length - 1; i > 0; i--)
                {
                    //if we're the tip, snap to the target
                    if (i == _positions.Length - 1)
                        _positions[i] = _target.position;
                    //else set the position based on the direction from the previous bone to this bone and this bone's length
                    else
                        _positions[i] = _positions[i + 1] + (_positions[i] - _positions[i + 1]).normalized * _bonesLength[i];
                }

                //from the root forward, pull towards the root based on the bone lengths and desired angles
                for (int i = 1; i < _positions.Length; i++)
                {
                    _positions[i] = _positions[i - 1] + (_positions[i] - _positions[i - 1]).normalized * _bonesLength[i - 1];
                }

                //if we're within snapping distance of the target, stop
                if ((_positions[_positions.Length - 1] - _target.position).sqrMagnitude < _snappingDist * _snappingDist)
                    break;
            }
        }

        //reapply the positions to the joints
        for (int i = 0; i < _positions.Length; i++)
        {
            _joints[i].position = _positions[i];
        }
    }


    private void HandleLineRenderer()
    {
        if (_lineRenderer == null)
            return;

        for (int i = 0; i < _chainLength + 1; i++)
        {
            _lineRenderer.SetPosition(i, _joints[i].position);
        }
    }
}
