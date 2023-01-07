using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
#pragma warning disable 649

    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Gun gun;

    PlayerControls controls;
    PlayerControls.PlayerInputActions playerInput;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    Coroutine fireCoroutine;
    Coroutine pullCoroutine;

    private void Awake ()
    {
        controls = new PlayerControls();
        playerInput = controls.PlayerInput;

        // groundMovement.[action].performed += context => do something
        playerInput.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        playerInput.Jump.performed += _ => movement.OnJumpPressed();

        playerInput.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerInput.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        playerInput.Shoot.started += _ => StartFiring();
        playerInput.Shoot.canceled += _ => StopFiring();

        playerInput.Pull.started += _ => StartPulling();
        playerInput.Pull.canceled += _ => StopPulling();
    }

    private void Update ()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
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

    void StartPulling()
    {
        gun.PullObject();
    }

    void StopPulling()
    {
        if (pullCoroutine != null)
        {
            StopCoroutine(pullCoroutine);
        }
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