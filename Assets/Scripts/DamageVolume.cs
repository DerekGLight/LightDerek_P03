using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    Rigidbody _rigidBody;
    public float _handVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _handVelocity = _rigidBody.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform _collidedRagdollTransform = collision.transform.root;
        ragdollBehavior _collidedRagdollScript = _collidedRagdollTransform.GetComponent<ragdollBehavior>();
        if(_collidedRagdollTransform.tag != transform.root.tag)
        {
            _collidedRagdollScript.TakeDamage(_handVelocity);
        }
    }
}
