using System.Collections;
using UnityEngine;

public class IKLeg : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _maxDistance, _snappingDistance;
    [SerializeField] private AnimationCurve _yCurve;
    [SerializeField] private float _yMaxHeight;
    [SerializeField] bool _moving;
    Vector3 _currentPosition;

    public float DistanceToTarget => Vector3.Distance(_target.position, transform.position);
    public float MaxDistance => _maxDistance;
    public bool Moving => _moving;
    public Transform Target => _target;

    private void Start()
    {
        _currentPosition = transform.position;
    }

    private void Update()
    {
        if (!Moving)
        {
            transform.position = _currentPosition;
        }
    }

    public IEnumerator MoveTowardsTarget(IKLeg otherLeg)
    {
        _moving = true;
        Vector3 initPosition = transform.localPosition;
        while (Vector3.Distance(transform.localPosition, _target.localPosition) > _snappingDistance)
        {
            float lerp = Vector3.Distance(otherLeg.Target.position, otherLeg.transform.position)/(_maxDistance) + 0.01f;
            Vector3 nextPosition = Vector3.Lerp(initPosition, _target.localPosition, lerp);
            float yPos = _target.position.y + (_yCurve.Evaluate(lerp) * _yMaxHeight);
            nextPosition.y = yPos;
            transform.localPosition = nextPosition;
            if (lerp >= 1)
                break;

            yield return null;
        }

        transform.position = _target.position;
        _currentPosition = transform.position;
        _moving = false;
    }

}
