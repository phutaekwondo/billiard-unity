using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCueBallProvider : MonoBehaviour
{
    public GameObject m_AvailableAreaSurface; //drag and drop here in inspector
    public GameObject upperRail;
    public GameObject lowerRail;
    public GameObject leftRail;
    public GameObject rightRail;
    public GameObject m_rack; //drag and drop here in inspector
    public GameObject m_cueBall; //drag and drop here in inspector

    private float m_ballRadius;
    private float m_maxY, m_minY, m_maxX, m_minX;
    private const float FULL_ROUND_DEGREE = 360f;

    public Vector3 NearestPositionInAvailableArea( Vector3 position )
    {
        Vector3 nearestPosition = position;
        if ( position.x > m_maxX ) nearestPosition.x = m_maxX;
        if ( position.x < m_minX ) nearestPosition.x = m_minX;
        if ( position.z > m_maxY ) nearestPosition.z = m_maxY;
        if ( position.z < m_minY ) nearestPosition.z = m_minY;
        return nearestPosition;
    }

    public Vector3 NearestAvailablePosition(Vector3 rawPosition)
    {
        Vector3 nearestPositionInArea = NearestPositionInAvailableArea(rawPosition);
        Vector2 rawPosition2D = new Vector2(nearestPositionInArea.x, nearestPositionInArea.z);


        //Get group of dangerous circles
        List<Circle> dangerousCircles  = GetDangerousCircles();

        if (IsPositionAvailable(rawPosition2D, dangerousCircles))
        {
            return new Vector3(rawPosition2D.x, 0, rawPosition2D.y);
        }

        dangerousCircles = SortCirlcesByDistanceWithPoint(dangerousCircles, rawPosition2D);

        foreach ( Circle circle in dangerousCircles )
        {
            if (circle.isInCircle(rawPosition2D))
            {
                Tuple<Vector2,bool> tupleNearestAvailablePosition = GetNearestAvailablePositionWithCircle(circle, rawPosition2D, dangerousCircles);
                if (tupleNearestAvailablePosition.Item2)
                {
                    return new Vector3(tupleNearestAvailablePosition.Item1.x, rawPosition.y, tupleNearestAvailablePosition.Item1.y);
                }
            }   
        }

        //we need to remove this when we finish the algoritm
        //avoid cueball is out of table
        return nearestPositionInArea; //just a placeholder
    }

    //PRIVATE METHODS
    private bool IsPositionAvailable(Vector2 position, List<Circle> dangerousCircles)
    {
        if ( position.x > m_maxX ) return false;
        if ( position.x < m_minX ) return false;
        if ( position.y > m_maxY ) return false;
        if ( position.y < m_minY ) return false;

        foreach ( Circle circle in dangerousCircles )
        {
            if (circle.isInCircle(position))
            {
                return false;
            }
        }

        return true;
    }

    private Tuple<Vector2,bool> GetNearestAvailablePositionWithCircle(Circle circle, Vector2 rawPosition2D, List<Circle> dangerousCircles)
    {
        List<OverlapRange> overlapRanges = GetListOfOverlapRanges(circle, dangerousCircles);

        bool isFullyOverlapCircle = false;

        foreach ( OverlapRange range in overlapRanges )
        {
            if (range.IsFullRound())
            {
                isFullyOverlapCircle = true;
                break;
            }
        }

        if (isFullyOverlapCircle)
        {
            return new Tuple<Vector2, bool>(rawPosition2D, false);
        }
        else
        {
            //find the nearest position on the circle
            Vector2 nearestPosition;
            float angleWithCenterOfCircle = circle.AngleWithPoint(rawPosition2D);  
            nearestPosition = circle.GetPointWithAngle(angleWithCenterOfCircle);
            //the nearest angle 
            foreach ( OverlapRange overlapRange in overlapRanges )
            {
                if (overlapRange.IsContain(angleWithCenterOfCircle))
                {
                    float nearestAngle;
                    if (overlapRange.m_min == 0 || overlapRange.m_max == FULL_ROUND_DEGREE)//overlapRange at start or end
                    {
                        bool isOverlapRangeAtStart = overlapRange.m_min == 0;
                        bool hasOtherRange = false;
                        float maxAngle = 0, minAngle = 0;
                        //find end range
                        foreach (OverlapRange findingOtherRange in overlapRanges)
                        {
                            if (findingOtherRange.m_max == FULL_ROUND_DEGREE && isOverlapRangeAtStart)
                            {
                                hasOtherRange = true;
                                maxAngle = overlapRange.m_max;
                                minAngle = findingOtherRange.m_min - FULL_ROUND_DEGREE;
                            }
                            else if (findingOtherRange.m_min == 0 && !isOverlapRangeAtStart)
                            {
                                hasOtherRange = true;
                                maxAngle = findingOtherRange.m_max + FULL_ROUND_DEGREE;
                                minAngle = overlapRange.m_min;
                            }
                        }
                        if (hasOtherRange)
                        {
                            if (Math.Abs(minAngle - angleWithCenterOfCircle) < Math.Abs(maxAngle - angleWithCenterOfCircle))
                            {
                                nearestAngle = minAngle;
                            }
                            else
                            {
                                nearestAngle = maxAngle;
                            }

                            if (nearestAngle < 0)
                            {
                                nearestAngle += FULL_ROUND_DEGREE;
                            }
                            else if (nearestAngle > FULL_ROUND_DEGREE)
                            {
                                nearestAngle -= FULL_ROUND_DEGREE;
                            }
                        }
                        else
                        {
                            nearestAngle = overlapRange.GetNearestLimit(angleWithCenterOfCircle);
                        }
                    }
                    else
                    {
                        nearestAngle = overlapRange.GetNearestLimit(angleWithCenterOfCircle);
                    }
                    nearestPosition = circle.GetPointWithAngle(nearestAngle);
                    return new Tuple<Vector2, bool>(nearestPosition, true);
                }
            }
            return new Tuple<Vector2, bool>(nearestPosition, true);
        }
    }

    private List<OverlapRange> GetListOfOverlapRanges(Circle circle, List<Circle> dangerousCircles)
    {
        List<OverlapRange> overlapRanges = new List<OverlapRange>();
        //check rails
        //max Y
        if (Math.Abs(m_maxY - circle.m_center.y) < circle.m_radius) // if MaxY line cut the circle
        {
            float halfWideAngle = Mathf.Acos((m_maxY - circle.m_center.y) / circle.m_radius) * Mathf.Rad2Deg;
            if (halfWideAngle > 90)
            {
                overlapRanges.Add(new OverlapRange(0, 90 + halfWideAngle));
                overlapRanges.Add(new OverlapRange(FULL_ROUND_DEGREE - (90 - halfWideAngle), FULL_ROUND_DEGREE));
            }
            else
            {
                overlapRanges.Add(new OverlapRange(90 - halfWideAngle, 90 + halfWideAngle));
            }
        }
        //min Y
        if (Math.Abs(m_minY - circle.m_center.y) < circle.m_radius) // if MinY line cut the circle
        {
            float halfWideAngle = Mathf.Acos((circle.m_center.y - m_minY) / circle.m_radius) * Mathf.Rad2Deg;
            if (halfWideAngle > 90)
            {
                overlapRanges.Add(new OverlapRange( 270 - halfWideAngle, FULL_ROUND_DEGREE));
                overlapRanges.Add(new OverlapRange(0,(270 + halfWideAngle)-FULL_ROUND_DEGREE));
            }
            else
            {
                overlapRanges.Add(new OverlapRange(270 - halfWideAngle, 270 + halfWideAngle));
            }
        }
        //max X
        if (Math.Abs(m_maxX - circle.m_center.x) < circle.m_radius) // if MaxX line cut the circle
        {
            float halfWideAngle = Mathf.Acos((m_maxX - circle.m_center.x) / circle.m_radius) * Mathf.Rad2Deg;
            overlapRanges.Add(new OverlapRange(0, halfWideAngle));
            overlapRanges.Add(new OverlapRange(FULL_ROUND_DEGREE - halfWideAngle, FULL_ROUND_DEGREE));
        }
        //min X
        if (Math.Abs(m_minX - circle.m_center.x) < circle.m_radius) // if MinX line cut the circle
        {
            float halfWideAngle = Mathf.Acos((circle.m_center.x - m_minX) / circle.m_radius) * Mathf.Rad2Deg;
            overlapRanges.Add(new OverlapRange(180 - halfWideAngle, 180 + halfWideAngle));
        }

        //check other balls
        foreach( Circle otherCircle in dangerousCircles )
        {
            if (otherCircle.m_center != circle.m_center)
            {
                //mid angle should between 0 and 360
                Vector2 otherCenter = otherCircle.m_center;
                float distanceFrom2Center = Vector2.Distance(circle.m_center, otherCenter);
                float circleRadius = circle.m_radius;

                if (distanceFrom2Center < circleRadius*2)
                {
                    float midAngle = circle.AngleWithPoint(otherCenter);
                    float halfWideAngle = Mathf.Acos((distanceFrom2Center/2) / circleRadius) * Mathf.Rad2Deg;
                    float rangeMin = midAngle - halfWideAngle;
                    float rangeMax = midAngle + halfWideAngle;

                    if (rangeMin < 0)
                    {
                        OverlapRange belowZeroRange = new OverlapRange(360 + rangeMin, 360);
                        overlapRanges.Add(belowZeroRange);
                        rangeMin = 0;
                    }
                    if (rangeMax > 360)
                    {
                        OverlapRange above360Range = new OverlapRange(0, rangeMax - 360);
                        overlapRanges.Add(above360Range);
                        rangeMax = 360;
                    }
                    OverlapRange overlapRange = new OverlapRange(rangeMin, rangeMax);
                    overlapRanges.Add(overlapRange);
                }
            }
        }

        List<OverlapRange> mergedRanges = new List<OverlapRange>();
        //merge the overlap ranges
        for ( int i = 0; i < overlapRanges.Count; i++ )
        {
            bool isMerged = false;
            for ( int j = i+1; j < overlapRanges.Count; j++ )
            {
                //is mergeable to i
                if ( overlapRanges[i].IsMergeable(overlapRanges[j]) )
                {
                    isMerged = true;
                    //merge to j
                    overlapRanges[j].MergeFrom(overlapRanges[i]);
                }
            }
            if (!isMerged)
            {
                mergedRanges.Add(overlapRanges[i]);
            }
        }
        return mergedRanges;
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
            Circle ballCircle = new Circle(new Vector2(ball.transform.position.x, ball.transform.position.z), m_ballRadius*2);
            dangerousCircles.Add(ballCircle);
        }
        return dangerousCircles;
    }

    private void Start() 
    {
        m_ballRadius = m_cueBall.transform.localScale.x / 2;
        // m_maxX = m_AvailableAreaSurface.transform.position.x + m_AvailableAreaSurface.transform.localScale.x / 2;
        // m_minX = m_AvailableAreaSurface.transform.position.x - m_AvailableAreaSurface.transform.localScale.x / 2;
        // m_maxY = m_AvailableAreaSurface.transform.position.z + m_AvailableAreaSurface.transform.localScale.z / 2;
        // m_minY = m_AvailableAreaSurface.transform.position.z - m_AvailableAreaSurface.transform.localScale.z / 2;
        m_maxY = upperRail.transform.position.z - (upperRail.transform.localScale.z / 2) - m_ballRadius;
        m_minY = lowerRail.transform.position.z + (lowerRail.transform.localScale.z / 2) + m_ballRadius;
        m_maxX = rightRail.transform.position.x - (rightRail.transform.localScale.z / 2) - m_ballRadius; //get the z scale because the rail is rotated
        m_minX = leftRail.transform.position.x  + (leftRail.transform.localScale.z  / 2) + m_ballRadius; //get the z scale because the rail is rotated
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

        public bool isInCircle(Vector2 point)
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
    class OverlapRange
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
