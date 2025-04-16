using UnityEngine;

public class PinBallBehavior : MonoBehaviour
{
    // 1) other components affected by the pin ball
    public MeshRenderer color;
    public Rigidbody body;
    public GameEvents game;
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        // 2) if the sphere goes out of bounds, delete it
        if (transform.position.y < 0)
            Destroy(gameObject);
    }
    public virtual void OnCollisionEnter(Collision other) 
    {   // 3) if the other object is a peg, add to the score, and play audio
        if (other.collider.name.Contains("Peg"))   
        {     
            game.AddPoint(1);     
            audioSource.Play();   
        }

        //if the other object is the player block, push the pinball back up
        else if (other.collider.name.Contains("Player"))   
        {     
            body.AddForce(new Vector3(0, 15, 0), ForceMode.Impulse);   
        } 
    }
}
