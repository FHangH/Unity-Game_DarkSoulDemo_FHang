using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed = 80.0f;
    public float verticalSpeed = 60.0f;
    public float tempEulerX;
    public float cameraDampValue = 0.1f;
    public float cameraDampMaxSpeed = 15.0f;
    public Vector3 cameraDampVelocity;
    private GameObject cameraHandle;
    private GameObject playerHandle;
    private GameObject model;
    private new GameObject camera;
    private Vector3 tempModelEuler;

    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 0;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        tempModelEuler = model.transform.eulerAngles;

        playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
        //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * verticalSpeed * Time.deltaTime);
        tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(-tempEulerX, 0, 0);

        model.transform.eulerAngles = tempModelEuler;

        //camera.transform.position = transform.position;
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue, cameraDampMaxSpeed, Time.fixedDeltaTime);
        camera.transform.eulerAngles = transform.eulerAngles;
    }
}
