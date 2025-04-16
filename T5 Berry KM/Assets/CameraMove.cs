using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    private float thetaY = 0;
    private float thetaX = 0;

    [SerializeField]
    private float horizontalArc = 100;

    [SerializeField]
    private float verticalArc = 30;

    [SerializeField]
    private float speed = 0.2f;

    public void OnMove(InputAction.CallbackContext context)
    {
        //InputValue can be anything, so it must be cast
        Vector2 delta = context.ReadValue<Vector2>();

        // estimate motion from mouse speed
        thetaY += delta.x * speed;
        thetaX -= delta.y * speed;

        //apply rotation amount, with limits
        thetaY = Mathf.Clamp(thetaY, -horizontalArc, horizontalArc);
        thetaX = Mathf.Clamp(thetaX, -verticalArc, verticalArc);
        transform.localRotation = Quaternion.Euler(thetaX, thetaY, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
