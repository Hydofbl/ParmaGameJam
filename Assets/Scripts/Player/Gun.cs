using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

    Camera cam => Camera.main;
    Transform camTransform => cam.transform;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Image crosshair;
    [Header("LayerMasks")]
    [SerializeField] private string teleportableLayerName;
    [SerializeField] private string pullableObjectLayer;

    [Header("General Stats")]
    [SerializeField] float range = 50f;
    [SerializeField] float reloadTime;
    WaitForSeconds reloadWait;
    [SerializeField] float offsetDistance = 1.5f;

    [Header("TeleportLaser")]
    [SerializeField] MeshRenderer mRTeleport;
    [SerializeField] Transform muzzle;
    [SerializeField] bool canTeleport;

    [Header("Pickup Settings")]
    [Header("Cube")]
    [SerializeField] MeshRenderer mRPickup;
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldRB;

    [Header("KeyBall")]
    [SerializeField] private Animator GunBallHolderAnim;
    [SerializeField] GameObject keyBallPref;
    [SerializeField] float ballShootSpeed = 1f;
    [SerializeField] bool canGetKeyBall;
    [SerializeField] bool isHoldingObject;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;


    float xScreen = Screen.width / 2;
    float yScreen = Screen.height / 2;

    Ray ray => cam.ScreenPointToRay(new Vector3(xScreen, yScreen, 0));

    private void Awake ()
    {
        reloadWait = new WaitForSeconds(reloadTime);
    }

    private void Start()
    {
        ChangeMatColor(mRPickup, Color.white);
        ChangeMatColor(mRTeleport, Color.green);
        canTeleport = true;
        canGetKeyBall = true;
        isHoldingObject = false;
    }

    private void Update()
    {
        if (heldObj != null)
        {
            // MoveObject
            MoveObject();
        }

        if (Physics.Raycast(camTransform.position, ray.direction, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(pullableObjectLayer))
                crosshair.color = Color.blue;
            else
                crosshair.color = Color.white;
        }
    }

    public void ShootTeleport ()
    {
        RaycastHit hit;
        Vector3 shootingDir = GetShootingDirection();
        if (Physics.Raycast(camTransform.position, shootingDir, out hit, range))
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

    public void ShootKeyBall()
    {
        // Parent eklenebilir.
        GameObject keyBallObj = Instantiate(keyBallPref, muzzle.position, Quaternion.identity);

        // Direction'u ayarla
        keyBallObj.GetComponent<Rigidbody>().AddForce(ray.direction * ballShootSpeed, ForceMode.Impulse);

        canGetKeyBall = true;
        isHoldingObject = false;

        ChangeMatColor(mRPickup, Color.white);
    }

    public void PickUp()
    {
        if (heldObj == null)
        {

            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, ray.direction, out hit, pickupRange))
            {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer(pullableObjectLayer))
                {
                    GameObject hitObj = hit.collider.gameObject;
                    if (hitObj.CompareTag("Cube"))
                    {
                        // Pickup
                        PickupObject(hit.transform.gameObject);

                        canTeleport = false;
                        isHoldingObject = true;

                        ChangeMatColor(mRTeleport, Color.red);
                    }
                    else if (hitObj.CompareTag("KeyBall") && canGetKeyBall)
                    {

                        if (hitObj.GetComponent<KeyBallChecker>().onKeyHole)
                        {
                            KeyHole keyHoleScr = hitObj.transform.GetComponentInParent<KeyHole>();
                            if (keyHoleScr.IsActive)
                            {
                                // Get Keyball
                                canGetKeyBall = false;
                                ChangeMatColor(mRPickup, Color.blue);
                                keyHoleScr.getKeyBall();
                            }
                        }
                        else
                        {
                            // Get Keyball
                            canGetKeyBall = false;
                            ChangeMatColor(mRPickup, Color.blue);
                            Destroy(hitObj);
                            // Set Animations
                        }

                        isHoldingObject = true;
                    }
                    else if (hitObj.CompareTag("KeyBall") && !canGetKeyBall)
                    {
                        ShootKeyBall();
                    }
                }
            }
        }
        else
        {
            // Drop
            DropObject();

            canTeleport = true;
            isHoldingObject = false;

            ChangeMatColor(mRTeleport, Color.green);
        }
        GunBallHolderAnim.SetBool("isHoldingObject", isHoldingObject);
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
        Vector3 targetPos = camTransform.position + camTransform.forward * range;
        Vector3 direction = targetPos - camTransform.position;

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