using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsRoll : MonoBehaviour
{
    public GameObject creditsText;
    public float speed = 20f;

    void Update()
    {
        creditsText.transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (creditsText.transform.position.y >= 20)
        {
            SceneManager.LoadScene("New Start Scene");
        }
    }
}
