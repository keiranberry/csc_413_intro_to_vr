using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparatusLaser : MonoBehaviour
{

  private LineRenderer line;

  [SerializeField]
  private float startingWidth = 0.05f;

  private Bounds bounds;

  [SerializeField]
  private int maxBounce = 10;

  [SerializeField]
  private float maxDistance = 20;


  void Update()
  {
    bounds = GetComponent<Collider>().bounds;
    MakeReflection();
  }

  private void MakeReflection()
  {
    Vector3 start = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
    Vector3 direction = transform.up;

    List<Vector3> points = new List<Vector3>();
    points.Add(start);

    //repeatly bounce laser until absorbed, nothing hit, or max bonces met
    int count = 1;
    bool done = false;
    while (!done)
    {
      //shoot out a ray that aligns to the current points
      RaycastHit hit;
      if (Physics.Raycast(start, direction, out hit))
      {
        //hit a mirror, add point and bounce
        if (hit.collider.name.Contains("Mirror"))
        {
          points.Add(hit.point);

          // calculate the new direction r = d−2(d⋅n)n
          start = hit.point;
          direction = direction - 2 * Vector3.Dot(direction, hit.normal) * hit.normal;
          direction.Normalize();
        }
        else
        {
          //laser absorbed
          points.Add(hit.point);
          done = true;
        }

      }
      else
      {
        //nothing hit, stop
        points.Add(start + direction * maxDistance);
        done = true;

      }

      //stop infinite reflection
      count++;
      if (count > maxBounce)
        done = true;

    }

    line.positionCount = points.Count;
    line.SetPositions(points.ToArray());

  }


  void Start()
  {
    //make a line renderer
    line = gameObject.AddComponent<LineRenderer>();

    //give it a base material so the color can be changed
    line.material = new Material(Shader.Find("Sprites/Default"));

    SetWidth(startingWidth);
  }



  public void SetWidth(float width)
  {
    line.startWidth = width;
    line.endWidth = width;

  }


  public void SetColor(Color c)
  {
    line.startColor = c;
    line.endColor = c;

  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.name.Contains("RedLens"))
    {
      SetColor(Color.red);
    }
    else if (collision.gameObject.name.Contains("GreenLens"))
    {
      SetColor(Color.green);
    }
    else if (collision.gameObject.name.Contains("BlueLens"))
    {
      SetColor(Color.blue);
    }
    else if (collision.gameObject.name.Contains("WhiteLens"))
    {
      SetColor(Color.white);

    }


  }
}
