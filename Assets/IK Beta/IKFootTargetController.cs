using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootTargetController : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _floorMask;
    [SerializeField] private float _distanceFromMyCentre;
    [SerializeField] private IKLimb _limb;

    private void Start()
    {
        //_distanceFromMyCentre = transform.localScale.x / 2f;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, float.PositiveInfinity, _floorMask))
        {
            transform.position = hit.point;// + (Vector3.up * _distanceFromMyCentre);
            transform.up = hit.normal;
        }

        if (Vector3.Distance(transform.position, _limb.Target.transform.position) <= _limb.LimbLength * 1.01f)
        {
            //ragdoll leg
            _limb.ToggleKinematicLimb(true);
        }
        else
        {
            _limb.ToggleKinematicLimb(false);
        }
    }
}
