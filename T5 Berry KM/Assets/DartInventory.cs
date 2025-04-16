using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DartInventory : MonoBehaviour
{
    private List<GameObject> darts = new List<GameObject>();

    [SerializeField]
    private float force = 75f;

    [SerializeField]
    private InputActionProperty action;

    public void AddDart(GameObject dart)
    {
        darts.Add(dart);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        action.action.performed += OnThrow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (darts.Count > 0)
        {
            //get dart, and reenable
            GameObject dart = darts[0];
            darts.RemoveAt(0);
            dart.SetActive(true);

            //place in hand, and do a final rotation since the axis are not the same
            dart.transform.position = transform.position +transform.up*0.3f;
            dart.transform.localRotation = transform.rotation;
            Quaternion rotate = Quaternion.Euler(90, 0, 0);
            dart.transform.localRotation *= rotate;

            //apply force
            Rigidbody body = dart.GetComponent<Rigidbody>();
            body.AddForce(transform.up * force, ForceMode.Force);
        }
    }
}
