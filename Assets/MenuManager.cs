using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
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

    private Canvas selfCanvas;

    public bool isCanvasOn => selfCanvas.enabled;

    private void Awake()
    {
        selfCanvas = GetComponent<Canvas>();

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
        SceneManager.LoadScene(2);
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

    public void OpenAsPause()
    {
        //Time.timeScale = 0f;
        ToggleHide(true);
        baseCredits.gameObject.SetActive(false);
        baseOptions.gameObject.SetActive(false);
        baseButtons.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(BackToGame);
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(BackToGame);
    }

    public void BackToGame()
    {
        //Time.timeScale = 1f;
        ToggleHide(false);
    }

    public void ToggleHide(bool enabled)
    {
        selfCanvas.enabled = enabled;
    }

}
