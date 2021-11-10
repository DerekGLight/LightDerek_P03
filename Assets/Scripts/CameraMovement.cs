using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;
    [SerializeField] Transform _brainCage;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //if (_level1Controll._isInPauseMenu != true)
        //{
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            _brainCage.Rotate(Vector3.up * mouseX);
            Cursor.visible = false;
        //}
        //else
        //{
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        //}
    }
}