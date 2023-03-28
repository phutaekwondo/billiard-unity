using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GeometrySlave;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject m_simpleSpherePrefab;
    [SerializeField] private GeometrySlave m_geometrySlave;

    private void Start() {
        List<LineSegment2D> railLineSegments = m_geometrySlave.GetRailLineSegments2D();
        Debug.Log("railLineSegments.Count: " + railLineSegments.Count);
        foreach (LineSegment2D railLineSegment in railLineSegments)
        {
            Debug.Log("railLineSegment: " + railLineSegment.m_start + " " + railLineSegment.m_end);
            GameObject sphere = Instantiate(m_simpleSpherePrefab, To3D(railLineSegment.m_start, 0.5f), Quaternion.identity);
            sphere = Instantiate(m_simpleSpherePrefab, To3D(railLineSegment.m_end, 0.5f), Quaternion.identity);
        }
    }
}
