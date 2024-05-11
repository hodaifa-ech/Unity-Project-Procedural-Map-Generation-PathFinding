using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game : MonoBehaviour
{

    public GameObject menu;
    private bool isMenuVisible = false;


    private Vector3 startScale; 

    private void Start()
    {
        startScale = menu.transform.localScale; 
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMenuVisible = !isMenuVisible;

            StartCoroutine(showCard()); 
        }

    }

    public IEnumerator showCard()
    {
        // Toggle menu visibility
        
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            if(isMenuVisible)
            {
                menu.SetActive(isMenuVisible);
                elapsedTime += Time.deltaTime;
                Vector3 LerpedScale = Vector3.Lerp(menu.transform.localScale, startScale + new Vector3(startScale.x+0.3f, startScale.y+0.4f,0), (elapsedTime / 0.1f));
                menu.transform.localScale = LerpedScale;
                yield return null;

            }
            else
            {
                menu.SetActive(isMenuVisible);
                menu.transform.localScale = startScale;
                yield return null;
            }
        }

    }

}
