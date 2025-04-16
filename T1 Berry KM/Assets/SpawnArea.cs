using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    private int number = 3;
    [SerializeField]
    private GameObject item;
    private GameObject[] spawnedObjects;
    private Bounds bounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // cannot spawn if not item is set
        if (item != null)
        {
            // make new object
            spawnedObjects = new GameObject[number];
            for (int i = 0; i < number; i++)
            {
                spawnedObjects[i] = Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
            }

            // set up bounding box using collider if available, and transform is not
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                bounds = collider.bounds;
            }
            else
            {
                bounds = new Bounds(transform.position, transform.localScale);
            }

            PlaceRandomly();
        }

        else
        {
            Debug.Log("No spawn item set");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlaceRandomly()
    {
        for (int i = 0; i < number; i++)
        {
            // get random location in the play area
            float radius = item.transform.localScale.x / 2;
            float x = Random.Range(bounds.min.x + radius, bounds.max.x - radius);
            float y = Random.Range(bounds.min.y + radius, bounds.max.y - radius);
            float z = Random.Range(bounds.min.z + radius, bounds.max.z - radius);

            // place inside of the play area
            spawnedObjects[i].transform.position = new Vector3(x, y, z);
            spawnedObjects[i].transform.parent = transform;
        }
    }

    public void Reset()
    {
        if (spawnedObjects != null)
        {
            for (int i = 0; i < number; i++)
            {
                spawnedObjects[i].SetActive(true);
            }

            PlaceRandomly();
        }
    }
}
