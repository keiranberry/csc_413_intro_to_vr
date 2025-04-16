using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AllSceneCommand : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty mainMenuAction;
    private void Start()
    {
        if (mainMenuAction != null)
        {
            mainMenuAction.action.performed += OnMainScene;
        }
        //first load, load the starting scene
        if (SceneManager.GetActiveScene().name == "Players" && SceneManager.sceneCount == 1)
        {
            //load the scene by index
            AsyncOperation op = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
            op.completed += (AsyncOperation o) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
            };
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMainScene(InputAction.CallbackContext context)
    {
        // already in the main menu, short circuit
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        // load the current scene into the first scene listed (main scene) to have all at the end
        AsyncOperation op = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
        Scene currentScene = SceneManager.GetActiveScene();
        op.completed += (AsyncOperation o) =>
        {
            // load the first scene listed (main scene)
            Scene scene = SceneManager.GetSceneByBuildIndex(0);
            SceneManager.SetActiveScene(scene);
            //unload the active scene which should be players as that is a "add on"
            SceneManager.UnloadSceneAsync(currentScene);
        };
    }
}
