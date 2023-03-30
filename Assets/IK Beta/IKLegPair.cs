using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegPair : MonoBehaviour
{
    [SerializeField] private IKFootFollow _leftFootFollow, _rightFootFollow;
    [ SerializeField] private IKLimb _leftLeg, _rightLeg;
    public IKFootFollow LeftFollow => _leftFootFollow;
    public IKFootFollow RightFollow => _rightFootFollow;

    public bool EitherLegMoving => _leftFootFollow.Moving || _rightFootFollow.Moving;

    private void Update()
    {
        if (!EitherLegMoving && _leftLeg)
        {
            if (!_leftFootFollow.Moving && _leftFootFollow.DistanceToTarget > _leftFootFollow.MaxDistance)
            {
                _leftFootFollow.StartCoroutine(_leftFootFollow.MoveTowardsTarget(_rightFootFollow));
            }
            else
            if (!_rightFootFollow.Moving && _rightFootFollow.DistanceToTarget > _rightFootFollow.MaxDistance)
            {
                _rightFootFollow.StartCoroutine(_rightFootFollow.MoveTowardsTarget(_leftFootFollow));
            }
        }
        

    }

}
