using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;
using SimpleJSON;
using UnityEngine.Networking;
using System.Collections;
using System.Threading;

[EcsInject]
public class MainCameraSystem : IEcsInitSystem, IEcsRunSystem
{

    // TODO: move to share 
    private static int fieldSize = 34;

    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedMouse = 5f;
    private static readonly float RotateSpeed = 100f;

    private static readonly float[] BoundsX = new float[] { 0f, fieldSize * MapUtils.tileSize };
    private static readonly float[] BoundsY = new float[] { 0f, fieldSize };
    private static readonly float[] BoundsZ = new float[] { 0f, fieldSize * MapUtils.tileSize };
    private static readonly float[] ZoomBounds = new float[] { 10f, 85f };

    private Camera cam;
    private GameObject cameraContainer;

    private Vector3 lastPanPosition;

    private Vector3 lastRotatePosition;

    private Vector3 cameraPositionOffest;

    private float zoomScaling;


    void IEcsInitSystem.Initialize()
    {
        cameraContainer = GameObject.FindGameObjectWithTag("MainCamera");
        cam = cameraContainer.GetComponentInChildren<Camera>();
        zoomScaling = 1;
    }

    void IEcsInitSystem.Destroy()
    {
    }

    void IEcsRunSystem.Run()
    {

        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(1))
        {
            lastRotatePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            RotateCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Perform the movement
        var delta = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        cameraContainer.transform.Translate(
            (cameraContainer.transform.right * (-delta.y) * PanSpeed +
            cameraContainer.transform.forward * (delta.x) * PanSpeed) * zoomScaling,
            Space.World
        );

        // Ensure the camera remains within bounds.
        Vector3 pos = cameraContainer.transform.position;
        pos.x = Mathf.Clamp(cameraContainer.transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(cameraContainer.transform.position.z, BoundsZ[0], BoundsZ[1]);
        cameraContainer.transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }
        cameraContainer.transform.Translate(cameraContainer.transform.up * (-offset), Space.World);
        Vector3 pos = cameraContainer.transform.position;
        pos.y = Mathf.Clamp(cameraContainer.transform.position.y, BoundsY[0], BoundsY[1]);
        cameraContainer.transform.position = pos;
    }

    void RotateCamera(Vector3 newRotatePosition)
    {
        var delta = cam.ScreenToViewportPoint(lastRotatePosition - newRotatePosition);
        // Cache the position
        lastRotatePosition = newRotatePosition;
        cameraContainer.transform.Rotate(Vector3.up, delta.x * RotateSpeed);
        var xAngle = cam.transform.rotation.eulerAngles.x - delta.y * RotateSpeed;
        if (24 < xAngle && xAngle < 70)
        {
            cam.transform.Rotate(Vector3.right, -delta.y * RotateSpeed);
        }

    }
}
