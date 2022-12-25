using UnityEngine;
using static MouseTrackingHelper;

public delegate void CueBallPositionControllerEvent();

public class CueBallPositionController : MonoBehaviour
{
    public event CueBallPositionControllerEvent m_OnChoosingPositionFinished;

    public GameObject m_cueBall; //drag and drop cue ball here in inspector
    public GameplayManager m_gameplayManager; //drag and drop GameplayManager here in inspector

    private float m_defaultBallsY = 0.0f;
    private bool m_isEnabled = false;

    private void Start() 
    {
        m_defaultBallsY = m_gameplayManager.GetDefaultBallsY();
        m_OnChoosingPositionFinished += OnFinishedInternal;
    }

    private void OnFinishedInternal()
    {
        this.Disable();
    }

    private void Update() 
    {
        if ( !m_isEnabled ) return;
        UpdateCueBallPosition();
        
        //hanlde input
        if ( Input.GetMouseButtonDown(0) )
        {
            m_OnChoosingPositionFinished.Invoke();
        }
    }

    public void UpdateCueBallPosition()
    {
        if ( !m_isEnabled ) return;

        //Move the ball with the mouse
        Vector3 mousePosition = GetMousePositionWithY(m_defaultBallsY);
        Vector3 cueBallPositionUpdate = new Vector3(mousePosition.x, m_defaultBallsY, mousePosition.z);
        m_cueBall.transform.position = cueBallPositionUpdate;

        //avoid cueball is overlap with other game objects

        //avoid cueball is out of table
    }

    public bool IsEnabled() 
    { 
        return m_isEnabled; 
    }

    public void SetCueBallPhysicsEnabled(bool isEnabled)
    {
        if ( !isEnabled )
        {
            m_cueBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_cueBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        m_cueBall.GetComponent<Rigidbody>().isKinematic = !isEnabled;
        m_cueBall.GetComponent<Rigidbody>().detectCollisions = isEnabled;
    }

    public void Disable()
    {
        m_isEnabled = false;
        //enable rigidbody of cue ball
        SetCueBallPhysicsEnabled(true);
    }

    public void Enable()
    {
        m_isEnabled = true;
        //disable rigidbody of cue ball
        SetCueBallPhysicsEnabled(false);
    }
}
