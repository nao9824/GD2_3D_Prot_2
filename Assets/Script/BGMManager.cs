using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance = null;

    public static BGMManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if ((instance != null && instance != this) ||
            SceneManager.GetActiveScene().name==("Title") ||
            SceneManager.GetActiveScene().name == ("StageChoice"))
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
