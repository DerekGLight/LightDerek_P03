using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copyAnimationMovement : MonoBehaviour
{
    [SerializeField] private Transform _targetJoint;
    [SerializeField] private bool _ClampBackwardsMovement;

    ConfigurableJoint _thisJoint;
    Quaternion targetInitialRotation;

    // Start is called before the first frame update
    void Start()
    {
        _thisJoint = GetComponent<ConfigurableJoint>();
        targetInitialRotation = _targetJoint.transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _thisJoint.targetRotation = copyRotation();
    }

    private Quaternion copyRotation()
    {
        return Quaternion.Inverse(_targetJoint.localRotation) * targetInitialRotation;
    }
}
