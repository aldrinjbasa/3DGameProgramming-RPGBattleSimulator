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
        SceneManager.LoadScene("Credits");
    }
}
