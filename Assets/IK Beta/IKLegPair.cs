using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegPair : MonoBehaviour
{
    [SerializeField] private IKFootFollow _leftFootFollow, _rightFootFollow;
    [ SerializeField] private IKLimb _leftLeg, _rightLeg;
    public IKFootFollow LeftFollow => _leftFootFollow;
    public IKFootFollow RightFollow => _rightFootFollow;

    public bool EitherLegMoving => _leftFootFollow.IsMoving || _rightFootFollow.IsMoving;

    private void Update()
    {
        if (!EitherLegMoving)
        {
            if (!_leftFootFollow.IsMoving && _leftFootFollow.DistanceToTarget > _leftFootFollow.MaxDistance && !_leftLeg.IsRagdoll)
            {
                _leftFootFollow.StartCoroutine(_leftFootFollow.MoveTowardsTarget(_rightFootFollow));
            }
            else
            if (!_rightFootFollow.IsMoving && _rightFootFollow.DistanceToTarget > _rightFootFollow.MaxDistance && !_rightLeg.IsRagdoll)
            {
                _rightFootFollow.StartCoroutine(_rightFootFollow.MoveTowardsTarget(_leftFootFollow));
            }
        }
        

    }

}
