using System;
using UnityEngine;
using static MouseTrackingHelper;

public delegate void CueBallPositionControllerEvent();

public class CueBallPositionController : MonoBehaviour
{
    public event CueBallPositionControllerEvent m_OnChoosingPositionFinished;

    public GameObject m_cueBallGO; //drag and drop cue ball here in inspector
    private CueBall m_cueBall;
    public GameplayManager m_gameplayManager; //drag and drop GameplayManager here in inspector


    private AvailableCueBallProvider m_availableCueBallProvider;
    private float m_defaultBallsY = 0.0f;
    private bool m_isEnabled = false;

    private void Start() 
    {
        m_cueBall = m_cueBallGO.GetComponent<CueBall>();
        m_availableCueBallProvider = GetComponent<AvailableCueBallProvider>();

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
        //need to use raycast in this function
        if ( !m_isEnabled ) return;

        //Move the ball with the mouse
        Vector3 mousePosition = GetMousePositionWithY(m_defaultBallsY);
        mousePosition = new Vector3(mousePosition.x, m_defaultBallsY, mousePosition.z);
        mousePosition = m_availableCueBallProvider.NearestAvailablePosition(mousePosition);

        m_cueBallGO.transform.position = mousePosition;

        //if current cueball position is overlapping with other balls, choose the nearest available position
    }


    public bool IsEnabled() 
    { 
        return m_isEnabled; 
    }

    public void SetCueBallPhysicsEnabled(bool isEnabled)
    {
        m_cueBall.SetCueBallPhysicsEnabled(isEnabled);
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
