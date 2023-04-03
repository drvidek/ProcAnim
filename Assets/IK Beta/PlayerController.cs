using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Walk }

    [SerializeField] private float _walkSpeed, _turnSpeed;
    [SerializeField] private Transform _leftFootTarget, _rightFootTarget, _leftKneeTarget, _rightKneeTarget;
    [SerializeField] private IKLegPair _legs;

    bool _shiftingHeight;

    private void FixedUpdate()
    {
        float vertAxis = Input.GetAxis("Vertical");
        if (vertAxis > 0)
        {
            Vector3 moveDir = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            transform.position += moveDir * _walkSpeed * Time.deltaTime;
        }

        float horAxis = Input.GetAxis("Horizontal");
        if (horAxis != 0)
        {
            transform.Rotate(0, horAxis * _turnSpeed * Time.deltaTime, 0);
        }

       // float height = (_legs.LeftFollow.Target.position.y + _legs.LeftFollow.Target.position.y) / 2;
       // Debug.Log($"{_legs.LeftFollow.Target.position.y} + {_legs.LeftFollow.Target.position.y} ({_legs.LeftFollow.Target.position.y + _legs.LeftFollow.Target.position.y}) / 2 = {height}");
       // transform.position = QMath.ReplaceVectorValue(transform.position, VectorValue.y, height);
    }
}
