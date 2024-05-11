using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class WallTypeNews
{
    public static HashSet<int> wallTop = new HashSet<int>
   {
       0b0010,
        0b1111

   };
    public static HashSet<int> wallSideLeft = new HashSet<int>
   {
      0b01000
   };

    public static HashSet<int> wallSideRight = new HashSet<int>
{
     0b0001
};

public static HashSet<int> wallBottm = new HashSet<int>
{
     0b1000
};

}
