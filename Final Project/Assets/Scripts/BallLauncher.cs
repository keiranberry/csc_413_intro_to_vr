using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField]
    public float launchForceX = 10f;

    [SerializeField]
    public float launchForceY = 5f;

    public Vector3 startPosition;
    public Rigidbody rb;

    /// <summary>
    /// Sets the private rigidbody variable and start position 
    /// of the flying target, and carries out the initial 
    /// launch of the flying target.
    /// </summary>
    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody>();

        LaunchBall();
    }

    private void Update()
    {
        if(transform.position.y <= -3)
        {
            transform.position = startPosition;
            LaunchBall();
        }
    }

    /// <summary>
    /// Launches the flying ball target using a force vector which 
    /// is customizable using the SerializeFields member variables
    /// </summary>
    public void LaunchBall()
    {
        rb.linearVelocity = Vector3.zero;

        Vector3 force = new Vector3(launchForceX, launchForceY, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }

    /// <summary>
    /// Sets the target back to its original position and re-launches 
    /// it upon colliding with an object
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.contacts[0].otherCollider.name.Contains("Spawn"))
        {
            transform.position = startPosition;
            rb.linearVelocity = Vector3.zero;
            LaunchBall();
        }
    }
}
