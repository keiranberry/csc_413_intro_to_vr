using UnityEngine;

public class MakeAsteroids : MonoBehaviour
{
    [SerializeField] 
    private int count = 100; 
    
    [SerializeField]
    private GameObject asteroidModel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject belt = new GameObject("Asteroid Belt"); 
        belt.transform.parent = transform; 
        belt.transform.localPosition = Vector3.zero; 
        belt.transform.localRotation = Quaternion.identity;
        belt.transform.localScale = Vector3.one;

        for (int i = 0; i < count; i++)
        {
            float radius = Random.Range(15f, 17f);
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float height = Random.Range(-1f, 1f);

            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            Vector3 position = new Vector3(x, height, z);
            GameObject asteroid = Instantiate(asteroidModel, position, Random.rotation);

            asteroid.transform.parent = belt.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
