using UnityEngine;

public delegate void OnGameplayEvent();
public class GameplayManager : MonoBehaviour
{
    public PlayerController m_playerController = null; // drag and drop the player controller in the inspector
    public GameObject m_cueBall = null;
    public GameObject m_rack = null;

    public event OnGameplayEvent m_OnGameplayRestart;

    private bool m_isInGameplay = false;
    private bool m_isBallsMoving = false;

    private float m_defaultBallsY = 0.032f;
    private Vector3 m_defaultCueBallPosition;

    private void Start() 
    {
        m_defaultCueBallPosition = m_cueBall.transform.position;
    }

    private void Update() 
    {
        m_isBallsMoving = IsBallsMoving_Calculate();
    }

    public float GetDefaultBallsY()
    {
        return m_defaultBallsY;
    }

    public bool IsInGameplay { get { return m_isInGameplay; } }

    public bool IsBallsMoving()
    {
        return m_isBallsMoving;
    }

    private bool IsBallsMoving_Calculate()
    {
        float speedThreshord = 0.01f;

        bool IsMovingBall(GameObject ball)
        {
            //if ball not on the table, it is not moving
            if ( ball.transform.position.y < 0.0f )
            {
                return false;
            }
            return ball.GetComponent<Rigidbody>().velocity.magnitude > speedThreshord;
        }

        //return true if cue ball is moving
        if ( IsMovingBall(m_cueBall) )
        {
            return true;
        }

        // return true if any other ball is moving
        foreach ( Transform ball in m_rack.transform )
        {
            if ( IsMovingBall(ball.gameObject) )
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
        m_playerController.Enable();
    }

    public void ExitGamePlay()
    {
        m_isInGameplay = false;
        Physics.autoSimulation = false;
        m_playerController.Disable();
    }

    public void Pause()
    {
        ExitGamePlay();
    }

    public void Restart()
    {
        //NEED TO IMPLEMENT
        m_OnGameplayRestart.Invoke();

        //reset the cueBall
        m_cueBall.transform.position = m_defaultCueBallPosition;

        //stop the cueBall
        m_cueBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_cueBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //...

        EnterGamePlay();
    }

    public void Resume()
    {
        EnterGamePlay();
    }
}
