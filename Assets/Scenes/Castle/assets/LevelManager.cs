using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void RestartLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the scene with the specified index
        SceneManager.LoadScene(currentSceneIndex);
    }
}
