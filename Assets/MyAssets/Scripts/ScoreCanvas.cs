using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCanvas : MonoBehaviour
{
    private Transform playerToFace;

    private void Start()
    {
        playerToFace = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - playerToFace.position);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
