using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    /// <summary>
    /// basic struct to store an object initial orientation
    /// </summary>
    struct InitialState
    {
        public Vector3 position;
        public Quaternion rotation;
        public GameObject child;
    }

    private InitialState[] children;//test

    [SerializeField]
    private KeyCode ResetKey = KeyCode.R;

    /// <summary>
    /// Save the orientation of all objects in this game object
    /// </summary>
    void Start()
    {
        //save children and their starting transform
        children = new InitialState[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            children[i] = new InitialState();
            children[i].position = transform.GetChild(i).position;
            children[i].rotation = transform.GetChild(i).rotation;
            children[i].child = transform.GetChild(i).gameObject;
        }
            
    }

    /// <summary>
    /// Basic key montoring
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(ResetKey))
        {
            ResetAll();
        }
    }

    /// <summary>
    /// Resets the orientation of all children objects
    /// </summary>
    void ResetAll()
    {
        foreach(InitialState state in children)
        {
            //reset transform
            state.child.gameObject.transform.position = state.position;
            state.child.gameObject.transform.rotation = state.rotation;

            //finds the rigid body and remove motion
            Rigidbody rb = state.child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                state.child.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
                state.child.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            }

        }
    }
}
