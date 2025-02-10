using UnityEngine;

public class HighValuePinball: PinBallBehavior
{
    private void Awake()
    {
        // 1) shrink the pinball to make it harder to hit a peg
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    public override void OnCollisionEnter(Collision other)
    {
        // 2) complete redo of the collision effect
        if(other.collider.name.Contains("Peg"))
        {
            game.AddPoint(10);
            audioSource.Play();
        }
        else if (other.collider.name.Contains("Player"))
        {
            body.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        }
    }
}
