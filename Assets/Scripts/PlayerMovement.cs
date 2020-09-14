using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject player, camera, light;
    public CharacterController playerController;

    public float lookSensitivty, moveSpeed;

    float xRotation = 0F;


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivty * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivty * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90F, 90F);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        light.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.transform.Rotate(new Vector3(0, 1*mouseX, 0));

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveVector = player.transform.right * moveX * Time.deltaTime * moveSpeed 
        + player.transform.forward * moveZ * Time.deltaTime * moveSpeed;
        playerController.Move(moveVector);

    }
}
