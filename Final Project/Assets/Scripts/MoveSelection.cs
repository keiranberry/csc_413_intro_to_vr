using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSelection : MonoBehaviour
{
    // the movement code
    [SerializeField]
    private Navigation navigation;
    // user input events
    [SerializeField]
    private InputActionProperty walk;
    [SerializeField]
    private InputActionProperty strafe;
    [SerializeField]
    private InputActionProperty turn;

    // player rig parameters
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private GameObject rightHand;

    [SerializeField]
    private InputActionProperty vrWalk;
    [SerializeField]
    private InputActionProperty vrTurn;

    private bool canMove = true;
    private float resetDis = 0.3f;
    private float activateDis = 0.8f;
    private bool canTurn = true;

    private float currentWalkInput = 0f;
    private float currentStrafeInput = 0f;
    private float currentTurnInput = 0f;

    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float turnSpeed = 45f;

    void Start()
    {
        vrWalk.action.performed += OnVrMove;
        vrTurn.action.performed += OnVrTurn;

        walk.action.performed += ctx => currentWalkInput = ctx.ReadValue<float>();
        walk.action.canceled += ctx => currentWalkInput = 0f;

        strafe.action.performed += ctx => currentStrafeInput = ctx.ReadValue<float>();
        strafe.action.canceled += ctx => currentStrafeInput = 0f;

        turn.action.performed += ctx => currentTurnInput = ctx.ReadValue<float>();
        turn.action.canceled += ctx => currentTurnInput = 0f;
    }

    public void Update()
    {
        if (Mathf.Abs(currentWalkInput) > 0.01f)
        {
            Vector3 forward = new Vector3(head.transform.forward.x, 0, head.transform.forward.z).normalized;
            navigation.ContinuousMove(forward * currentWalkInput, moveSpeed);
        }

        if (Mathf.Abs(currentStrafeInput) > 0.01f)
        {
            Vector3 right = new Vector3(head.transform.right.x, 0, head.transform.right.z).normalized;
            navigation.ContinuousMove(right * currentStrafeInput, moveSpeed);
        }

        if (Mathf.Abs(currentTurnInput) > 0.1f)
        {
            navigation.OnConfirmTurn(currentTurnInput, turnSpeed);
        }
    }

    /// <summary>
    /// Called when move is requested in VR by the player. Preprocesses the direction and passes the step request on to the navigation.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="context"></param>
    private void OnVrMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (canMove && input.magnitude > activateDis)
        {
            // 1) figure out our direction in left-handed system
            float theta = Mathf.Atan2(input.x, input.y);
            // 2) rotate forward around up by theta
            Vector3 forward = new Vector3(head.transform.forward.x, 0, head.transform.forward.z);
            forward.Normalize();
            Vector3 dir = Quaternion.AngleAxis(Mathf.Rad2Deg * theta, Vector3.up) * forward;
            // 3) apply step
            navigation.OnConfirmStep(dir);
            canMove = false;
        }
        else
        {
            if (input.magnitude < resetDis)
                canMove = true;
        }
    }

    /// <summary>
    /// Called when turn is requested by the player in VR. Calls the navigation's turn function.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="context"></param>
    private void OnVrTurn(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        //Debug.Log(input);
        if (canTurn && Mathf.Abs(input.x) > activateDis)
        {
            navigation.OnConfirmTurn(input.x, turnSpeed);
            canTurn = false;
        }
        else
        {
            if (Mathf.Abs(input.x) < resetDis)
                canTurn = true;
        }
    }
}
