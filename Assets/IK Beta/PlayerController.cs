using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private Transform _leftTarget, _rightTarget;

    private void Update()
    {
        float vertAxis = Input.GetAxis("Vertical");
        if (vertAxis != 0)
        {
            _leftTarget.transform.localPosition = QMath.ReplaceVectorValue(_leftTarget.transform.localPosition, VectorValue.z, Mathf.Abs(_leftTarget.transform.localPosition.z) * Mathf.Sign(vertAxis));
            _rightTarget.transform.localPosition = QMath.ReplaceVectorValue(_rightTarget.transform.localPosition, VectorValue.z, Mathf.Abs(_rightTarget.transform.localPosition.z) * Mathf.Sign(vertAxis));
            transform.position += new Vector3(0, 0, Input.GetAxis("Vertical")) * _walkSpeed * Time.deltaTime;
        }
    }
}
