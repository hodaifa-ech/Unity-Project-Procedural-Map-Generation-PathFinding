using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingMap : MonoBehaviour
{
    public Slider SroomPercent ;
    public Slider ScorridorLength;
    public Slider ScorridorCount;
   

    // ***** MAP *****
    private void Awake()
    {

        SroomPercent.value = 1f;
        ScorridorLength.value =  25f  ;  
        ScorridorCount.value =  25f  ;
        Setting();


    }

    

    public void Setting()
    {
        CorridorFirstDundeonGenerator.roomPercent = SroomPercent.value;  // 1
        CorridorFirstDundeonGenerator.corridorLength = (int) ScorridorLength.value;   // 25
        CorridorFirstDundeonGenerator.corridorCount = (int) ScorridorCount.value; // 25
    }

    public void DefaultSetting()
    {
        SroomPercent.value = 1f;
        ScorridorLength.value = 25f;   // 8
        ScorridorCount.value = 25f;   // 8
    }


}
