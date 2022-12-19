using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public ShootController m_shootController = null; // drag and drop the shoot controller in the inspector

    private bool m_isInGameplay = false;

    public bool IsInGameplay { get { return m_isInGameplay; } }

    public void EnterGamePlay()
    {
        m_isInGameplay = true;
        Physics.autoSimulation = true;
        m_shootController.Enable();
    }

    public void ExitGamePlay()
    {
        m_isInGameplay = false;
        Physics.autoSimulation = false;
        m_shootController.Disable();
    }
}
