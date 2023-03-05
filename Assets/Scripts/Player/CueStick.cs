using UnityEngine;

public class CueStick : MonoBehaviour
{
    //REFERENCES
    [SerializeField] private GameObject m_bumper; // the bumper has the same global position as the cue stick
    [SerializeField] private GameObject m_tip;

    //VIRAIBLES
    private float m_cueStickLength;
    private const float MIN_AIMING_DISTANCE_FROM_HEAD = 0.5f;
    private const float MAX_AIMING_DISTANCE_FROM_HEAD = 1f;
    private float m_power = 0f;
    private Vector3 m_aimingDirection = Vector3.zero;
    private Vector3 m_aimingPoint = Vector3.zero;
    private Vector3 DEFAULT_AIMING_DIRECTION = new Vector3(0,0,1);

    //PRIVATE METHODS
    private void Start() 
    {
        m_cueStickLength = Vector3.Distance( m_bumper.transform.position, m_tip.transform.position );
    }

    //PUBLIC METHODS
    public void SetAim(Vector3 direction, Vector3 point)
    {
        SetAimDirection(direction);
        SetAimPoint(point);
    }
    public void SetAimDirection(Vector3 aimingDirection)
    {
        throw new System.NotImplementedException();
    }
    public void SetAimPoint(Vector3 aimPoint)
    {
        throw new System.NotImplementedException();
    }
    public void SetAimingDistance(float aimingDistance)
    {
        throw new System.NotImplementedException();
    }
    public void SetVisibility(bool visible)
    {
        GetComponent<MeshRenderer>().enabled = visible;
    }
    private void UpdateCueStickTransform()
    {
        //position
        Vector3 tipPosition = m_aimingPoint - m_aimingDirection * (MIN_AIMING_DISTANCE_FROM_HEAD + m_power * (MAX_AIMING_DISTANCE_FROM_HEAD - MIN_AIMING_DISTANCE_FROM_HEAD));
        Vector3 bumperPosition = tipPosition - m_aimingDirection * m_cueStickLength;
        transform.position = bumperPosition;

        //rotation
        //TODO: just rotate y axis
    }
}
