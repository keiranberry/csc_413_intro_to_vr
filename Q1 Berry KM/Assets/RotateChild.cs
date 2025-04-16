using UnityEngine;

public class RotateChild : MonoBehaviour
{
    [SerializeField] 
    private GameObject childCube;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            childCube.transform.Rotate(45, 0, 0);
        }
    }
}
