using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

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

        cameraSpeed = 200f;
        edgeScrollSize = 50;

        targetFieldOfView = 60f;
        foVmin = 30f; foVmax = 70f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovement();
    }

    public void HandleCameraZoom(float value)
    {
        if (Input.mouseScrollDelta.y == 0) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        targetFieldOfView += value;

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, foVmin, foVmax);

        cinemachineCamera.Lens.OrthographicSize = targetFieldOfView;

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

        // Bewegen des Kamera Targets --> Kamera l�uft automatisch hinterher (siehe Cinemachine)
        Vector3 movementDir = cameraSpeed * Time.deltaTime * inputDir;
        cameraTarget.position += movementDir;
    }
}
