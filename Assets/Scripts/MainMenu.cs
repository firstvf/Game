using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    private Player _player;
    private bool _isAbleToOpen = true;
    private bool _isAbleToRestart = true;
    private Scene _bossScene => SceneManager.GetSceneByBuildIndex(5);

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)        
            OpenInfoUI();        
    }

    public void RestartGame()
    {
        PlayerData.RestartGame();
        SceneManager.LoadScene(0);
    }

    public void GitHubLink()
    {
        Application.OpenURL("https://github.com/firstvf");
    }

    public void OpenInfoUI()
    {
        _mainMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitMenu()
    {
        _mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private IEnumerator TimeBeforeRestartScene()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<PlayerData>().SaveDataRestartLevel();
        SceneManager.LoadScene(_bossScene.name);
    }

    private IEnumerator TimeBeforeOpenMainMenu()
    {
        yield return new WaitForSeconds(1.5f);
        OpenInfoUI();
    }

    private void Update()
    {
        if (_player == null && _isAbleToRestart && _bossScene.isLoaded)
        {
            _isAbleToRestart = false;
            StartCoroutine(TimeBeforeRestartScene());
        }
        if (_player == null && _isAbleToOpen && !_bossScene.isLoaded)
        {
            _isAbleToOpen = false;
            StartCoroutine(TimeBeforeOpenMainMenu());
        }
        if (Input.GetKey(KeyCode.I) && !_mainMenu.activeInHierarchy)
            OpenInfoUI();
        if (Input.GetKey(KeyCode.Escape))
            ExitMenu();
    }
}
