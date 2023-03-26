using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegPair : MonoBehaviour
{
    [SerializeField] private IKLeg _leftLeg, _rightLeg;

    private void Update()
    {
        if (!_leftLeg.Moving && _leftLeg.DistanceToTarget > _leftLeg.MaxDistance)
        {
            _leftLeg.StartCoroutine(_leftLeg.MoveTowardsTarget(_rightLeg));
        }
        else
        if (!_rightLeg.Moving && _rightLeg.DistanceToTarget > _rightLeg.MaxDistance)
        {
            _rightLeg.StartCoroutine(_rightLeg.MoveTowardsTarget(_leftLeg));
        }

        //if (!_rightLeg.Moving)
        //{
        //    if (_leftLeg.DistanceToTarget > _leftLeg.MaxDistance && !_leftLeg.Moving)
        //    {
        //        _leftLeg.StartCoroutine(_leftLeg.MoveTowardsTarget(_rightLeg.transform));
        //    }
        //}
        //
        //if (!_leftLeg.Moving)
        //{
        //    if (_rightLeg.DistanceToTarget > _rightLeg.MaxDistance)
        //    {
        //        _rightLeg.StartCoroutine(_rightLeg.MoveTowardsTarget(_rightLeg.transform));
        //    }
        //}

    }

}
