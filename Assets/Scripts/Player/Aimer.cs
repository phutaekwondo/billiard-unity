using UnityEngine;

public class Aimer : MonoBehaviour
{
    [SerializeField] GameObject m_cueBall;
    [SerializeField] GameObject m_imaginationBall;
    public Vector3 GetAimingDirection()
    {
        Vector3 direction = MouseTrackingHelper.GetBallOnTablePositionWithMouse() - m_cueBall.transform.position;
        direction.y = 0;
        direction.Normalize();
        return direction;
    }
    public void SetAimingVisibility(bool visible)
    {
        throw new System.NotImplementedException();
    }
    public void UpdateAimingComponents()
    {
        //update imagination ball position
        //aimline 
        //cueball move direction
        //tagetball move direction
        throw new System.NotImplementedException();
    }
}
