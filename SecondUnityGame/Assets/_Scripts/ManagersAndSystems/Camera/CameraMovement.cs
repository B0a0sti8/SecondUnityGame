using Unity.Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform cameraTarget;
    CinemachineCamera cinemachineCamera;

    bool useEdgeScrolling;

    float cameraSpeed;
    int edgeScrollSize;

    float targetFieldOfView;
    float foVmin, foVmax;



    private void Start()
    {
        cameraTarget = transform.Find("Target");
        cinemachineCamera = transform.Find("CinemachineCamera").GetComponent<CinemachineCamera>();

        useEdgeScrolling = false;

        cameraSpeed = 50f;
        edgeScrollSize = 20;

        targetFieldOfView = 35f;
        foVmin = 5f; foVmax = 40f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraZoom();
        HandleCameraMovement();
    }

    void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            Debug.Log(Input.mouseScrollDelta.y);

            if (Input.mouseScrollDelta.y > 0) targetFieldOfView += -1;
            if (Input.mouseScrollDelta.y < 0) targetFieldOfView += 1;

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, foVmin, foVmax);
            Debug.Log(targetFieldOfView);

            cinemachineCamera.Lens.OrthographicSize = targetFieldOfView;
        }
    }

    void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        // Steuerung der Kamera mit WASD oder Zeigern
        if (Input.GetKey(KeyCode.W)) inputDir.y = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.y = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        if (Input.GetKey(KeyCode.UpArrow)) inputDir.y = +1f;
        if (Input.GetKey(KeyCode.DownArrow)) inputDir.y = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) inputDir.x = +1f;

        // Steuerung der Kamera mit Maus am Bildschirmrand
        if (useEdgeScrolling)
        {
            if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
            if (Input.mousePosition.y < edgeScrollSize) inputDir.y = -1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.y = +1f;
        }

        // Bewegen des Kamera Targets --> Kamera läuft automatisch hinterher (siehe Cinemachine)
        Vector3 movementDir = cameraSpeed * Time.deltaTime * inputDir;
        cameraTarget.position += movementDir;
    }
}
