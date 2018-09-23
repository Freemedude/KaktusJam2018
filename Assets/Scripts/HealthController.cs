using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public GameObject HeartPrefab;
    private List<GameObject> hearts = new List<GameObject>();

    /// <summary>
    /// Draws the hearts at the start of the game.
    /// </summary>
    public void DrawHearts(int currentHearts)
    {
        for (int i = 0; i < currentHearts; i++)
        {
            GameObject heart = Instantiate(HeartPrefab,
                new Vector3(transform.position.x + 33 * i, transform.position.y, transform.position.z),
                Quaternion.identity, transform);
                hearts.Add(heart);
        }
    }

    /// <summary>
    /// Updates the hearts on screen.
    /// </summary>
    public void UpdateHearts(int currentHearts)
    {
        hearts[currentHearts].SetActive(false);
    }

    /// <summary>
    /// Draws all the hearts when the game is restarted.
    /// </summary>
    public void RestartHearts()
    {
        foreach (GameObject heart in hearts)
            heart.SetActive(true);
    }
}
