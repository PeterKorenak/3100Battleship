using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int aiDifficulty = 1;
    public bool won = true;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetDifficulty(int level)
    {
        aiDifficulty = level;
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(1);
    }

    public void LoadStart()
    {
        SceneManager.LoadScene(0);
    }

}
