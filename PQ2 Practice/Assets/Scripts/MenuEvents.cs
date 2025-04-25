using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{

    [SerializeField]
    private GameObject ballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AButtonPressed()
    {
        if (ballPrefab == null) return;

        // Spawn position: from the right hand
        Vector3 spawnPosition = new Vector3(1,1,1);

        // Instantiate the prefab
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

}
