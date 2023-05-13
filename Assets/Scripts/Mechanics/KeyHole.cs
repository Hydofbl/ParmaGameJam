using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private HoverPower hoverPower;
    [SerializeField] private MeshRenderer keyBallMR;
    [SerializeField] private Material matOpq;
    [SerializeField] private Material matTrans;

    public bool IsActive;

    private void Awake()
    {
        keyBallMR.material = IsActive ? matOpq : matTrans;
    }

    public void getKeyBall()
    {
        KeyHoleTrigger(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyBall") && !IsActive)
        {
            KeyHoleTrigger(true);
            Destroy(other.gameObject);
        }
    }

    void KeyHoleTrigger(bool activity)
    {
        IsActive = activity;
        keyBallMR.material = activity ? matOpq : matTrans;

        if (doorController != null)
            doorController.CheckDoor();
        else
            hoverPower.CheckIsActive();
    }
}
