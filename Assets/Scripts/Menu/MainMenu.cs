using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsMenuPanel;
    [SerializeField]
    private GameObject _smallDifficultyPanel;
    [SerializeField]
    private GameObject _soundMenuPanel;
    private bool _settingsButtonInteractionAllowed = true;

    private static MainMenu _instance;
    public static MainMenu Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("MainMenuManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Play()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 0)
        {
            _smallDifficultyPanel.SetActive(true);
        }
        else
        {
            LoadScene("Main");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void DeleteStats()
    {
        PlayerPrefs.DeleteAll();
        _settingsMenuPanel.GetComponent<SettingsPanel>().SetDifficulty(0);
    }

    public void ReadMoreButton()
    {
        string fileName = "linkToGit";
        string readFromFilePath = Application.streamingAssetsPath + "/" + fileName + ".txt";
        string link = "";

        List<string> lines = File.ReadAllLines(readFromFilePath).ToList();
        StringBuilder text = new StringBuilder();
        foreach (string line in lines)
        {
            text.AppendLine(line);
        }
        link = text.ToString();

        Application.OpenURL(link);
    }

    public void ShowPanel(GameObject panel)
    {
        if (!panel.activeInHierarchy)
        {
            if (_settingsButtonInteractionAllowed)
            {
                panel.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(HideSettingsPanel(panel));
        }
    }

    IEnumerator HideSettingsPanel(GameObject panel)
    {
        _settingsButtonInteractionAllowed = false;
        panel.GetComponent<Animator>().SetTrigger("Hide");

        yield return new WaitForSeconds(1.5f);

        panel.SetActive(false);
        _settingsButtonInteractionAllowed = true;
    }

    private void Start()
    {
        _soundMenuPanel.SetActive(false);
    }
}
