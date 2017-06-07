using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Transform MainMenu, MenuJouer, MenuCreer, MenuWaiting, MenuJoin;
    private Transform ActiveMenu;

    private List<Transform> SpecialMenus = new List<Transform>();

    public Text error, GameName;
      
    void Start()
    {
        //SpecialMenus.Add(MenuWaiting);
        //SpecialMenus.Add(MenuJoin);
        ActiveMenu = MainMenu;
    }

    private void Update()
    {
        if(ActiveMenu == MenuCreer)
        {
            if(Input.GetKey(KeyCode.Return))
                LoadMenuIfValidated(MenuWaiting);
        }
    }

    #region navigation

    public void LoadScene(string scene)
    {
        LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void LoadMenu(Transform menu)
    {       
        DisableActiveMenu();
        ActiveMenu = menu;
        menu.gameObject.SetActive(true);
    }

    public void DisableActiveMenu()
    {
        ActiveMenu.gameObject.SetActive(false);
    }

    #endregion

    #region menu Création Partie
    public void LoadMenuIfValidated(Transform menu)
    {
        if(GameName.text.Length > 4)
        {
            LoadMenu(menu);
            error.text = "";
        }
        else
            error.text = "Veuillez entrer un nom de Partie d'au moins 5 Caractères";
    }
    #endregion
}
