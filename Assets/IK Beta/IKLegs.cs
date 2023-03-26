using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegs : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _maxDistance, _snappingDistance;
    [SerializeField] private AnimationCurve _yCurve;
    [SerializeField] private float _yMaxHeight;
    bool _moving;
    Vector3 _currentPosition;
    [SerializeField] private Transform _root;
    [SerializeField] private float _stepDuration;

    private void Start()
    {
        _currentPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(_target.position, transform.position) > _maxDistance && !_moving)
        {
            StartCoroutine(MoveTowardsTarget());
        }
        else
            transform.position = _currentPosition;
    }

    IEnumerator MoveTowardsTarget()
    {
        _moving = true;
        Vector3 initPosition = transform.position;
        for (int i = 0; i < _stepDuration; i++)
        {
            float lerp = i / (_stepDuration + 1f);

            Vector3 nextPosition = Vector3.Lerp(initPosition, _target.position, lerp);
            float yPos = _yCurve.Evaluate(lerp) * _yMaxHeight;
            nextPosition.y = yPos;
            transform.position = nextPosition;
            
            i++;
            yield return new WaitForFixedUpdate();
        }

        transform.position = _target.position;
        _currentPosition = transform.position;
        _moving = false;
    }

}
