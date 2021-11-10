using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderMovement : MonoBehaviour
{
    [SerializeField] CharacterController _fleshPrison;
    [SerializeField] Collider _mapCollider;

    public float _speed;
    public float _verticalSpeedWeight;

    private bool _isInMap;
    private Vector3 origin = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distFromZero = Vector3.Distance(origin, transform.position);
        if (transform.position.y <= 0 && distFromZero <= 28) 
        {
            _mapCollider.enabled = true;
            _isInMap = true;
        }
        else
        {
            _mapCollider.enabled = false;
            _isInMap = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;


        if (Input.GetButton("Jump"))
        {
            move.y = 1 * _verticalSpeedWeight;
        }        
        if (Input.GetKey(KeyCode.LeftControl) && _isInMap == false)
        {
            if(transform.position.y - 1 > 0 || distFromZero >= 28)
            {
                move.y = -1 * _verticalSpeedWeight;
            }
        }
        _fleshPrison.Move(move * _speed * Time.deltaTime);//movement apply
    }
}
