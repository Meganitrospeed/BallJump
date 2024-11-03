using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPicker : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i; // Capture the index for the closure
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int levelIndex)
    {
        // Assuming your levels are named "Level 1", "Level 2", etc.
        string levelName = "Level " + (levelIndex + 1);
        SceneManager.LoadScene(levelName);
    }
}