using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuScriptNew : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject levelClearedMenu;
    public GameObject faceClearedText;
    public GameObject planetStartText;

    public GameObject[] menus;

    public Button nextLevelButton;
    public Ray ray;
    public RaycastHit hit;
    private GameObject currentHit;

    private GraphicRaycaster ray2;


    // Start is called before the first frame update
    void Start()
    {
        menus = new GameObject[] { pauseMenu, mainMenu, gameOverMenu, levelClearedMenu };
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        levelClearedMenu.SetActive(false);
        faceClearedText.SetActive(false);
        nextLevelButton = levelClearedMenu.transform.Find("Upgrade1").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagement.pauseIsPressed)
        {
            ControlPauseMenu();
        }

        //If uiMouseClicked is true
        if (GameManagement.uiMouseClicked)
        {
            //Check the raycast to see if button is being pressed... doesn't detect ui button for some reason
            MouseRaycast();
            
        }

        if(GameManagement.endItemChosen)
        {
            GameManagement.endItemChosen = false;
            LoadLevelSelect();
        }
    }

    //Why isn't the button being clicked?????????????????????????????
    public void LoadLevelSelect()
    {
        //Turn off all menus first
        foreach (GameObject gameObject in menus)
        {
            if (gameObject.activeSelf != false)
            {
                gameObject.SetActive(false);
            }
        }
        GameManagement.menuOnScreen = false;

        Time.timeScale = 1f;

        //Load next scene
        SceneManager.LoadScene("PlanetSelector");
    }

    public void ControlPauseMenu()
    {
        if (GameManagement.gameIsPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            GameManagement.gameIsPaused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            GameManagement.gameIsPaused = true;
        }

        GameManagement.pauseIsPressed = false;
    }

    public void LoadMenu(GameObject menu)
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        GameManagement.menuOnScreen = true;
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        GameManagement.menuOnScreen = false;
    }

    private void MouseRaycast()
    {
        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            currentHit = hit.collider.gameObject;
            print(currentHit.name);
            //If currentHit has a button
            if (currentHit.GetComponent<Button>())
            {
                //Activate button
                currentHit.GetComponent<Button>().onClick.Invoke();
            }

        }
    }

    public void SetFaceClearedText(bool active)
    {
        faceClearedText.SetActive(active);
    }

}
