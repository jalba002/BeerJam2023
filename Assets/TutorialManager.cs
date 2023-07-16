using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    // 
    private int inxTutorial = 0;
    public RectTransform[] allTutorials;
    public RectTransform[] clickToContinue;

    public float waitTime = 10f;
    private float currentTime = 0f;

    private void Start()
    {
        currentTime = waitTime;
        //NextTutorial();
    }
    public void Update()
    {
        if (currentTime > 0f) currentTime -= Time.deltaTime;
        else
        {
            clickToContinue[inxTutorial].gameObject.SetActive(true);
        }

        if (Input.anyKeyDown && currentTime <= 0f)
        {
            NextTutorial();
        }
    }

    public void NextTutorial()
    {
        if (inxTutorial >= allTutorials.Length-1)
        {
            SceneManager.LoadScene("Main");
            return;
        }

        allTutorials[inxTutorial].gameObject.SetActive(true);
        if (inxTutorial > 0)
        {
            allTutorials[inxTutorial - 1].gameObject.SetActive(false);
        }
        inxTutorial++;
        currentTime = waitTime;
    }
}
