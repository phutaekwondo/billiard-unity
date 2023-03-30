using UnityEngine;
using System;
using System.Collections.Generic;
using static GeometrySlave;

public class Aimer : MonoBehaviour
{
    [SerializeField] GameObject m_cueBall;
    [SerializeField] GameObject m_imaginationBall;
    [SerializeField] GameObject m_aimDirectionLine;
    [SerializeField] GameObject m_cueBallMoveDirectionLine;
    [SerializeField] GameObject m_targetBallMoveDirectionLine;
    [SerializeField] GeometrySlave m_geometrySlave;
    private bool m_isAiming = true; // ablity to chagne aim direction
    private AimVisualizeType m_aimVisualType;
    private Vector3 m_aimDirection = Vector3.zero;
    private Vector2 m_aimDirection2D = Vector2.zero;

    //public method
    public void SetAbleToChangeAimDirection(bool enable)
    {
        m_isAiming = enable;
    }
    public Vector3 GetAimingDirection()
    {
        return m_aimDirection;
    }
    public void SetVisibility(bool visible)
    {
        if (visible)
        {
            m_imaginationBall.            GetComponent<Renderer>().enabled = true;
            m_aimDirectionLine.           GetComponent<Renderer>().enabled = true;
            m_cueBallMoveDirectionLine.   GetComponent<Renderer>().enabled = true;
            m_targetBallMoveDirectionLine.GetComponent<Renderer>().enabled = true;
            if (m_aimVisualType == AimVisualizeType.HitRail)
            {
                m_targetBallMoveDirectionLine.GetComponent<Renderer>().enabled = false;
            }
            else if (m_aimVisualType == AimVisualizeType.HitNothing)
            {
                m_targetBallMoveDirectionLine.GetComponent<Renderer>().enabled = false;
                m_cueBallMoveDirectionLine.   GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            m_imaginationBall.            GetComponent<Renderer>().enabled = false;
            m_aimDirectionLine.           GetComponent<Renderer>().enabled = false;
            m_cueBallMoveDirectionLine.   GetComponent<Renderer>().enabled = false;
            m_targetBallMoveDirectionLine.GetComponent<Renderer>().enabled = false;
        }
    }
    //private method
    private void Update() 
    {
        if (m_isAiming)
        {
            UpdateAimDirection();
            UpdateAimingComponents();
        }
    }
    private void UpdateAimDirection()
    {
        Vector3 direction = MouseTrackingHelper.GetBallOnTablePositionWithMouse() - m_cueBall.transform.position;
        direction.y = 0;
        direction.Normalize();
        m_aimDirection = direction;
        m_aimDirection2D = GeometrySlave.To2D(m_aimDirection);
    }
    private void UpdateAimingComponents()
    {
        List<Circle> targetBallHitZones = m_geometrySlave.GetDangerousCircles();
        targetBallHitZones = m_geometrySlave.SortCirlcesByDistanceWithCueBall(targetBallHitZones);
        Tuple<Circle?,Vector2?> hittedTargetZoneTuple = HittedTargetZone(targetBallHitZones);
        Circle? hittedZone = hittedTargetZoneTuple.Item1;
        if (hittedZone != null) // hit target ball
        {
            m_aimVisualType = AimVisualizeType.HitTargetBall;
            Vector2 hitPoint = hittedTargetZoneTuple.Item2.HasValue ? hittedTargetZoneTuple.Item2.Value : Vector2.zero; // it shouldn't be zero
            //todo: calculate cueball and target ball direction
            //cueball direction
            Vector2 cueballPotentialDirection = Vector2.Perpendicular(hitPoint - hittedZone.Value.m_center);
            Vector2 cueballDirection = 
                ((m_aimDirection2D + cueballPotentialDirection).magnitude > (m_aimDirection2D-cueballPotentialDirection).magnitude) ?
                cueballPotentialDirection : -cueballPotentialDirection;
            cueballDirection.Normalize();
            
            //target ball direction
            Vector2 targetBallDirection = -(hitPoint - hittedZone.Value.m_center).normalized;

            //update aim visualize component
        }
        else //hit the rail or not
        {
            m_aimVisualType = AimVisualizeType.HitRail;
            //todo: find hit point on the rail
            List<LineSegment2D> railLineSegments = m_geometrySlave.GetRailLineSegments2D();
            StraightRay2D cueballShotRay = 
                new StraightRay2D(GeometrySlave.To2D(m_cueBall.transform.position), m_aimDirection2D);

            foreach (LineSegment2D railLineSegment in railLineSegments)
            {
                Vector2? hitPoint = railLineSegment.CutPoint(cueballShotRay);
                if (hitPoint.HasValue)
                {
                    //todo
                }
                else
                {
                    m_aimVisualType = AimVisualizeType.HitNothing;
                }
            }
        }
        SetVisibility(true);
    }
    private Tuple<Circle?,Vector2?> HittedTargetZone(List<Circle> hitZones)
    {
        Vector2 startPoint = GeometrySlave.To2D(m_cueBall.transform.position);
        foreach (Circle hitZone in hitZones)
        {
            Vector2 prjCenterOnAimLine = (Vector2)Vector3.Project(hitZone.m_center - startPoint, m_aimDirection2D) + startPoint;
            if (hitZone.IsContain(prjCenterOnAimLine)) //hit
            {
                float disFromCenterToPrj = Vector2.Distance(prjCenterOnAimLine, hitZone.m_center);
                float disFromPrjToHitPoint = Mathf.Sqrt(Mathf.Pow(hitZone.m_radius,2) - Mathf.Pow(disFromCenterToPrj*disFromCenterToPrj,2));
                Vector2 hitPoint = prjCenterOnAimLine - (m_aimDirection2D.normalized*disFromPrjToHitPoint);
                return new Tuple<Circle?, Vector2?> (hitZone,hitPoint);
            }
        }
        return new Tuple<Circle?,Vector2?> (null,null);
    }

    enum AimVisualizeType
    {
        HitTargetBall,
        HitRail,
        HitNothing
    }
}
