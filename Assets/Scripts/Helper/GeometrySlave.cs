using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometrySlave : MonoBehaviour
{
    [SerializeField] GameObject m_cueBall;
    [SerializeField] GameObject m_rack;
    [SerializeField] GameObject m_posZLongRail1;
    [SerializeField] GameObject m_posZLongRail2;
    [SerializeField] GameObject m_negZLongRail1;
    [SerializeField] GameObject m_negZLongRail2;
    [SerializeField] GameObject m_posXShortRail;
    [SerializeField] GameObject m_negZShortRail;

    private List<LineSegment2D> m_railLineSegments = new List<LineSegment2D>();
    private float m_ballRadius = GameConfig.m_ballRadius;
    public static float FULL_ROUND_DEGREE = 360f;

    //public method
    public static Vector2 To2D(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
    public static Vector3 To3D(Vector2 vec, float y)
    {
        return new Vector3(vec.x,y,vec.y);
    }
    public List<LineSegment2D> GetRailLineSegments2D()
    {
        if (m_railLineSegments.Count == 0)
        {
            CalculateLineSegments();
        }
        return m_railLineSegments;
    }
    public List<Circle> SortCirlcesByDistanceWithCueBall(List<Circle> dangerousCircles)
    {
        return SortCirlcesByDistanceWithPoint(dangerousCircles, To2D(m_cueBall.transform.position));
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
            Circle ballCircle = new Circle(To2D(ball.transform.position), m_ballRadius*2);
            dangerousCircles.Add(ballCircle);
        }
        return dangerousCircles;
    }

    //PRIVATE METHOD
    private void Start() 
    {
        //calculate rail line segments
        if (m_railLineSegments.Count == 0)
        {
            CalculateLineSegments();
        }
    }

    private void CalculateLineSegments()
    {
        //posZLongRail1
        Transform now = m_posZLongRail1.transform;
        Vector2 start = new Vector2(now.position.x - now.lossyScale.x/2, now.position.z - now.lossyScale.z/2 - m_ballRadius);
        Vector2 end   = new Vector2(now.position.x + now.lossyScale.x/2, now.position.z - now.lossyScale.z/2 - m_ballRadius);
        m_railLineSegments.Add(new LineSegment2D(start, end));
        //posZLongRail2
        now = m_posZLongRail2.transform;
        start = new Vector2(now.position.x - now.lossyScale.x/2, now.position.z + now.lossyScale.z/2 + m_ballRadius);
        end   = new Vector2(now.position.x + now.lossyScale.x/2, now.position.z + now.lossyScale.z/2 + m_ballRadius);
        m_railLineSegments.Add(new LineSegment2D(start, end));
        //negZLongRail1
        now = m_negZLongRail1.transform;
        start = new Vector2(now.position.x - now.lossyScale.x/2, now.position.z + now.lossyScale.z/2 + m_ballRadius);
        end   = new Vector2(now.position.x + now.lossyScale.x/2, now.position.z + now.lossyScale.z/2 + m_ballRadius);
        m_railLineSegments.Add(new LineSegment2D(start, end));
        //negZLongRail2
        now = m_negZLongRail2.transform;
        start = new Vector2(now.position.x - now.lossyScale.x/2, now.position.z - now.lossyScale.z/2 - m_ballRadius);
        end   = new Vector2(now.position.x + now.lossyScale.x/2, now.position.z - now.lossyScale.z/2 - m_ballRadius);
        m_railLineSegments.Add(new LineSegment2D(start, end));
        //posXShortRail
        now = m_posXShortRail.transform;
        start = new Vector2(now.position.x + now.lossyScale.x/2 + m_ballRadius,now.position.z - now.lossyScale.z/2);
        end   = new Vector2(now.position.x + now.lossyScale.x/2 + m_ballRadius,now.position.z + now.lossyScale.z/2);
        m_railLineSegments.Add(new LineSegment2D(start, end));
        //negZShortRail
        now = m_negZShortRail.transform;
        start = new Vector2(now.position.x - now.lossyScale.x/2 - m_ballRadius, now.position.z - now.lossyScale.z/2);
        end   = new Vector2(now.position.x - now.lossyScale.x/2 - m_ballRadius, now.position.z + now.lossyScale.z/2);
        m_railLineSegments.Add(new LineSegment2D(start, end));
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
    public class StraightRay2D
    {
        public Vector2 m_start;
        public Vector2 m_direction;
        public StraightRay2D(Vector2 start, Vector2 direction)
        {
            m_start = start;
            m_direction = direction;
        }
        public bool IsCut(LineSegment2D seg)
        {
            return seg.IsCut(this);
        }
    }
    public class LineSegment2D
    {
        public Vector2 m_start;
        public Vector2 m_end;
        public LineSegment2D(Vector2 start, Vector2 end)
        {
            m_start = start;
            m_end = end;
        }
        public bool IsCut(StraightRay2D ray)
        {
            float shouldBeTotal = Vector2.Angle(m_start-ray.m_start, m_end - ray.m_start);
            float total = 
                Vector2.Angle(m_start - ray.m_start,ray.m_direction) 
                + Vector2.Angle(m_end-ray.m_start, ray.m_direction);

            return total <= shouldBeTotal;
        }
        
        public Vector2? CutPoint(StraightRay2D ray)
        {
            //here some math formula
            // C: ray start
            // V: Ray direction
            // A: line segment start
            // B: line segment end
            // name the point we finding as D
            // D = C + tV
            //      Cx(Ay-By) + Cy(Bx-Ax) + AxBy - AyBx
            // t =  -----------------------------------
            //      Vx(By-Ay) + Vy(Ax-Bx)
            if (IsCut(ray)) return null;
            Vector2 C = ray.m_start;
            Vector2 V = ray.m_direction;
            Vector2 A = m_start;
            Vector2 B = m_end;
            float t = (C.x * (A.y - B.y) + C.y * (B.x - A.x) + A.x * B.y - A.y * B.x) / (V.x * (B.y - A.y) + V.y * (A.x - B.x));
            return C + (t * V);
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
