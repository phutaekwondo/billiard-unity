using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GeometrySlave;
using static MouseTrackingHelper;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject m_simpleSpherePrefab;
    [SerializeField] private GeometrySlave m_geometrySlave;
    [SerializeField] private GameObject m_cueBall;
    private GameObject m_simpleSphereCutPoint;
    private GameObject m_simpleSphereMouse;
    List<LineSegment2D> m_railLineSegments;

    private void Start() 
    {
        m_simpleSphereCutPoint = Instantiate(m_simpleSpherePrefab);
        m_simpleSphereMouse = Instantiate(m_simpleSpherePrefab);
        // StraightRay2D ray = new StraightRay2D(GeometrySlave.To2D(m_cueBall.transform.position), new Vector2(0,1));
        
        m_railLineSegments = m_geometrySlave.GetRailLineSegments2D();
        foreach (LineSegment2D railLineSegment in m_railLineSegments)
        {
            Debug.Log("railLineSegment: " + railLineSegment.m_start + " " + railLineSegment.m_end);
            GameObject sphere = Instantiate(m_simpleSpherePrefab, To3D(railLineSegment.m_start, 0.5f), Quaternion.identity);
            sphere = Instantiate(m_simpleSpherePrefab, To3D(railLineSegment.m_end, 0.5f), Quaternion.identity);
        //     Vector2? cutpoint = railLineSegment.CutPoint(ray);
        //     if (cutpoint.HasValue)
        //     {
        //         Vector2 cutpointValue = cutpoint.Value;
        //         m_simpleSphereCutPoint = Instantiate(m_simpleSpherePrefab, To3D(cutpointValue, 0.5f), Quaternion.identity);
        //     }
        }
    }
    private void Update() 
    {
        Vector2 mousePosition = To2D(MouseTrackingHelper.GetBallOnTablePositionWithMouse());
        m_simpleSphereMouse.transform.position = To3D(mousePosition, 0.5f);
        Vector2 cuaballPosition = GeometrySlave.To2D(m_cueBall.transform.position);
        StraightRay2D ray = new StraightRay2D(cuaballPosition, mousePosition - cuaballPosition);

        LineSegment2D railLineSegment = m_railLineSegments[0];
        {
            if (railLineSegment.IsCut(ray))
            {
                Debug.Log("cut");
            }
            else
            {
                Debug.Log("not cut");
            }
            Vector2? cutpoint = railLineSegment.CutPoint(ray);
            if (cutpoint.HasValue)
            {
                Vector2 cutpointValue = cutpoint.Value;
                m_simpleSphereCutPoint.transform.position = To3D(cutpointValue, 0.5f);
            }
        }
    }
}
