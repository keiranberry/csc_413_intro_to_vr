
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
  [SerializeField]
  protected string countKey = "count";

  [SerializeField]
  protected GameObject[] items;

  //object currently made
  protected GameObject[] madeItems;

  [SerializeField]
  protected int numToMakeIfNotFound = 15;


  // Start is called before the first frame update
  void Awake()
  {
    if (items.Length == 0)
    {
      Debug.LogWarning("No spawn items made, since no items were in the list");
      return;
    }
    int count = PlayerPrefs.GetInt(countKey, numToMakeIfNotFound);
    madeItems = new GameObject[count];

    //madeItems = new GameObject[numToMakeIfNotFound];
  }

  void Start()
  {
    if (items.Length != 0)
    {
      GenerateItems();
    }
  }

  protected virtual void GenerateItems()
  {
    //make new items randomly within the collider bounds
    Bounds b = GetComponent<Collider>().bounds;
    for (int i = 0; i < madeItems.Length; i++)
    {
      int index = Random.Range(0, items.Length);
      madeItems[i] = MakeItemInArea(b, items[index]);
    }
  }

  /// <summary>
  /// Randomly place this item in teh collider area
  /// </summary>
  /// <param name="b"></param>
  /// <param name="i"></param>
  /// <param name="item"></param>
  protected GameObject MakeItemInArea(Bounds b, GameObject item)
  {
    float x = Random.Range(b.min.x, b.max.x);
    float y = Random.Range(b.min.y, b.max.y);
    float z = Random.Range(b.min.z, b.max.z);

    return Instantiate(item, new Vector3(x, y, z), Quaternion.identity);
  }
}
