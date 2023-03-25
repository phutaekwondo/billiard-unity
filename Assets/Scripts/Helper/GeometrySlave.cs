using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometrySlave : MonoBehaviour
{
    [SerializeField] GameObject m_cueBall;
    [SerializeField] GameObject m_rack;
    private float m_ballRadius = GameConfig.m_ballRadius;
    public static float FULL_ROUND_DEGREE = 360f;

    //public method
    public List<Circle> SortCirlcesByDistanceWithCueBall(List<Circle> dangerousCircles)
    {
        return SortCirlcesByDistanceWithPoint(dangerousCircles, new Vector2(m_cueBall.transform.position.x, m_cueBall.transform.position.z));
    }
    public List<Circle> SortCirlcesByDistanceWithPoint(List<Circle> dangerousCircles, Vector2 rawPosition)
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
    public List<Circle> GetDangerousCircles()
    {
        List<Circle> dangerousCircles = new List<Circle>();
        //for each ball in rack
        for ( int i = 0; i < m_rack.transform.childCount; i++ )
        {
            GameObject ball = m_rack.transform.GetChild(i).gameObject;
            Circle ballCircle = new Circle(new Vector2(ball.transform.position.x, ball.transform.position.z), m_ballRadius*2);
            dangerousCircles.Add(ballCircle);
        }
        return dangerousCircles;
    }

    //sub-classes
    public struct Circle
    {
        public Vector2 m_center;
        public float m_radius;

        public Circle(Vector2 center, float radius)
        {
            this.m_center = center;
            this.m_radius = radius;
        }

        public bool IsContain(Vector2 point)
        {
            float distance = Vector2.Distance(m_center, point);
            return distance <= m_radius;
        }

        public float AngleWithPoint( Vector2 point )
        {
            //angle should between 0 and 36
            float angle = Vector2.Angle(point - this.m_center, Vector2.right);
            if (point.y < this.m_center.y)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        public Vector2 GetPointWithAngle(float angle)
        {
            //angle to vector
            Vector2 angleVector = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector2 point = this.m_center + angleVector * this.m_radius;
            return point;
        }
    }
    public class OverlapRange
    {
        //should be in degree
        public float m_min;
        public float m_max;

        public OverlapRange(float min, float max)
        {
            this.m_min = min;
            this.m_max = max;
        }

        public bool IsMergeable(OverlapRange other)
        {
            return this.IsContain(other.m_min) || this.IsContain(other.m_max) || other.IsContain(this.m_min) || other.IsContain(this.m_max);
        }

        public void MergeFrom(OverlapRange other)
        {
            this.m_max = other.m_max > this.m_max ? other.m_max : this.m_max;
            this.m_min = other.m_min < this.m_min ? other.m_min : this.m_min;
        }

        public bool IsContain(float angle)
        {
            return (angle >= this.m_min && angle <= this.m_max);
        }

        public bool IsFullRound ()
        {
            return this.m_max - this.m_min >= 360;
        }

        public float GetNearestLimit(float angle)
        {
            float minDistance = Mathf.Abs(angle - this.m_min);
            float maxDistance = Mathf.Abs(angle - this.m_max);
            if (minDistance < maxDistance)
            {
                return this.m_min;
            }
            return this.m_max;
        }
    }
}
