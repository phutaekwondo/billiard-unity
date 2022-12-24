using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public ShootController m_shootController = null; // drag and drop the shoot controller in the inspector
    public GameObject m_cueBall = null;
    public GameObject m_rack = null;

    private bool m_isInGameplay = false;
    private bool m_isBallsMoving = false;

    private void Update() 
    {
        m_isBallsMoving = IsBallsMoving_Calculate();
    }

    public bool IsInGameplay { get { return m_isInGameplay; } }

    public bool IsBallsMoving()
    {
        return m_isBallsMoving;
    }

    private bool IsBallsMoving_Calculate()
    {
        float speedThreshord = 0.01f;

        //return true if cue ball is moving
        if ( m_cueBall.GetComponent<Rigidbody>().velocity.magnitude > speedThreshord )
        {
            return true;
        }

        // return true if any other ball is moving
        foreach ( Transform ball in m_rack.transform )
        {
            if ( ball.GetComponent<Rigidbody>().velocity.magnitude > speedThreshord )
            {
                return true;
            }
        }

        return false;
    }

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
