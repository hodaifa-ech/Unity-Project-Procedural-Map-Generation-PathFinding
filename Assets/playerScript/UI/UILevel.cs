using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevel : MonoBehaviour
{
    
    [SerializeField]
    private TextMeshProUGUI text = null;

    public void Udpatelevel(int Coin)
    {

        text.SetText(Coin.ToString());
    }
}
