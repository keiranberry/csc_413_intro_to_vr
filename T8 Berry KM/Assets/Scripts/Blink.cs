using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
  [SerializeField]
  private float speed = 2;
  private bool onOff = true;
  private float currentTime = 0;


  void Update()
  {
    //update if it is visable
    currentTime += Time.deltaTime;
    GetComponent<Renderer>().enabled = onOff;
    GetComponent<Collider>().enabled = onOff;

    // if a timeout occurs, flip visibility
    if (currentTime >= speed)
    {
      currentTime = 0;
      onOff = !onOff;
    }
  }
}
