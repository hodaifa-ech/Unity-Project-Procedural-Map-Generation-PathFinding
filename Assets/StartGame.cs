using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartGame : MonoBehaviour 
{

    [SerializeField]
    private AbstractDungeonGenerator dungeonGenerator;


    public GameObject algorithm; 

    // Start is called before the first frame update

    
   
    IEnumerator Start()
    {
        yield return Method1();
        yield return Method2();

    }

    IEnumerator Method1()
    {
        dungeonGenerator.GenerateDunguon();
        yield return null;


    }

    IEnumerator Method2()
    {
        algorithm.SetActive(true);
        yield return null;

    }

 



   
}
