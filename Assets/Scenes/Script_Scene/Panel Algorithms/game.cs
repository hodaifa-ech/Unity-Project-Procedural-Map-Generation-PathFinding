using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    

    //*****

    public List<GameObject> menus;
    public List<KeyCode> keys;
    private List<bool> menuVisibilities;
    private Vector3 startScale;
    private Coroutine currentCoroutine;

    private void Start()
    {
        

        //*** 
        menuVisibilities = new List<bool>(new bool[menus.Count]);
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }
        if (menus.Count > 0)
        {
            startScale = menus[0].transform.localScale;
        }
    }

   

    private void Update()
    {

        


        //*****
        for (int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                ToggleMenu(i);
                break; // Only allow one menu toggle per frame
            }
        }
    }
    public void Resume(int index) { ToggleMenu(index); } // for resume btn 
    private void ToggleMenu(int index)
    {
        // Check if any menu is currently visible
        if (IsAnyMenuVisible() && !menuVisibilities[index])
        {
            return; // If another menu is open and the current one is not the same, do nothing
        }

        // Toggle the visibility of the selected menu
        menuVisibilities[index] = !menuVisibilities[index];

        // Hide all other menus
        for (int i = 0; i < menus.Count; i++)
        {
            if (i != index)
            {
                menuVisibilities[i] = false;
            }
        }

        // Update the time scale based on the selected menu's visibility
        Time.timeScale = menuVisibilities[index] ? 0f : 1f;

        // Stop any currently running coroutine
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Start the coroutine for the selected menu
        currentCoroutine = StartCoroutine(ShowCard(menus[index], menuVisibilities[index]));

        // Hide other menus immediately
        for (int i = 0; i < menus.Count; i++)
        {
            if (i != index)
            {
                menus[i].SetActive(false);
            }
        }
    }

    private bool IsAnyMenuVisible()
    {
        foreach (bool visibility in menuVisibilities)
        {
            if (visibility)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator ShowCard(GameObject menu, bool show)
    {
        float elapsedTime = 0f;
        float duration = 0.1f;

        Vector3 targetScale = show ? startScale + new Vector3(1f, 1f, 0) : startScale;

        if (show)
        {
            menu.SetActive(true);
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            menu.transform.localScale = Vector3.Lerp(menu.transform.localScale, targetScale, elapsedTime / duration);
            yield return null;
        }

        menu.transform.localScale = targetScale;

        if (!show)
        {
            menu.SetActive(false);
        }
    }
}
