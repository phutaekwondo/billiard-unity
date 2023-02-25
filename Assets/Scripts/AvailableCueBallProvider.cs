using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCueBallProvider : MonoBehaviour
{
    public GameObject m_AvailableAreaSurface; //drag and drop here in inspector
    public GameObject m_rack; //drag and drop here in inspector
    public GameObject m_cueBall; //drag and drop here in inspector

    private float m_ballRadius;
    private float m_maxZ, m_minZ, m_maxX, m_minX;

    public Vector3 NearestPositionInAvailableArea( Vector3 position )
    {
        Vector3 nearestPosition = position;
        if ( position.x > m_maxX ) nearestPosition.x = m_maxX;
        if ( position.x < m_minX ) nearestPosition.x = m_minX;
        if ( position.z > m_maxZ ) nearestPosition.z = m_maxZ;
        if ( position.z < m_minZ ) nearestPosition.z = m_minZ;
        return nearestPosition;
    }

    public Vector3 NearestAvailablePosition(Vector3 rawPosition)
    {
        //NEED TO IMPLEMENT
        Vector2 rawPosition2D = new Vector2(rawPosition.x, rawPosition.z);


        //Get group of dangerous circles
        List<Circle> dangerousCircles  = GetDangerousCircles();

        if (IsPositionAvailable(rawPosition2D, dangerousCircles))
        {
            return rawPosition;
        }

        dangerousCircles = SortCirlcesByDistanceWithPoint(dangerousCircles, rawPosition2D);

        foreach ( Circle circle in dangerousCircles )
        {
            Vector2 nearestAvailablePosition = GetNearestAvailablePositionWithCircle(circle, rawPosition2D, dangerousCircles);
        }

        //we need to remove this when we finish the algoritm
        //avoid cueball is out of table
        Vector3 nearestPosition = NearestPositionInAvailableArea(rawPosition);

        return nearestPosition; //just a placeholder
    }

    //PRIVATE METHODS
    private bool IsPositionAvailable(Vector2 position, List<Circle> dangerousCircles)
    {
        if ( position.x > m_maxX ) return false;
        if ( position.x < m_minX ) return false;
        if ( position.y > m_maxZ ) return false;
        if ( position.y < m_minZ ) return false;

        foreach ( Circle circle in dangerousCircles )
        {
            if (Vector2.Distance(position, circle.m_center) < circle.m_radius)
            {
                return false;
            }
        }

        return true;
    }

    private Vector2 GetNearestAvailablePositionWithCircle(Circle circle, Vector2 rawPosition2D, List<Circle> dangerousCircles)
    {
        List<OverlapRange> overlapRanges = new List<OverlapRange>();
        foreach( Circle otherCircle in dangerousCircles )
        {
            //mid angle should between 0 and 360

        }

    }

    private List<Circle> SortCirlcesByDistanceWithPoint(List<Circle> dangerousCircles, Vector2 rawPosition)
    {
        float[] distances = new float[dangerousCircles.Count];
        float[] indexes = new float[dangerousCircles.Count];

        for (int i = 0; i < dangerousCircles.Count; i++)
        {
            distances[i] = Vector2.Distance(dangerousCircles[i].m_center, rawPosition);
            indexes[i] = i;
        }

        //sort distances
        for (int i = 0; i < distances.Length; i++)
        {
            for (int j = i + 1; j < distances.Length; j++)
            {
                if (distances[i] > distances[j])
                {
                    float temp = distances[i];
                    distances[i] = distances[j];
                    distances[j] = temp;

                    temp = indexes[i];
                    indexes[i] = indexes[j];
                    indexes[j] = temp;
                }
            }
        }

        List<Circle> sortedCircles = new List<Circle>();
        for (int i = 0; i < indexes.Length; i++)
        {
            sortedCircles.Add(dangerousCircles[(int)indexes[i]]);
        }
        return sortedCircles;
    }

    private List<Circle> GetDangerousCircles()
    {
        List<Circle> dangerousCircles = new List<Circle>();
        //for each ball in rack
        for ( int i = 0; i < m_rack.transform.childCount; i++ )
        {
            GameObject ball = m_rack.transform.GetChild(i).gameObject;
            Circle ballCircle = new Circle((Vector2)ball.transform.position, m_ballRadius*2);
            dangerousCircles.Add(ballCircle);
        }
        return dangerousCircles;
    }

    private void Start() 
    {
        m_maxX = m_AvailableAreaSurface.transform.position.x + m_AvailableAreaSurface.transform.localScale.x / 2;
        m_minX = m_AvailableAreaSurface.transform.position.x - m_AvailableAreaSurface.transform.localScale.x / 2;
        m_maxZ = m_AvailableAreaSurface.transform.position.z + m_AvailableAreaSurface.transform.localScale.z / 2;
        m_minZ = m_AvailableAreaSurface.transform.position.z - m_AvailableAreaSurface.transform.localScale.z / 2;

        m_ballRadius = m_cueBall.transform.localScale.x / 2;
    }

    private void Update()
    {
        //DEBUG
        // Debug.Log(GetDangerousCircles().Count);
    }

    private struct Circle
    {
        public Vector2 m_center;
        public float m_radius;

        public Circle(Vector2 center, float radius)
        {
            this.m_center = center;
            this.m_radius = radius;
        }
    }
    struct OverlapRange
    {
        //should be in degree
        public float m_min;
        public float m_max;

        public OverlapRange(float min, float max)
        {
            this.m_min = min;
            this.m_max = max;
        }
    }
}
