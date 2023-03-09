using UnityEngine;
using static GameConfig;

class MouseTrackingHelper
{
    static public Vector3 GetBallOnTablePositionWithMouse()
    {
        return GetMousePositionWithY(GameConfig.m_ballOnTableYPosition);
    }

    static public Vector3 GetMousePositionOnPlane(Plane plane)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    static public Vector3 GetMousePositionWithY(float y)
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        return GetMousePositionOnPlane(plane);
    }
}