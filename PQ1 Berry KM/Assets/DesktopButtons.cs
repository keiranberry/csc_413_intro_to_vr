using UnityEngine;

public class DesktopButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject capsule;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            capsule.transform.position = capsule.transform.position + new Vector3(0,0,0.05f);
        }
    }
}
