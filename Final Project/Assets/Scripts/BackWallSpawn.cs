using UnityEngine;

public class BackWallSpawn : SpawnArea
{ 
    /// <summary>
    /// Custom generate items function specifically for the target models.
    /// Since they have a unique orientation and shape, plus unique spawn conditions.
    /// </summary>
    public override void GenerateItems()
    {
        Bounds bounds = GetComponent<Collider>().bounds;

        float lastx = 0;
        float lasty = 0;

        for( int i = 0;  i < madeItems.Length; i++)
        {
            float x = 0;
            float y = 0;
            float z = (bounds.max.z + bounds.min.z) / 2;

            do
            {
                x = Random.Range(bounds.min.x, bounds.max.x) + 0.5f;
                y = Random.Range(bounds.min.y, bounds.max.y) + 0.5f;
            }while(Mathf.Abs(x-lastx) <= 1 && Mathf.Abs(y-lasty) <= 1);

            Vector3 spawnLocation = new Vector3(x, y, z);

            madeItems[i] = Instantiate(spawnItem, spawnLocation, Quaternion.Euler(-90, 0, 0));

            lastx = x;
            lasty = y;
        }
    }
}
