using UnityEngine;

public class Generate2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject parent = new GameObject("parent");
        parent.transform.parent = transform.parent;
        parent.transform.localPosition = new Vector3(2, 2, 2);

        for (int i = 1; i < 4; i++)
        {
            string name = "child" + i.ToString();
            GameObject child = new GameObject(name);
            child.transform.localPosition = new Vector3(0, 0, 0);
            child.transform.parent = parent.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
