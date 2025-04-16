using UnityEngine;

public class PostTouch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Contains("Right"))
        {
            Debug.Log("Right Controller touched");
        }
        else if (collision.collider.name.Contains("Left"))
        {
            Debug.Log("Left Controller touched");
        }
    }
}
