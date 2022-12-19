using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public ShootController m_shootController = null; // drag and drop the shoot controller in the inspector

    public void EnterGamePlay()
    {
        Physics.autoSimulation = true;
        m_shootController.Enable();
    }

    public void ExitGamePlay()
    {
        Physics.autoSimulation = false;
        m_shootController.Disable();
    }
}
