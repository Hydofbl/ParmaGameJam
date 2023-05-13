using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private HoverPower howerPower;

    private void Start()
    {
        howerPower = GetComponent<HoverPower>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(howerPower.isActive)
            {
                Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();

                playerRB.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
