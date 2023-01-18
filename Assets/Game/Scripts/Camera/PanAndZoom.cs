using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanAndZoom : MonoBehaviour
{
    public float panSpeed { get => _panSpeed; private set => _panSpeed = value; }
    public float zoomSpeed { get => _zoomSpeed; private set => _zoomSpeed = value; }
    public float zoomInMax { get => _zoomInMax; private set => _zoomInMax = value; }
    public float zoomOutMax { get => _zoomOutMax; private set => _zoomOutMax = value; }


    [SerializeField] private float _panSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 15f;
    [SerializeField] private float _zoomInMax = 40f;
    [SerializeField] private float _zoomOutMax = 90f;

    private CinemachineInputProvider _inputProvider;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _cameraTransform;

    private void Awake()
    {
        // Get references to the Cinemachine components
        _inputProvider = GetComponent<CinemachineInputProvider>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraTransform = _virtualCamera.VirtualCameraGameObject.transform;

        // Set the initial zoom level
        _virtualCamera.m_Lens.OrthographicSize = zoomOutMax;
    }

    private void Update()
    {
        // Get the input axis values for panning and zooming

        float x = _inputProvider.GetAxisValue(0);
        float y = _inputProvider.GetAxisValue(1);
        float z = _inputProvider.GetAxisValue(2);
        if (x != 0 || y != 0)
        {
            PanScreen(x, y);
        }

        // If the z axis is not 0 and the mouse is not over a UI element, zoom the screen
        if (z != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            ZoomScreen(z);
        }


    }
    // Function to handle zooming the screen
    public void ZoomScreen(float increment)
    {
        // Get the current field of view and calculate the target field of view
        float fov = _virtualCamera.m_Lens.OrthographicSize;
        float target = Mathf.Clamp(fov + increment, zoomInMax, zoomOutMax);

        // Smoothly move towards the target field of view
        _virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(fov, target, zoomSpeed * Time.deltaTime);
    }

    // Function to determine the direction to pan based on the mouse position
    public Vector2 PanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;
        if (y >= Screen.height * .95f)
        {
            direction.y += 1;
        }
        else if (y <= Screen.height * 0.05f)
        {
            direction.y -= 1;
        }
        if (x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }
        return direction;
    }

    public void PanScreen(float x, float y)
    {
        Vector2 Direction = PanDirection(x, y);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
            _cameraTransform.position + (Vector3)Direction * panSpeed, Time.deltaTime);
    }
}
