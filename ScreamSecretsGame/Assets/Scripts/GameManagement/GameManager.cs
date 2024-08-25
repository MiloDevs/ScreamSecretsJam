using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float loadProgress;
    [SerializeField] private float minimumDisplayTime = 2f; // Minimum time to show the loading screen
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar;

    private AsyncOperation loadingOperation;
    private const float LOAD_READY_THRESHOLD = 0.9f;
    private bool gameIsPaused = false;
    private void Awake()
    {
        DontDestroyOnLoad(this.transform.parent.gameObject);
    }

    private void Start()
    {
        loadingScreen.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (!gameIsPaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        loadingScreen.SetActive(true);
        float elapsedTime = 0f;

        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            elapsedTime += Time.deltaTime;
            loadProgress = Mathf.Clamp01(loadingOperation.progress / LOAD_READY_THRESHOLD);

            // Update the progress bar
            if (progressBar != null)
            {
                progressBar.fillAmount = loadProgress;
            }

            // If the scene is ready to activate and minimum time has elapsed, activate it
            if (loadingOperation.progress >= LOAD_READY_THRESHOLD && elapsedTime >= minimumDisplayTime)
            {
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Ensure minimum display time is met even if loading is very fast
        while (elapsedTime < minimumDisplayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        loadingScreen.SetActive(false);
        MenuManager.menumanagerinstance.HideMainMenu();
        MenuManager.menumanagerinstance.HideAllMenus();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MenuManager.menumanagerinstance.ShowMainMenu();
        }
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        PlayerFPSController.playerinstance.DisablePlayerMovement();
        MenuManager.menumanagerinstance.ShowMenu("Pause Menu");
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void UnpauseGame()
    {
        gameIsPaused = false;
        MenuManager.menumanagerinstance.HideMenu("Pause Menu");
        PlayerFPSController.playerinstance.EnablePlayerMovement();
        Time.timeScale = 1;
    }
}