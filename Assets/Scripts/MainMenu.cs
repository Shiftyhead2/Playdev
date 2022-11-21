using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuUI;
    [SerializeField]
    private GameObject LoadUI;
    [SerializeField]
    private GameObject ContinueUI;
    [SerializeField]
    private Button ContinueButton;

    private Loader loader;

    void Start()
    {
        loader = FindObjectOfType<Loader>();

        string path = Application.persistentDataPath + "/PlayerData.data";

        if (File.Exists(path))
        {
            ContinueButton.interactable = true;
        }
        else
        {
            ContinueButton.interactable = false;
        }
        MainMenuUI.SetActive(true);
        LoadUI.SetActive(false);
        ContinueUI.SetActive(false);
    }

    public void QuitGame()
    {
        //Debug.Log("Quit game");
        Application.Quit();
    }

    public void NewGame()
    {
        loader.GameIsLoaded = false;
        StartCoroutine(LoadSceneAsync());
    }

    public void ContinueGame()
    {
        loader.GameIsLoaded = true;
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        LoadUI.SetActive(true);
        MainMenuUI.SetActive(false);
        ContinueUI.SetActive(false);
        Slider loadSlider = LoadUI.GetComponentInChildren<Slider>();
        TextMeshProUGUI sliderText = LoadUI.GetComponentInChildren<TextMeshProUGUI>();

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / .9f);
            sliderText.text = (progress * 100f) + "%";
            loadSlider.value = progress;

            yield return null;
        }

    }
}
