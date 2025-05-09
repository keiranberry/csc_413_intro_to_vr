using UnityEngine;

public class Animations : MonoBehaviour
{
    public Vector3 growTargetScale = new Vector3(1, 1, 1);
    public Vector3 shrinkTargetScale = new Vector3(0, 0, 0);
    public Quaternion rotateTarget = Quaternion.Euler(0, 180, 0);
    public float scaleSpeed = 0.1f;

    // Booleans for each animation possible
    private bool isGrowing = false;
    private bool isShrinking = false;
    private bool isSpawning = false;

    /// <summary>
    /// Update to run key frame animations.
    /// Growing and shrinking cannot take place at the same time, 
    /// but spinning and either growing and shrinking should be fine.
    /// </summary>
    private void Update()
    {
        if (isGrowing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, growTargetScale, scaleSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.localScale, growTargetScale) < 0.01f)
            {
                transform.localScale = growTargetScale;
                isGrowing = false;
            }
        }

        if (isShrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, shrinkTargetScale, scaleSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.localScale, growTargetScale) < 0.01f)
            {
                transform.localScale = shrinkTargetScale;
                isShrinking = false;

                //transform.parent = null;
                transform.gameObject.SetActive(false);

            }
        }

        if (isSpawning)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                                  transform.rotation.eulerAngles.y + 5,
                                                  transform.rotation.eulerAngles.z);

            if (!isGrowing && Quaternion.Angle(transform.rotation, rotateTarget) < 1)
            {
                transform.localRotation = rotateTarget;
                isSpawning = false;
            }
        }
    }

    /// <summary>
    /// Starts the growing animation for the weapons.
    /// </summary>
    public void StartGrowing()
    {
        isShrinking = false;
        isSpawning = false;
        isGrowing = true;
    }

    /// <summary>
    /// Starts the shrinking animation for the weapons.
    /// </summary>
    public void StartShrinking()
    {
        transform.parent = null;
        isGrowing = false;
        isSpawning = false;
        isShrinking = true;
    }
    
    /// <summary>
    /// On purchase, Weapons should grow and spin in place.
    /// </summary>
    public void OnPurchase()
    {
        rotateTarget = transform.localRotation;
        isShrinking = false;
        isGrowing = true;
        isSpawning = true;
    }
}
