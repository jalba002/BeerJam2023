using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button exitButton;
    public Button backButton;

    [Header("UI")]
    public RectTransform baseButtons;
    public RectTransform baseOptions;
    public RectTransform baseCredits;

    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        optionsButton.onClick.AddListener(Options);
        creditsButton.onClick.AddListener(Credits);
        exitButton.onClick.AddListener(Exit);
        backButton.onClick.AddListener(BackToMenu);
        BackToMenu();
    }

    public void Play()
    {
        Debug.Log("Play!");
        // Loadscene
        // Play cutscene in the future and when finished, load scene.
        SceneManager.LoadScene("");
    }

    public void Options()
    {
        Debug.Log("Options");
        baseButtons.gameObject.SetActive(false);
        baseOptions.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void Credits()
    {
        Debug.Log("Credits");
        baseButtons.gameObject.SetActive(false);
        baseCredits.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("Exit!");
        if (Application.isPlaying) Application.Quit();
    }

    public void BackToMenu()
    {
        // Hide Credits
        // hide Options,
        // Show buttons
        baseCredits.gameObject.SetActive(false);
        baseOptions.gameObject.SetActive(false);
        baseButtons.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
    }

}
