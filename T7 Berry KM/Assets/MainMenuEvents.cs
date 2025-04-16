using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DefaultSceneChange()
    {
        PlayerPrefs.SetInt("count", 15);
        PlayerPrefs.SetInt("Sand", 2000);
        // (1) start loading the scene by name 
        AsyncOperation op = SceneManager.LoadSceneAsync("Sandbox", LoadSceneMode.Additive);

        Scene currentScene = SceneManager.GetActiveScene();

        // (2) add a completion listener
        op.completed += (AsyncOperation o) =>
        {
            //(3) now that the scene has started loading, find it by name, and set it to active
            Scene scene = SceneManager.GetSceneByName("Sandbox");
            SceneManager.SetActiveScene(scene);

            //(4) unload prior scene to release memory
            SceneManager.UnloadSceneAsync(currentScene);
        };
    }
    public void EasyModeSceneChange()
    {
        PlayerPrefs.SetInt("count", 5);
        PlayerPrefs.SetInt("Sand", 0);
        // (1) start loading the scene by name 
        AsyncOperation op = SceneManager.LoadSceneAsync("Sandbox", LoadSceneMode.Additive);

        Scene currentScene = SceneManager.GetActiveScene();

        // (2) add a completion listener
        op.completed += (AsyncOperation o) =>
        {
            //(3) now that the scene has started loading, find it by name, and set it to active
            Scene scene = SceneManager.GetSceneByName("Sandbox");
            SceneManager.SetActiveScene(scene);

            //(4) unload prior scene to release memory
            SceneManager.UnloadSceneAsync(currentScene);
        };
    }
}
