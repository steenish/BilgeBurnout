using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float lookSpeed = 1.0f;


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        float rotationX = -Input.GetAxis("Mouse Y") * Time.deltaTime * lookSpeed;
        float rotationY = Input.GetAxis("Mouse X") * Time.deltaTime * lookSpeed;
        transform.localRotation = transform.localRotation * Quaternion.Euler(rotationX, rotationY, 0.0f);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);

        float forwardMovement = Input.GetAxis("Vertical");
        float rightMovement = Input.GetAxis("Horizontal");

        transform.position += forwardMovement * transform.forward * Time.deltaTime * moveSpeed;
        transform.position += rightMovement * transform.right * Time.deltaTime * moveSpeed;

        if (Input.GetKey(KeyCode.Space)) {
            transform.position += transform.up * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            transform.position -= transform.up * Time.deltaTime * moveSpeed;
        }
    }
}
