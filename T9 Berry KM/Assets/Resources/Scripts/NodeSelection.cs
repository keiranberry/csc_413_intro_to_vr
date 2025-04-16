using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NodeSelection : MonoBehaviour
{
  #region custom 2d axis dpad controls
  private float activateDis = 0.8f;
  private bool canTurn = true;
  private float resetDis = 0.3f;
  #endregion


  #region player rig parameters
  [SerializeField]
  private GameObject head;

  [SerializeField]
  private GameObject feet;

  //the movement code
  [SerializeField]
  private Navigation navigation;

  [SerializeField]
  private GameObject rightHand;
  #endregion


  #region selection indicator
  [SerializeField]
  private GameObject decal;
  #endregion


  #region user input events
  [SerializeField]
  private InputActionProperty confirmTeleport;

  [SerializeField]
  private InputActionProperty turn;

  [SerializeField]
  private InputActionProperty vrTurn;
  #endregion

  //waypoints in the scene
  private GameObject[] wayPoints;

  public void OnConfirmTeleport(InputAction.CallbackContext context)
  {
    GameObject node = GetTargetNode();
    if (node != null)
    {
      //get normal of the top of this waypoint
      RaycastHit hit;
      Physics.Raycast(node.transform.position + Vector3.up, Vector3.down, out hit);

      navigation.OnConfirmTeleport(hit);
    }
  }

  public void Update()
  {
    InsertionSortByDistanceToHead();

    GameObject node = GetTargetNode();

    //found decal, turn it on or off as needed
    if (node != null)
    {

      Vector3 pos = node.transform.position;
      Renderer render = node.GetComponent<Renderer>();
      //if we have a render set the decal on top.
      if (render != null)
      {
        pos = new Vector3(pos.x, render.bounds.max.y, pos.z);
      }

      //get normal of the top of this waypoint
      RaycastHit hit;
      Physics.Raycast(pos + Vector3.up * 0.1f, Vector3.down, out hit);
      //rotate decal to this normal
      decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
      //place decal on top, and nudge up a tiny bit
      decal.transform.position = pos + Vector3.up * 0.01f;

      decal.SetActive(true);
    }
    else
    {
      decal.SetActive(false);
    }
  }


  private void OnVrTurn(InputAction.CallbackContext context)
  {
    Vector2 input = context.ReadValue<Vector2>();
    Debug.Log(input);
    if (canTurn && Mathf.Abs(input.x) > activateDis)
    {
      navigation.OnConfirmTurn(input.x);
      canTurn = false;
    }
    else
    {
      if (Mathf.Abs(input.x) < resetDis)
        canTurn = true;
    }
  }

  private void OnConfirmTurn(InputAction.CallbackContext context)
  {
    float input = context.ReadValue<float>();
    navigation.OnConfirmTurn(input);
  }


  void Start()
  {
    confirmTeleport.action.performed += OnConfirmTeleport;
    turn.action.performed += OnConfirmTurn;
    vrTurn.action.performed += OnVrTurn;

    if (decal != null)
    {
      decal = Instantiate(decal);
      decal.SetActive(false);
    }

    wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
    Array.Sort(wayPoints, CompareEntries);
  }
  public int CompareEntries(GameObject left, GameObject right)
  {
    float distToLeft = (head.transform.position - left.transform.position).magnitude;
    float distToRight = (head.transform.position - right.transform.position).magnitude;
    if (distToLeft == distToRight)
    {
      return 0;
    }
    else if (distToLeft > distToRight)
    {
      return 1;
    }
    else
    {
      return -1;
    }
  }

  private void InsertionSortByDistanceToHead()
  {
    // for every element
    for (int i = 1; i < wayPoints.Length; i++)
    {
      // for every element already sorted
      GameObject key = wayPoints[i];
      bool flag = false;
      for (int j = i - 1; j >= 0 && !flag; j--)
      {
        // shift element up one, until the correct spot is found
        if (CompareEntries(key, wayPoints[j]) < 0)
        {
          wayPoints[j + 1] = wayPoints[j];
          wayPoints[j] = key;
        }
        else
          flag = true;
      }
    }
  }



  public void OnDrawGizmos()
  {
    if (head != null)
    {
      //show teleport range
      Gizmos.color = new Color(1, 1, 1, 0.5f);
      float playerHeight = head.transform.position.y - feet.transform.position.y;
      Gizmos.DrawCube(head.transform.position - new Vector3(0, playerHeight, 0),
                      new Vector3(2 * maxDistance, maxHeightDiff, 2 * maxDistance));
      Gizmos.color = new Color(1, 0, 0, 0.5f);
      Gizmos.DrawSphere(head.transform.position, maxDistance);



    }
  }


  [SerializeField]
  private float maxDistance = 10f;
  [SerializeField]
  private float maxAngle = 45f;
  [SerializeField]
  private float maxHeightDiff = float.PositiveInfinity;


  private GameObject GetTargetNode()
  {
    foreach (GameObject g in wayPoints)
    {
      //make sure the node is within distance
      float distance = Vector3.Distance(head.transform.position, g.transform.position);
      if (distance > maxDistance)
      {
        break;
      }


      float theta = Vector3.Angle(head.transform.forward, g.transform.position - head.transform.position);
      if (theta < maxAngle && Mathf.Abs(feet.transform.position.y - g.transform.position.y) < maxHeightDiff)
      {
        return g;
      }
    }
    return null;
  }


}

