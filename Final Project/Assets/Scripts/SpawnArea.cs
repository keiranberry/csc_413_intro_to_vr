using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    protected int count = 5;

    [SerializeField]
    protected GameObject spawnItem;

    [SerializeField]
    private float launchForceX = 0.0f;

    [SerializeField]
    private float launchForceY = 0.0f;

    protected GameObject[] madeItems;

    private void Start()
    {
        madeItems = new GameObject[count];

        // GenerateItems();
    }

    /// <summary>
    /// Generates selected objects in the spawn range.
    /// </summary>
    public virtual void GenerateItems()
    {
        Bounds b = GetComponent<Collider>().bounds;

        float lastx = 0;
        float lasty = 0;
        float lastz = 0;

        for(int i = 0; i < madeItems.Length; i++)
        {
            float x = 0;
            float y = 0;
            float z = 0;

            do // Randomize location, but prevent overlaps
            {
                x = Random.Range(b.min.x, b.max.x);
                y = Random.Range(b.min.y, b.max.y) + 0.5f;
                z = Random.Range(b.min.z, b.max.z);
            } while (Mathf.Abs(x-lastx) <= 1 && Mathf.Abs(y-lasty) <=1 && Mathf.Abs(z-lastz) <= 1);

            Vector3 spawnLocation = new Vector3(x, y, z);

            madeItems[i] = Instantiate(spawnItem, spawnLocation, Quaternion.identity);

            if (madeItems[i].name.ToLower().Contains("bomb")) // Bomb objects will have a launch force to fly them across the range.
            {
                BallLauncher launchScript = madeItems[i].gameObject.GetComponent<BallLauncher>();
                launchScript.launchForceX = launchForceX;
                launchScript.launchForceY = launchForceY;
                launchScript.startPosition = spawnLocation;
                launchScript.rb = madeItems[i].GetComponent<Rigidbody>();

                launchScript.LaunchBall();
            }
            else if (madeItems[i].name.Contains("Bouncy")) // Bouncy objects will fall straight down and bounce off of the ground
            {
                BouncyBallScript bounceScript = madeItems[i].gameObject.GetComponent<BouncyBallScript>();
                bounceScript.startPosition = spawnLocation;
            }

            lastx = x;
            lasty = y;
            lastz = z;
        }
    }

    /// <summary>
    /// Destroy all objects in the spawn area.
    /// </summary>
    public void DestroyItems()
    {
        foreach(GameObject item in madeItems)
        {
            Destroy(item);
        }
    }

}
