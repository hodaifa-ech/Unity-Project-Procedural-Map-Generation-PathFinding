using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
  public abstract void CreatFeedback();
    public abstract void CompletePreviousFeedback();
    protected virtual void OnDestroy()
    {
        CompletePreviousFeedback();
    }
}
