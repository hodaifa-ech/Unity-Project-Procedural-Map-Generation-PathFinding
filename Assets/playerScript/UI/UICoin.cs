using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text = null;

    public void UdpateCoin(int Coin)
    {
        
        text.SetText(Coin.ToString());
    }
}
