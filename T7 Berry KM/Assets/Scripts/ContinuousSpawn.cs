using UnityEngine;

public class ContinuousSpawn : SpawnArea
{
    [SerializeField]
    private int maxPerUpdate = 50;
    //how many currently made
    private int currentlyMade = 0;
    [SerializeField]
    private string childName = "Spawned Items";
    private GameObject child;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        child = new GameObject(childName);
        child.transform.parent = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentlyMade < madeItems.Length)
            GenerateItems();
    }

    protected override void GenerateItems()
    {
        //make new items randomly within the collider bounds
        Bounds b = GetComponent<Collider>().bounds;
        int i = 0;
        while (currentlyMade < madeItems.Length && i < maxPerUpdate)
        {
            i++;
            int index = Random.Range(0, items.Length);
            madeItems[currentlyMade] = MakeItemInArea(b, items[index]);
            madeItems[currentlyMade].transform.parent = child.transform;
            currentlyMade++;
        }
    }

}
