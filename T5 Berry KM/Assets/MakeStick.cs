using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStick : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }


  private void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.name.Contains("Dart"))
    {
      Debug.Log("D:" + Vector3.Distance(collision.gameObject.transform.position, transform.position));
      //rought estimate to restrict to being inside the dart board circle
      if (Vector3.Distance(collision.gameObject.transform.position, transform.position) < transform.localScale.x)
      {
        Rigidbody body = collision.gameObject.gameObject.GetComponent<Rigidbody>();
        body.isKinematic = true;
      }

    }
  }



}
