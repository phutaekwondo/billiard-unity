using UnityEngine;

public class AimLineTracker : MonoBehaviour
{
    public GameObject m_cueBall = null; // The cue ball
    public GameObject m_aimPoint = null; // The aim point

    private LineRenderer m_lineRenderer = null; // The line renderer
    private float m_aimPointY = 0;

    private void Start() {
        if (m_cueBall == null) {
            throw new System.Exception("Cue ball not set");
        }
        if (m_aimPoint == null) {
            throw new System.Exception("Aim point not set");
        }

        if (m_lineRenderer == null) {
            m_lineRenderer = GetComponent<LineRenderer>();
        }
        if (m_lineRenderer == null) {
            throw new System.Exception("Line renderer not set");
        }

        m_aimPointY = m_aimPoint.transform.position.y;
    }

    private void Update() {
        UpdateAimPointPosition();
        AimLineUpdatePosition();
    }

    void AimLineUpdatePosition() {
        m_lineRenderer.positionCount = 2;
        m_lineRenderer.SetPosition(0, m_cueBall.transform.position);
        m_lineRenderer.SetPosition(1, m_aimPoint.transform.position);
    }

    void UpdateAimPointPosition() 
    {
        float yAimPointToCamera = Camera.main.transform.position.y - m_aimPoint.transform.position.y;

        //update the aim point position based on the mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = yAimPointToCamera;
        Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 aimPointPosition = screenToWorldPoint ;

        m_aimPoint.transform.position = aimPointPosition;
    }
}
