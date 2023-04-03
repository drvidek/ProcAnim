using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootTarget : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _floorMask;
    [SerializeField] private float _distanceFromMyCentre;
    [SerializeField] private IKLimb _limb;

    private Vector3 _startPosition;

    public IKLimb Limb => _limb;

    private void Start()
    {
        _distanceFromMyCentre = transform.localScale.x / 2f;
        _startPosition = transform.localPosition;
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

        Vector3 rayStart = transform.parent.position + _startPosition + (Vector3.up * 0.5f);
        if (_limb.IsRagdoll)
            rayStart.z -= _startPosition.z;


        RaycastHit hit;
        if (!Physics.CheckBox(rayStart, Vector3.one * 0.1f, Quaternion.identity, _floorMask) &&
            Physics.Raycast(rayStart, Vector3.down, out hit, float.PositiveInfinity, _floorMask))
        {
            transform.position = hit.point + (Vector3.up * _distanceFromMyCentre);
        }
    }
}
