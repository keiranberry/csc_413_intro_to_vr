using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Navigation : MonoBehaviour
{
    // player rig parameters
    [SerializeField]
    private GameObject feet;
    [SerializeField]
    private GameObject head;
    // flag to disallow doubling up of teleportation commands
    private bool isMoving = false;
    public bool IsMoving { get => isMoving; }

    // where to try to move
    private Vector3 movementTarget;

    // super speed parameters
    [SerializeField]
    private float teleportTime = 0.1f;
    private float startTime = 0;
    private Vector3 startLoc = Vector3.zero;

    [SerializeField]
    private int maxSteepness = 20;

    [SerializeField]
    private float stepDistance = 0.2f;
    [SerializeField]
    private int correctionIterations = 8;

    [SerializeField]
    public float fallBuffer = 0.05f;
    private float v = 0;
    private List<XRNodeState> nodeStates = new List<XRNodeState>();
    private GameObject lastPlatform;
    private Vector3 lastPlatformLoc;


    void Start()
    {
    }

    public void Update()
    {
        if (isMoving)
        {
            OnMoving();
        }
        else
        {
            XRHMD hmd = InputSystem.GetDevice<XRHMD>();
            if (hmd == null || (hmd.trackingState.value == 3 || hmd.trackingState.value == 15))
            {
                ApplyPhysics();
            }
        }
    }

    /// <summary>
    /// Helper function for initializing movement.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    private void OnInit()
    {
        isMoving = true;
        startTime = 0;
        // find starting ground height
        RaycastHit hitGround;
        if (Physics.Raycast(head.transform.position, Vector3.down, out hitGround))
        {
            startLoc = hitGround.point;
        }
    }

    /// <summary>
    /// Moves player to desired location.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    private void OnMoving()
    {
        // move to next position
        startTime += Time.deltaTime;
        Vector3 target = Vector3.Lerp(startLoc, movementTarget, startTime / teleportTime);
        // offset between play area and head location, same as before
        Vector3 difference = feet.transform.position - head.transform.position;
        difference.y = 0;
        // fix up ground height if going over bumps
        RaycastHit groundHit;
        float playerHeight = head.transform.position.y - feet.transform.position.y;
        if (Physics.Raycast(head.transform.position, Vector3.down, out groundHit))
        {
            // found a bump, change starting height to this height to smooth the effect
            if (groundHit.distance < playerHeight)
            {
                startLoc.y = groundHit.point.y;
            }
        }
        // final position, same as before
        feet.transform.position = target + difference;
        // check to see if movement has ended, and do any cleanup
        if (startTime > teleportTime)
            OnMoveEnd();
    }

    /// <summary>
    /// Helper function for end of move functionality.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    private void OnMoveEnd()
    {
        isMoving = false;
        TeleportTo(movementTarget);
    }

    /// <summary>
    /// Function to teleport to a target location.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="target"></param>
    private void TeleportTo(Vector3 target)
    {
        // offset between play area and head location
        Vector3 difference = feet.transform.position - head.transform.position;
        // ignore changes in y right now, to keep the head at the same height!
        difference.y = 0;
        // final position
        feet.transform.position = target + difference;
    }

    /// <summary>
    /// Check to see whether a given location is legal to move to.
    /// Adjusted from the class version so that colliders (such as our music areas) can be moved into.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="hit"></param>
    /// <returns>Boolean, whether it is legal to move to the location or not</returns>
    public bool IsLegalToMove(RaycastHit hit)
    {
        // currently moving, ignore input
        if (isMoving)
        {
            return false;
        }
        // nothing detected, disallow jumping off cliffs
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            // check how flat the interaction plane is.
            float groundAngle = Mathf.Abs(Vector3.Angle(Vector3.up, hit.normal));
            // if too steep, this location is NOT OK!
            if (groundAngle > maxSteepness)
            {
                return false;
            }
        }

        return true;
    }

    public void OnDrawGizmos()
    {
        // show only during run by checking if a variable is set
        if (head != null)
        {
            Vector3 right = new Vector3(head.transform.right.x, 0, head.transform.right.z);
            right.Normalize();
            Vector3 forward = new Vector3(head.transform.forward.x, 0, head.transform.forward.z);
            forward.Normalize();
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(head.transform.position + forward, new Vector3(.1f, .1f, .1f));
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(head.transform.position + right, new Vector3(.1f, .1f, .1f));
            Gizmos.color = new Color(0, 0, 0, 0.5f);
            Gizmos.DrawSphere(feet.transform.position, 0.3f);
        }
    }

    /// <summary>
    /// Function invoked on confirmation of a step
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="dir"></param>
    public void OnConfirmStep(Vector3 dir)
    {
        movementTarget = AttemptStep(dir);
        OnInit();
    }

    /// <summary>
    /// Attempt a step. If not legal to move, return the same point which you started at.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    private Vector3 AttemptStep(Vector3 step)
    {
        // (1A) where would our teleport point be
        Vector3 newLoc = head.transform.position + step * stepDistance;
        RaycastHit hit;
        Physics.Raycast(newLoc, Vector3.down, out hit);
        // (1B) if legal to teleport, teleport!
        if (IsLegalToMove(hit))
        {
            return hit.point;
        }
        else
        {
            //(3) binary search for legal teleport in between
            Physics.Raycast(head.transform.position, Vector3.down, out hit);
            Vector3 correctedLoc = hit.point;
            float magnitude = stepDistance / 2;
            for (int i = 1; i < correctionIterations; i++)
            {
                // determine no teleport location
                newLoc = head.transform.position + step * magnitude;
                Physics.Raycast(newLoc, Vector3.down, out hit);
                // if legal to teleport, try to move forward by half again
                if (IsLegalToMove(hit))
                {
                    magnitude = magnitude + stepDistance / Mathf.Pow(2, i + 1);
                    correctedLoc = hit.point;
                }
                // if not legal to teleport, try to move backwards by half again
                else
                {
                    magnitude = magnitude - stepDistance / Mathf.Pow(2, i + 1);
                }
            }
            return correctedLoc;
        }
    }

    /// <summary>
    /// Function called when confirming a turn. Rotates the player in the given direction.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    /// <param name="dir"></param>
    public void OnConfirmTurn(float dir, float turnSpeed)
    {
        float direction = -dir * turnSpeed;
        // shift to "origin"
        Vector3 point = feet.transform.position - head.transform.position;
        // rotate around "origin" to find the new location
        float theta = direction * Mathf.Deg2Rad;
        Vector3 newPoint2 = new Vector3(point.x * Mathf.Cos(theta) - point.z * Mathf.Sin(theta),
        point.y,
        point.z * Mathf.Cos(theta) + point.x * Mathf.Sin(theta));
        // shift back
        Vector3 point2 = head.transform.position + newPoint2;
        feet.transform.position = point2;
        // apply rotate in reverse to place head in the same spot
        feet.transform.Rotate(new Vector3(0, -direction, 0));
    }

    /// <summary>
    /// Applies physics to player. If there is nothing under the player, applies gravity.
    /// If the platform the player is on moves, move the player with it.
    /// From tutorial 8 in class. Original code by Dr. Rebenitsch
    /// </summary>
    private void ApplyPhysics()
    {
        float playerHeight = head.transform.position.y - feet.transform.position.y;
        RaycastHit hit;
        Physics.Raycast(head.transform.position, Vector3.down, out hit, playerHeight + fallBuffer);
        if (hit.collider == null)
        {
            // nothing underneath, apply gravity
            v += Physics.gravity.y * Time.deltaTime;
            feet.transform.position = new Vector3(feet.transform.position.x,
            feet.transform.position.y + v * Time.deltaTime,
            feet.transform.position.z);
        }
        else
        {
            // if near the player height, pin to the floor
            TeleportTo(hit.point);
            v = 0;

            // platform moved, nudge player along
            Vector3 platformMovement = lastPlatformLoc - hit.collider.transform.position;
            if (lastPlatform == hit.collider.gameObject)
            {
                feet.transform.position = feet.transform.position - platformMovement;
            }
            lastPlatform = hit.collider.gameObject;
            lastPlatformLoc = lastPlatform.transform.position;

        }
    }


    public void ContinuousMove(Vector3 direction, float speed)
    {
        RaycastHit hit;
        Vector3 move = direction * speed * Time.deltaTime;

        Vector3 newPos = feet.transform.position + move;

        if (Physics.Raycast(newPos + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f))
        {
            float slope = Vector3.Angle(hit.normal, Vector3.up);
            if (slope < maxSteepness)
            {
                feet.transform.position = hit.point;
            }
        }
    }
}