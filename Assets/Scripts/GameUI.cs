using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject startGamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject quitPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Button continueButton;
    [SerializeField] bool isMainMenu;
    [SerializeField] GameObject leaveVillageUI;
    public bool IsPaused { get; private set; }
    public bool IsInSettings { get; private set; }
    public bool IsInQuitMenu { get; private set; }
    public bool IsInPlayGameMenu { get; private set; }
    public bool CanBePaused { get; set; } = true;
    public GameObject LeaveVillageUI => leaveVillageUI;
    AudioManager am;
    Vector2 mousePos;
    Vector2 previouseMousePos;
    float mouseHideTimer = 0f;
    private void Awake()
    {
        if(isMainMenu)
        {
            mainPanel.SetActive(true);
            pausePanel.SetActive(false);
        }
        else
        {
            mainPanel.SetActive(false);
            pausePanel.SetActive(false);
        }

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentScene))
            continueButton.interactable = true;
        else continueButton.interactable = false;

        am = AudioManager.Instance;
    }
    private void Update()
    {
        HandleMouseVisibility();
        if (!isMainMenu && CanBePaused)
        {
            HandleMouseVisibility();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!IsPaused)
                {
                    pausePanel.SetActive(true);
                    Time.timeScale = 0;
                    IsPaused = true;
                }
                else
                {
                    if (IsInSettings)
                    {
                        CloseSettings();
                    }
                    else if(IsInQuitMenu)
                    {
                        NoQuit();
                    }
                    else
                    {
                        pausePanel.SetActive(false);
                        Time.timeScale = 1;
                        IsPaused = false;
                    }
                }
            }
        }
        else if(isMainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsInSettings)
                {
                    CloseSettings();
                }
                else if (IsInQuitMenu)
                {
                    NoQuit();
                }
                else if(IsInPlayGameMenu)
                {
                    BackFromStartGame();
                }
            }
        }
    }
    public void ClearGameData()
    {
        PlayerPrefs.DeleteAll();
        SettingsManager.SaveSettings();
    }
    void HandleMouseVisibility()
    {
        //Hide mouse after 5 seconds of not moving it
        //always visible during main menu
        if (isMainMenu)
            return;
        mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (mousePos == previouseMousePos)
        {
            if (mouseHideTimer >= 3f && Cursor.visible)
            {
                Cursor.visible = false;
            }
            if (mouseHideTimer < 3f)
            {
                mouseHideTimer += Time.deltaTime;
            }
        }
        else
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
            mouseHideTimer = 0f;
        }
        previouseMousePos = mousePos;
    }
    #region Buttons
    public void QuitGame()
    {
        am.PlayButtonClickSound();
        quitPanel.SetActive(true);
        IsInQuitMenu = true;
    }
    public void YesQuit()
    {
        am.PlayButtonClickSound();
        Application.Quit();
    }
    public void NoQuit()
    {
        am.PlayButtonClickSound();
        quitPanel.SetActive(false);
        IsInQuitMenu = false;
    }
    public void NewGame()
    {
        am.PlayButtonClickSound();
        ClearGameData();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ContinueGame()
    {
        am.PlayButtonClickSound();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentScene))
            SceneManager.LoadSceneAsync(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentScene));
    }
    public void StartGame()
    {
        am.PlayButtonClickSound();
        IsInPlayGameMenu = true;
        startGamePanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void BackFromStartGame()
    {
        am.PlayButtonClickSound();
        IsInPlayGameMenu = false;
        startGamePanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void OpenSettings()
    {
        am.PlayButtonClickSound();
        settingsPanel.SetActive(true);
        IsInSettings = true;
    }
    public void CloseSettings()
    {
        am.PlayButtonClickSound();
        settingsPanel.SetActive(false);
        IsInSettings = false;
    }
    public void MainMenu()
    {
        am.PlayButtonClickSound();
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
    }
    public void ResumeGame()
    {
        am.PlayButtonClickSound();
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        IsPaused = false;
    }
    public void RestoreLastCheckpoint()
    {
        am.PlayButtonClickSound();
        ResumeGame();
        PlayerSpawnSystem.Instance.CheckCheckpoints();
    }
    public void LoadVillageScene()
    {
        LevelManager.Instance.LoadVillageScene();
    }
    public void LoadPreviousScene()
    {
        LevelManager.Instance.LoadPreviousScene();
    }
    public void LoadSceneById()
    {
        LevelManager.Instance.LoadSceneById();
    }
    #endregion
}
