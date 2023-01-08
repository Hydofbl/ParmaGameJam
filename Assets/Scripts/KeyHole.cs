using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private Material transparentMat;
    [SerializeField] private Material opaqueMat;
    [SerializeField] private MeshRenderer mRBall;
    private bool _isActive;

    public bool IsActive => _isActive;

    private void Start()
    {
        mRBall.material = transparentMat;
    }

    public void getKeyBall()
    {
        KeyHoleTrigger(false, transparentMat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyBall") && !_isActive)
        {
            KeyHoleTrigger(true, opaqueMat);
            Destroy(other.gameObject);
        }
    }

    void KeyHoleTrigger(bool activity, Material mat)
    {
        _isActive = activity;
        mRBall.material = mat;
        doorController.CheckDoor();
    }
}
