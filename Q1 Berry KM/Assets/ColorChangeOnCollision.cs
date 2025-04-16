using UnityEngine;

public class ColorChangeOnCollision : MonoBehaviour
{
    [SerializeField] 
    private Material blueMaterial;

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
        if (collision.gameObject.CompareTag("Ground"))
        {
            GetComponent<Renderer>().material = blueMaterial;
        }
    }
}
