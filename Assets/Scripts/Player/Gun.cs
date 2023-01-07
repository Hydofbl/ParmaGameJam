using UnityEngine;
using System.Collections;
using System.Threading;

public class Gun : MonoBehaviour {

    Transform cam;
    [SerializeField] Movement movement;
    [Header("LayerMasks")]
    [SerializeField] private LayerMask teleportableLayer;
    [SerializeField] private LayerMask pullableObjectLayer;

    [Header("General Stats")]
    [SerializeField] float range = 50f;
    [SerializeField] float reloadTime;
    WaitForSeconds reloadWait;
    [SerializeField] float offsetDistance = 1.5f;

    [Header("TeleportLaser")]
    [SerializeField] GameObject laser;
    [SerializeField] Transform muzzle;
    [SerializeField] float fadeDuration = 0.3f;
    [SerializeField] float destroyDuration = 0.8f;
    [SerializeField] bool canTeleport;

    [Header("Pulling")]
    [SerializeField] bool canPull;

    private void Awake ()
    {
        cam = Camera.main.transform;
        reloadWait = new WaitForSeconds(reloadTime);
    }

    private void Start()
    {
        canTeleport = true;
    }

    public void ShootTeleport ()
    {
        RaycastHit hit;
        Vector3 shootingDir = GetShootingDirection();
        if (Physics.Raycast(cam.position, shootingDir, out hit, range, teleportableLayer))
        {
            Debug.Log("Hit Teleportable");
            CreateLaser(hit.point); 
            movement.Teleport(hit.point + hit.normal * offsetDistance);
        }
        else
        {
            Debug.Log("Didn't hit");
            CreateLaser(cam.position + shootingDir * range);
        }

        canTeleport = false;
    }

    public void Pull()
    {
        RaycastHit hit;
        Vector3 shootingDir = GetShootingDirection();
        if (Physics.Raycast(cam.position, shootingDir, out hit, range, pullableObjectLayer))
        {
            Debug.Log("Hit Pullable");
            CreateLaser(hit.point);

        }
        else
        {
            Debug.Log("Didn't hit");
        }

        canTeleport = false;
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

    public IEnumerator PullObject()
    {
        if (canPull)
        {

        }
        else
        {
            yield return null;
        }
    }

    IEnumerator Reload()
    {
        if (canTeleport) {
            yield return null;
        }
        print("reloading...");
        yield return reloadWait;
        canTeleport = true; ;
        print("finished reloading");
        yield return null;
    }

    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        Vector3 direction = targetPos - cam.position;

        return direction.normalized;
    }

    void CreateLaser(Vector3 end)
    {
        GameObject laserObj = Instantiate(laser);
        LineRenderer lr = laserObj.GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.position, end });
        StartCoroutine(FadeLaser(lr));
        Destroy(laserObj, destroyDuration);
    }

    IEnumerator FadeLaser(LineRenderer lr)
    {
        float alpha = 1;
        while (alpha > 0) {
            alpha -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
            yield return null;
        }
        Destroy(lr);
    }
}