using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] doorActivatorsGO;
    Animator doorAnim;
    public bool isOpen;

    private void Start()
    {
        doorAnim = GetComponent<Animator>();
        CheckDoor();
    }

    public void CheckDoor()
    {
        isOpen = CanDoorOpen();
        doorAnim.SetBool("isDoorOpened", isOpen);
    }

    private bool CanDoorOpen()
    {
        foreach(GameObject obj in doorActivatorsGO)
        {
            if(obj.TryGetComponent(out PressurePlate pressurePlate))
            {
                if (!pressurePlate.IsPressed)
                    return false;
            }
            else if(obj.TryGetComponent(out KeyHole keyHole))
            {
                if (!keyHole.IsActive)
                    return false;
            }
        }

        return true;
    }
}
