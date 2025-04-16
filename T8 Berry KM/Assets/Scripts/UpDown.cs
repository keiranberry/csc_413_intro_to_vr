using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
  [SerializeField]
  private float speed = 2;

  private bool upDown = true;
  private Vector3 startPos;

  [SerializeField]
  private GameObject targetPos;
  private Vector3 endPos;
  private float currentTime = 0;


  void Start()
  {
    startPos = transform.position;
    endPos = targetPos.transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    currentTime += Time.deltaTime;

    //move platform
    float percent = currentTime / speed;
    if (upDown)
    {
      transform.position = Vector3.Lerp(startPos, endPos, percent);
    }
    else
    {
      transform.position = Vector3.Lerp(endPos, startPos, percent);
    }

    //on timeout, reverse direction
    if (percent >= 1)
    {
      currentTime = 0;
      upDown = !upDown;
    }
  }
}
