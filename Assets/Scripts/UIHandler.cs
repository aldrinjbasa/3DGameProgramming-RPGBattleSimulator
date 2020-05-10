using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject startMessage;
    public GameObject pauseMenu;
    public GameObject toCredits;
    public GameObject instructions;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseStartMessage()
    {
        startMessage.SetActive(false);
    }
    public void Credits()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        SceneManager.LoadScene("Credits");
    }
    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        FindObjectOfType<AudioManager>().Play("WorldMusic");
        SceneManager.LoadScene("Home");
    }
    public void OpenInstructions()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        instructions.SetActive(true);
    }
    public void CloseInstructions()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        instructions.SetActive(false);
    }
    public void CloseGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        SceneManager.LoadScene("MainMenu");
    }
}
