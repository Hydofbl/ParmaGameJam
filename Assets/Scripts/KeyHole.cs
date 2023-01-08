using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private GameObject keyBall;
    private bool _isActive;

    public bool IsActive => _isActive;

    public void getKeyBall()
    {
        KeyHoleTrigger(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyBall") && !_isActive)
        {
            KeyHoleTrigger(true);
            Destroy(other.gameObject);
        }
    }

    void KeyHoleTrigger(bool activity)
    {
        _isActive = activity;
        keyBall.SetActive(activity);
        doorController.CheckDoor();
    }
}
