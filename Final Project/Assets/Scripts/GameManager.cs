using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Class that controls the gameplay and its
/// associated information.
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject[] spawnAreas;

    public int score = 0;
    public int bank = 0;

    public float gameDuration = 60f; // Total time in seconds
    private float currentTime;
    private bool gameRunning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Update checks time left in every frame
    /// and calls end game when time is up.
    /// </summary>
    private void Update()
    {
        if (!gameRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndGame();
        }
    }

    /// <summary>
    /// Function to set up and start
    /// a round of the game
    /// </summary>
    public void StartGame()
    {
        score = 0;
        currentTime = gameDuration;
        gameRunning = true;

        //enable spawn areas
        foreach (GameObject spawn in spawnAreas)
        {
            SpawnArea spawnAreaScript = spawn.GetComponent<SpawnArea>();
            spawnAreaScript.GenerateItems();
        }
    }

    /// <summary>
    /// Function to tear down and end 
    /// a round of the game
    /// </summary>
    public void EndGame()
    {
        gameRunning = false;

        //disble spawn areas
        foreach(GameObject spawn in spawnAreas)
        {
            SpawnArea spawnAreaScript = spawn.GetComponent<SpawnArea>();
            spawnAreaScript.DestroyItems();
        }
    }

    /// <summary>
    /// Helper to update score
    /// </summary>
    /// <param name="points"></param>
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }

    /// <summary>
    /// Helper to update bank amount
    /// </summary>
    /// <param name="points"></param>
    public void AddBank(int points)
    {
        bank += points;
    }

    /// <summary>
    /// Helper to format the time remaining for 
    /// the menu
    /// </summary>
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }
}
