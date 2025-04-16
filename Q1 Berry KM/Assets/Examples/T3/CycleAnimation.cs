using UnityEngine;

public class CycleAnimation : MonoBehaviour
{
    [SerializeField]
    private float rotationPerSecond = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timeToDegreePerSecond = 360 * rotationPerSecond; 
        float rotationAngle = Time.deltaTime * timeToDegreePerSecond;
        transform.RotateAround(transform.parent.position, transform.parent.up, rotationAngle);
    }
}
