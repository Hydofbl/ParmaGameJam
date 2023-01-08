using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Gun gun;

    PlayerControls controls;
    PlayerControls.PlayerInputActions playerInput;

    Vector2 movementInput;
    Vector2 mouseInput;
    bool isMoving;

    Coroutine fireCoroutine;

    private void Awake ()
    {
        controls = new PlayerControls();
        playerInput = controls.PlayerInput;

        // groundMovement.[action].performed += context => do something
        playerInput.HorizontalMovement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

        playerInput.Jump.performed += _ => playerMovement.OnJumpPressed();

        playerInput.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerInput.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        playerInput.Shoot.started += _ => StartFiring();
        playerInput.Shoot.canceled += _ => StopFiring();

        playerInput.Pull.started += _ => StartPickingUp();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update ()
    {
        playerMovement.ReceiveMovementInput(movementInput);
        playerMovement.ReceiveMouseInput(mouseInput);
    }

    void StartFiring()
    {
        fireCoroutine = StartCoroutine(gun.FireTeleport());
    }

    void StopFiring()
    {
        if (fireCoroutine != null) {
            StopCoroutine(fireCoroutine);
        }
    }

    void StartPickingUp()
    {
        gun.PickUp();
    }

    private void OnEnable ()
    {
        controls.Enable();
    }

    private void OnDestroy ()
    {
        controls.Disable();
    }
}