using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour {


    public void nextScene()
    {
        SceneManager.LoadScene("FinalGame");
    }
}
