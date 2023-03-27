using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootTargetController : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _floorMask;
    [SerializeField] private float _distanceFromMyCentre;
    [SerializeField] private float _distanceFromStartHeight;

    public float DistanceFromStartHeight => _distanceFromStartHeight;

    private void Start()
    {
        _distanceFromMyCentre = transform.localScale.x / 2f;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, _checkDistance, _floorMask))
        {
            transform.position = hit.point + (Vector3.up * _distanceFromMyCentre);
            _distanceFromStartHeight += transform.position.y;
        }
    }
}
