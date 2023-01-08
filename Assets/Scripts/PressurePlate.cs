using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    private bool _isPressed;
    private Animator pressPlateAnim;
    private int activatorCount;

    public bool IsPressed => _isPressed;

    void Start()
    {
        pressPlateAnim = transform.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            _isPressed = true;
            if(!pressPlateAnim.GetBool("isPlatePressed"))
                pressPlateAnim.SetBool("isPlatePressed", _isPressed);

            activatorCount++;
            doorController.CheckDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            activatorCount--;
            if(activatorCount == 0)
                _isPressed = false;

            pressPlateAnim.SetBool("isPlatePressed", _isPressed);
            doorController.CheckDoor();
        }
    }
}
