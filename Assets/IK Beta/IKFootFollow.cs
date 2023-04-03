using System.Collections;
using UnityEngine;

public class IKFootFollow : MonoBehaviour
{
    [SerializeField] private IKFootTarget _target;
    [SerializeField] private float _maxDistanceFromTarget, _snappingDistance, _steptime;
    [SerializeField] private AnimationCurve _yCurve;
    [SerializeField] private float _stepHeight;
    [SerializeField] bool _isMoving;
    Vector3 _currentPosition;

    public float DistanceToTarget => Vector3.Distance(Target.position, transform.position);
    public float MaxDistance => _maxDistanceFromTarget;
    public bool IsMoving => _isMoving;
    public Transform Target => _target.transform;

    private void Start()
    {
        _currentPosition = transform.position;
    }

    private void Update()
    {
        if (!IsMoving)
        {
            if (!_target.Limb.IsRagdoll)
                transform.position = _currentPosition;
        }
    }

    private void FixedUpdate()
    {
        if (_target.Limb.IsRagdoll)
        {
            transform.position = Target.position;
            _currentPosition = transform.position;
        }
    }

    public IEnumerator MoveTowardsTarget(IKFootFollow otherLeg)
    {
        _isMoving = true;
        Vector3 initPosition = transform.position;
        Vector3 initUp = transform.up;
        for (int i = 1; i <= _steptime; ++i)
        {
            float lerp = i / (float)(_steptime + 1f);
            transform.position = Vector3.Lerp(initPosition, Target.position, lerp);
            transform.up = Vector3.Lerp(initUp, Target.up, lerp);
            transform.position += transform.up * Mathf.Sin(lerp * Mathf.PI) * _stepHeight;
            yield return new WaitForFixedUpdate();
        }

        // for (int i = 1; i <= _maxDistance; ++i)
        // {
        //     float lerp = Vector3.Distance(otherLeg.Target.position, otherLeg.transform.position)/(_maxDistance) + 0.01f;
        //     Vector3 nextPosition = Vector3.Lerp(initPosition, _target.position, lerp);
        //     nextPosition += Vector3.up * Mathf.Sin(i / (float)(_maxDistance + 0.01f) * Mathf.PI) * _stepHeight;
        //     transform.position = nextPosition;
        //     if (lerp >= 1)
        //         break;
        //
        //     yield return new WaitForFixedUpdate();
        // }
        transform.up = Target.up;
        transform.forward = PlayerController.main.transform.forward;
        transform.position = Target.position;
        _currentPosition = transform.position;
        _isMoving = false;
    }

}
