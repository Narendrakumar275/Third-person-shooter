using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiscript : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startMenuUI;
    public GameObject gameUI;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Scope UI")]
    public GameObject scopeUI;
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float zoomFOV = 30f;
    public float zoomSpeed = 10f;

    [Header("Player")]
    public GameObject player;

    [Header("Health UI")]
    public Slider healthSlider;

    [Header("Buttons")]
    public Button playButton;
    public Button resumeButton;
    public Button restartButton;
    public Button restartButton2;
    public Button exitButton;
    public Button exitButton2;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip buttonsound;

    void Start()
    {
        Time.timeScale = 0f;

        startMenuUI.SetActive(true);
        gameUI.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        scopeUI.SetActive(false);

        player.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playButton.onClick.AddListener(PlayGame);
        restartButton.onClick.AddListener(RestartGame);
        restartButton2.onClick.AddListener(RestartGame);

        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitGame);
        exitButton2.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (startMenuUI.activeSelf)
                PlayGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameUI.activeSelf)
            {
                if (pausePanel.activeSelf)
                    ResumeGame();
                else
                    PauseGame();
            }
        }

        HandleScope(); 
    }

    void HandleScope()
    {
        if (!gameUI.activeSelf) return;

        if (Input.GetMouseButton(1)) 
        {
            scopeUI.SetActive(true);

            playerCamera.fieldOfView = Mathf.Lerp(
                playerCamera.fieldOfView,
                zoomFOV,
                Time.deltaTime * zoomSpeed
            );
        }
        else
        {
            scopeUI.SetActive(false);

            playerCamera.fieldOfView = Mathf.Lerp(
                playerCamera.fieldOfView,
                normalFOV,
                Time.deltaTime * zoomSpeed
            );
        }
    }

    void PlaySound()
    {
        if (audioSource != null && buttonsound != null)
            audioSource.PlayOneShot(buttonsound);
    }

    public void PlayGame()
    {
        PlaySound();

        startMenuUI.SetActive(false);
        gameUI.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        player.SetActive(true);

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        PlaySound();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        PlaySound();

        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        PlaySound();

        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        PlaySound();

        Debug.Log("Game Closed");
        Application.Quit();
    }

    public void GameOver()
    {
        PlaySound();

        gameOverPanel.SetActive(true);
        gameUI.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void UpdateHealth(float current, float max)
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = max;
        healthSlider.value = current;
    }
}