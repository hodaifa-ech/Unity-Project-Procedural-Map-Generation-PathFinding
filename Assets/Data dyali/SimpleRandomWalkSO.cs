using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkSO :ScriptableObject
{

    public int iteration = 10,walklength = 10;
    public bool startRandomlyEachIteration = true;
}
