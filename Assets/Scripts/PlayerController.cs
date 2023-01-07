using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private LayerMask teleportableLayer;
    [SerializeField] private LayerMask pullableObjectLayer;

    #region Configuration

    [Header("FPS Controller Properties")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController characterController;

    private Vector2 moveInput;
    private Vector2 mouseInput;
    private Vector3 moveVector;
    private float verticalLookRotation;

    private FirstPersonInputAction _controls;

    #endregion

    private void OnEnable()
    {
        _controls.Player.Shoot.performed += OnShoot;
        _controls.Player.Pull.performed += OnPull;
        _controls.Player.Move.performed += OnMove;
        _controls.Player.Look.performed += OnLook;
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Shoot.performed -= OnShoot;
        _controls.Player.Pull.performed -= OnPull;
        _controls.Player.Move.performed -= OnMove;
        _controls.Player.Look.performed -= OnLook;
        _controls.Disable();
    }

    protected void Awake()
    {
        _controls = new FirstPersonInputAction();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Calculate move vector
        moveVector = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;

        // Rotate player based on mouse input
        transform.Rotate(Vector3.up, mouseInput.x * mouseSensitivity * Time.deltaTime);

        // Rotate camera based on mouse input
        verticalLookRotation += mouseInput.y * mouseSensitivity * Time.deltaTime;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
        playerCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;

        // Move the player using CharacterController
        characterController.Move(moveVector * Time.deltaTime);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    private void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, teleportableLayer))
        {
            Debug.DrawLine(muzzleTransform.position, hit.point, Color.red, 20f);

            Debug.Log(hit.normal);
            transform.position = hit.point + hit.normal * 2;
        }
        else
        {
            // Can't Teleport message or Animation
        }
    }

    private void OnPull(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, pullableObjectLayer))
        {
            Debug.DrawLine(muzzleTransform.position, hit.point, Color.blue, 20f);
            // Pull
            Debug.Log("PULLING");
        }
        else
        {
            // Can't Pull message or Animation
        }
    }
}
