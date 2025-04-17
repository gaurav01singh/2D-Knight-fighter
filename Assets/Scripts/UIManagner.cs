using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagner : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject hitEffectPanel;
    private bool death = false;


    private void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        hitEffectPanel.SetActive(false);
        Debug.Log("hello");
    }
    public void ShowGameOverPanel()
    {
        death = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
    

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
        //#if UNITY_EDITER
        UnityEditor.EditorApplication.isPlaying = false; // For Unity Editor
        //#endif
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


    private void PauseGame(bool pause)
    {
        pausePanel.SetActive(pause);
        if (pause) 
            Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }
    private void ShowHitEffect()
    {
        hitEffectPanel.SetActive(true);
    }

    private void OnEnable()
    {
        PlayerMovement.OnPlayerHit += ShowHitEffect;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerHit -= ShowHitEffect;
    }
}
