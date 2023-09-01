using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform target; // Reference to the character's Transform component.
    public float zoomSpeed = 5f; // The speed at which the camera zooms in.
    public float maxZoomSize = 5f; // The maximum orthographic size (zoom in) of the camera.
    public float followSpeed = 5f; // The speed at which the camera follows the character.

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Get the Main Camera in the scene.
    }

    private void LateUpdate()
    {
        if (target == null || mainCamera == null)
            return;

        // Calculate the distance between the character and the camera.
        float distance = Vector2.Distance(target.position, mainCamera.transform.position);

        // Calculate the desired orthographic size based on the distance and zoom speed.
        float desiredSize = Mathf.Lerp(maxZoomSize, mainCamera.orthographicSize, distance / zoomSpeed);

        // Limit the desired size to the max zoom size.
        desiredSize = Mathf.Clamp(desiredSize, maxZoomSize, mainCamera.orthographicSize);

        // Smoothly change the camera's orthographic size.
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, desiredSize, Time.deltaTime);

        // Smoothly follow the character.
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
