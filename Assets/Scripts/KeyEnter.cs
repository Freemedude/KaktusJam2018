using UnityEngine;
using UnityEngine.UI;

public class KeyEnter : MonoBehaviour
{
    public string inputName;
    Button buttonMe;

    // Use this for initialization
    void Start()
    {
        buttonMe = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetButtonDown(inputName) || Input.GetKeyDown(KeyCode.Space))
        {
            buttonMe.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}

