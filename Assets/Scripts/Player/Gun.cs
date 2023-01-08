using UnityEngine;
using System.Collections;
using System.Threading;

public class Gun : MonoBehaviour {

    Transform cam;
    [SerializeField] PlayerMovement playerMovement;
    [Header("LayerMasks")]
    [SerializeField] private string teleportableLayerName;
    [SerializeField] private LayerMask pullableObjectLayer;

    [Header("General Stats")]
    [SerializeField] float range = 50f;
    [SerializeField] float reloadTime;
    WaitForSeconds reloadWait;
    [SerializeField] float offsetDistance = 1.5f;

    [Header("TeleportLaser")]
    [SerializeField] MeshRenderer mRTeleport;
    [SerializeField] Transform muzzle;
    [SerializeField] bool canTeleport;

    [Header("Pulling")]
    [Header("Pickup Settings")]
    [SerializeField] MeshRenderer mRPickup;
    [SerializeField] bool canPickup;
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Awake ()
    {
        cam = Camera.main.transform;
        reloadWait = new WaitForSeconds(reloadTime);
    }

    private void Start()
    {
        ChangeMatColor(mRPickup, Color.green);
        ChangeMatColor(mRTeleport, Color.green);
        canTeleport = true;
        canPickup = true;
    }

    private void Update()
    {
        if (heldObj != null)
        {
            // MoveObject
            MoveObject();
        }
    }

    public void ShootTeleport ()
    {
        RaycastHit hit;
        Vector3 shootingDir = GetShootingDirection();
        if (Physics.Raycast(cam.position, shootingDir, out hit, range))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(teleportableLayerName))
            {
                Debug.Log("Hit Teleportable");
                playerMovement.Teleport(hit.point + hit.normal * offsetDistance);
                ChangeMatColor(mRTeleport, Color.red);
            }
            else
            {
                Debug.Log("Other obstacles - " + hit.collider.gameObject.layer);
            }
        }
        else
        {
            Debug.Log("Didn't hit");
            //CreateLaser(cam.position + shootingDir * range);
        }

        canTeleport = false;
    }

    public void PickUp()
    {
        if (heldObj == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, pullableObjectLayer))
            {
                // Pickup
                PickupObject(hit.transform.gameObject);

                canPickup = false;
                canTeleport = false;

                ChangeMatColor(mRPickup, Color.red);
                ChangeMatColor(mRTeleport, Color.red);
            }
        }
        else
        {
            // Drop
            DropObject();

            canPickup = true;
            canTeleport = true;

            ChangeMatColor(mRPickup, Color.green);
            ChangeMatColor(mRTeleport, Color.green);
        }
    }

    public IEnumerator FireTeleport()
    {
        if (canTeleport) {
            ShootTeleport();
            StartCoroutine(Reload());
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator Reload()
    {
        if (canTeleport) 
        {
            yield return null;
        }
        print("reloading...");
        yield return reloadWait;
        canTeleport = true;

        ChangeMatColor(mRTeleport, Color.green);
        print("finished reloading");
        yield return null;
    }

    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        Vector3 direction = targetPos - cam.position;

        return direction.normalized;
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldRB = pickObj.GetComponent<Rigidbody>();
            heldRB.useGravity = false;
            heldRB.drag = 10;
            heldRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDir = (holdArea.position - heldObj.transform.position);
            heldRB.AddForce(moveDir * pickupForce);
        }
    }

    void DropObject()
    {
        heldRB.useGravity = true;
        heldRB.drag = 1;
        heldRB.constraints = RigidbodyConstraints.None;

        heldObj.transform.parent = null;
        heldObj = null;
    }

    void ChangeMatColor(MeshRenderer mr, Color color)
    {
        mr.material.color = color;
    }
}