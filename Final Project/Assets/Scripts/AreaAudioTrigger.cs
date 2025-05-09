using UnityEngine;

public class AreaAudioTrigger : MonoBehaviour
{
    // Reference to the AudioSource component
    [SerializeField]
    public AudioSource areaAudioSource;

    [SerializeField]
    public AudioClip audioClip;

    /// <summary>
    /// Sets the AudioSource variable and checks for player 
    /// entering the trigger area, when the player enters
    /// the area the audio source is set to the specified
    /// audio clip. The audio source and then looped and played.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("MainCamera"))
        {
            // Set loop to true for continuous playback
            areaAudioSource.loop = true;

            // Assign the audio clip and play it
            areaAudioSource.clip = audioClip;
            areaAudioSource.Play();
        }
    }

    /// <summary>
    /// Checks for the player exiting the trigger area,
    /// the audio source is then stopped.
    /// </summary>
    public void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the area
        if (other.CompareTag("MainCamera"))
        {
            // Stop the audio and turn off the loop
            areaAudioSource.Stop();
            areaAudioSource.loop = false;
        }
    }
}
