using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootTarget : MonoBehaviour
{
    [SerializeField] private bool _rightFoot;
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _floorMask;
    [SerializeField] private float _distanceFromMyCentre;
    [SerializeField] private float _halfStepDistance = 0.25f, _halfSholderWidth = 0.11f, _maxStepHeight = 1f;
    [SerializeField] private IKLimb _limb;

    public IKLimb Limb => _limb;

    private void Start()
    {
        _distanceFromMyCentre = transform.localScale.x / 2f;
    }

    private void FixedUpdate()
    {
        //if (_limb.IsRagdoll)
        //{
        //    transform.localPosition = QMath.ReplaceVectorValue(_startPosition, VectorValue.z, 0);
        //}
        //else
        //if (transform.localPosition.z == 0)
        //{
        //    transform.localPosition = _startPosition;
        //}

        Vector3 rayStart = transform.parent.position + (Vector3.up * _maxStepHeight);
        if (!_limb.IsRagdoll)
        {
            rayStart += PlayerController.main.AverageDirection * _halfStepDistance;
        }
        rayStart += PlayerController.main.Hips.TransformDirection(Vector3.right) * _halfSholderWidth * (_rightFoot ? 1 : -1);
        //rayStart = transform.TransformDirection(rayStart);

        RaycastHit hit;
        if (!Physics.CheckBox(rayStart, Vector3.one * 0.1f, Quaternion.identity, _floorMask) &&
            Physics.SphereCast(rayStart, transform.localScale.x/2, Vector3.down, out hit, float.PositiveInfinity, _floorMask))
        {
            transform.position = hit.point + (Vector3.up * _distanceFromMyCentre);
            transform.up = hit.normal;
        }
    }
}
