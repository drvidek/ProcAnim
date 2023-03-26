using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegPair : MonoBehaviour
{
    [SerializeField] private IKLeg _leftLeg, _rightLeg;

    public bool EitherLegMoving => _leftLeg.Moving || _rightLeg.Moving;

    private void Update()
    {
        if (!EitherLegMoving)
        {
            if (!_leftLeg.Moving && _leftLeg.DistanceToTarget > _leftLeg.MaxDistance)
            {
                _leftLeg.StartCoroutine(_leftLeg.MoveTowardsTarget(_rightLeg));
            }

            if (!_rightLeg.Moving && _rightLeg.DistanceToTarget > _rightLeg.MaxDistance)
            {
                _rightLeg.StartCoroutine(_rightLeg.MoveTowardsTarget(_leftLeg));
            }
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
