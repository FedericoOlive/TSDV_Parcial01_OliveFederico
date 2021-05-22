using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [Serializable]
    public class PanelsList
    {
        public List<Image> images = new List<Image>();
        public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    }
    
    public List<PanelsList> menu = new List<PanelsList>();

    bool buttonPressed = false;
    int currentMenu = 0;
    float colorMultiply = 2;

    private void Start()
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Panel");

        for (int i = 0; i < panels.Length; i++)
        {
            menu.Add(new PanelsList());

            foreach (Image child in panels[i].GetComponentsInChildren<Image>(true))
            {
                menu[i].images.Add(child);
                child.gameObject.SetActive(false);
            }

            foreach (TextMeshProUGUI child in panels[i].GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                menu[i].texts.Add(child);
                child.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < menu.Count; i++)
        {
            if (i == currentMenu)
            {
                foreach (var btn in menu[i].images)
                {
                    btn.gameObject.SetActive(true);
                }

                foreach (var txt in menu[i].texts)
                {
                    txt.gameObject.SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        if (buttonPressed)
        {
            OnMenu();
        }
    }

    public void ButtonPressed(int _currentMenu)
    {
        buttonPressed = true;
        currentMenu = _currentMenu;

        for (int i = 0; i < menu.Count; i++)
        {
            if (i == currentMenu)
            {
                foreach (var btn in menu[i].images)
                {
                    btn.gameObject.SetActive(true);
                    Color color = btn.color;
                    btn.color = new Color(color.r, color.g, color.b, 0);
                }

                foreach (var txt in menu[i].texts)
                {
                    txt.gameObject.SetActive(true);
                    Color color = txt.color;
                    txt.color = new Color(color.r, color.g, color.b, 0);
                }
            }
        }
    }

    private void OnMenu()
    {
        float deltaT = Time.deltaTime;
        for (int i = 0; i < menu.Count; i++)
        {
            if (i != currentMenu)
            {
                foreach (var btn in menu[i].images)
                {
                    Color color = btn.color;
                    btn.color = new Color(color.r, color.g, color.b, color.a - deltaT * colorMultiply);
                    if (btn.color.a < 0 || !buttonPressed)
                    {
                        btn.color = new Color(color.r, color.g, color.b, 0);
                        btn.gameObject.SetActive(false);
                        buttonPressed = false;
                    }
                }

                foreach (var txt in menu[i].texts)
                {
                    Color color = txt.color;
                    txt.color = new Color(color.r, color.g, color.b, color.a - deltaT * colorMultiply);
                    if (txt.color.a < 0 || !buttonPressed)
                    {
                        txt.color = new Color(color.r, color.g, color.b, 0);
                        txt.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (var btn in menu[i].images)
                {
                    btn.gameObject.SetActive(true);

                    Color color = btn.color;
                    btn.color = new Color(color.r, color.g, color.b, color.a + deltaT * colorMultiply);
                    if (btn.color.a > 1 || !buttonPressed)
                    {
                        btn.color = new Color(color.r, color.g, color.b, 1);
                        buttonPressed = false;
                    }
                }

                foreach (var txt in menu[i].texts)
                {
                    txt.gameObject.SetActive(true);

                    Color color = txt.color;
                    txt.color = new Color(color.r, color.g, color.b, color.a + deltaT * colorMultiply);
                    if (txt.color.a > 1 || !buttonPressed)
                    {
                        txt.color = new Color(color.r, color.g, color.b, 1);
                    }
                }
            }
        }
    }

    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}