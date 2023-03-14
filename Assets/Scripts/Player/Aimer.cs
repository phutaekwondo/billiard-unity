using UnityEngine;

public class Aimer : MonoBehaviour
{
    [SerializeField] GameObject m_cueBall;
    public Vector3 GetAimingDirection()
    {
        Vector3 direction = MouseTrackingHelper.GetBallOnTablePositionWithMouse() - m_cueBall.transform.position;
        direction.y = 0;
        direction.Normalize();
        return direction;
    }
}
