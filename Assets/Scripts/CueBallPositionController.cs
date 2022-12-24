using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CueBallPositionControllerEvent();

public class CueBallPositionController : MonoBehaviour
{
    public event CueBallPositionControllerEvent m_OnChoosingPositionFinished;

    internal void Disable()
    {
        //Need to implement
    }

    internal void Enable()
    {
        //Need to implement
    }
}
