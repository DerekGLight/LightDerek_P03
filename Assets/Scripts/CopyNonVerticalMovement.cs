using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyNonVerticalMovement : MonoBehaviour
{
    [SerializeField] Transform _objectToFollow;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(_objectToFollow.position.x, transform.position.y, _objectToFollow.position.z);
        transform.position = newPosition;

        //Quaternion newRotation = _objectToFollow.rotation;
        //transform.rotation = newRotation;
    }
}
