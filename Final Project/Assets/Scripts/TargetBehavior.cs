using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private ParticleSystem particles;
    private bool hit = false;
    [SerializeField]
    private AudioSource breakSound;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        breakSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (hit && !particles.isPlaying)
        {
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// When an object gets hit, disable the mesh renderer
    /// and play both an explosion animation as well as a breaking sound
    /// </summary>
    public void GotHit()
    {
        //more complex models need their children to be disabled
        if (transform.gameObject.name.ToLower().Contains("bottle") ||
            transform.gameObject.name.ToLower().Contains("bomb") ||
            transform.gameObject.name.ToLower().Contains("wall"))
        {
            foreach (MeshRenderer mr in transform.GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }
        }
        else
        {
            transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        // Disable collider to prevent multiple animations and sounds being triggered
        transform.gameObject.GetComponent<Collider>().enabled = false;

        // Play explosion animation
        particles.Play();

        // if a breaking sound is set, play it
        if (breakSound != null)
        {
            breakSound.Play();
        }
        hit = true;
    }
}
