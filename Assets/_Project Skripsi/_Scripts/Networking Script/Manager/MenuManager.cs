using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] Menu[] menus;

    public string currentMenu;
    public string previousMenu;
    private void Awake()
    {
        instance = this;

    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
                currentMenu = menuName;
            }

            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
                if (menus[i].hidePreviousMenu)
                {
                    OpenMenu(previousMenu);
                }
                previousMenu = menus[i].menuName;
            }
        }
    }

    public void Onclik_BackButton()
    {
        OpenMenu(previousMenu);
    }

    public void OpenMenu(Menu menu)
    {

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
