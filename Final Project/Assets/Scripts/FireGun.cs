using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FireGun : MonoBehaviour
{
    [SerializeField]
    private GameObject muzzleFlashObject;

    [SerializeField]
    private Transform firePointObject;

    [SerializeField]
    private int RecoilForce = 20;

    [SerializeField]
    private AudioSource shootSound;

    private LineRenderer bulletPath;
    private Light muzzleFlashLight;
    private Vector3[] points = new Vector3[2];
    private Vector3 origin;
    private Vector3 direction;

    private bool isAiming = false;

    private bool recoilActive = false;

    private void Start()
    {
        bulletPath = GetComponent<LineRenderer>();
        muzzleFlashLight = muzzleFlashObject.GetComponent<Light>();
        shootSound = GetComponent<AudioSource>();
        muzzleFlashLight.enabled = false;
        bulletPath.enabled = false;
    }

    private void Update()
    {
        if (!recoilActive)
        {
            origin = firePointObject ? firePointObject.position : transform.position;
            if (UnityEngine.XR.XRSettings.isDeviceActive)
                direction = firePointObject ? firePointObject.forward : transform.forward;
            else
            {
                direction = Camera.main.transform.forward;
            }
        }

        points[0] = origin;
        points[1] = origin + direction * 50f;

        bulletPath.SetPositions(points);
    }

    /// <summary>
    /// When attack is triggered from a weapon, the object will fire out a line renderer to act as a bullet path.
    /// This will calculate what is hit.
    /// On Attack, a recoil animation, gunshot sound, and muzzle flash will also be triggered for 0.5s
    /// </summary>
    /// <param name="collider"></param>
    public void OnAttack(Collider collider)
    {
        //if its a sniper, have to be aiming
        if (IsSniper() && !isAiming)
        {
            return;
        }
        //if it's a sniper, have to re aim every time
        else if (IsSniper())
        {
            isAiming = false;
        }

        // play fire audio if exists
        if(shootSound != null)
            shootSound.Play();
        bulletPath.enabled = true; // bullet path
        muzzleFlashLight.enabled = true; // muzzle flash
        Invoke(nameof(DisableLine), 0.05f);

        // Check if bullet has hit anything
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit))
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Target"))
            {
                Console.WriteLine("hit");
                target.GetComponent<TargetBehavior>().GotHit();
                GameManager.Instance.AddScore(1);
                GameManager.Instance.AddBank(1);
            }

            if (target.CompareTag("Button"))
            {
                //GameManager.Instance.EndGame();
                //GameManager.Instance.StartGame();
                target.GetComponent<Button>().onClick.Invoke();
            }
        }

        // Add recoil if it is not already in a recoiling state
        if (!recoilActive)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - RecoilForce, transform.localEulerAngles.y - RecoilForce, transform.localEulerAngles.z);
            recoilActive = true;
        }
        Invoke(nameof(ResetRecoil), 0.1f); // reset recoil after 0.1s
    }

    /// <summary>
    /// On Aim function invoked on left trigger or right click press.
    /// Returns if not two handed, as this is only a two handed interaction.
    /// Sets the isAiming variable to true and shows the bullet path.
    /// </summary>
    /// <param name="collider"></param>
    public void OnAim(Collider collider)
    {
        if (!IsTwoHanded()) return;

        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            // in VR, only allow aiming if left hand is grabbing
            if (!LeftHandGrab.IsGrabbingGrip) return;
        }

        isAiming = true;
        bulletPath.enabled = true;
    }

    /// <summary>
    /// On Aim Cancel function invoked when the user stops aiming.
    /// Does not do anything for non-two-handed interactions.
    /// Sets isAiming to false and disables the bullet path.
    /// </summary>
    /// <param name="collider"></param>
    public void OnAimCancel(Collider collider)
    {
        if (!IsTwoHanded()) return;

        isAiming = false;
        bulletPath.enabled = false;
    }

    /// <summary>
    /// Helper function to see if a gun should be using two handed interactions.
    /// Two-handed guns have a left hand grip, while others do not.
    /// </summary>
    /// <returns></returns>
    private bool IsTwoHanded()
    {
        return transform.GetComponentsInChildren<Transform>(true)
                        .Any(t => t.name == "LeftHandGrip");
    }

    /// <summary>
    /// Helper function to see if the gun is a sniper. 
    /// Snipers have special two handed interactions.
    /// A sniper must be aiming to shoot and stops aiming after any shot.
    /// </summary>
    /// <returns></returns>
    private bool IsSniper()
    {
        return gameObject.name.Contains("Sniper");
    }

    /// <summary>
    /// Helper function to disable the bullet bath. 
    /// If the user is not continuously aiming, disables bullet path after shot.
    /// Sets muzzle flash to off after a shot.
    /// </summary>
    private void DisableLine()
    {
        if (!isAiming)
        {
            bulletPath.enabled = false;
        }
        muzzleFlashLight.enabled = false;
    }

    /// <summary>
    /// Helper function to reset gun after recoil has been triggered.
    /// Returns orientation to original settings.
    /// </summary>
    private void ResetRecoil()
    {
        if (recoilActive)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + RecoilForce, transform.localEulerAngles.y + RecoilForce, transform.localEulerAngles.z);
            recoilActive = false;
        }
    }
}
