using UnityEngine;

public class CueStick : MonoBehaviour
{
    private const float MIN_AIMING_DISTANCE_FROM_HEAD = 0.5f;
    private const float MAX_AIMING_DISTANCE_FROM_HEAD = 1f;

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
        throw new System.NotImplementedException();
    }
}
