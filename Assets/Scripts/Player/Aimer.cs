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
    private Vector3 m_aimDirection = Vector3.zero;

    //public method
    public void SetAbleToChangeAimDirection(bool enable)
    {
        m_isAiming = enable;
    }
    public Vector3 GetAimingDirection()
    {
        return m_aimDirection;
    }
    public void SetAimingVisibility(bool visible)
    {
        throw new System.NotImplementedException();
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
    }
    private void UpdateAimingComponents()
    {
        List<Circle> targetBallHitZones = m_geometrySlave.GetDangerousCircles();
        targetBallHitZones = m_geometrySlave.SortCirlcesByDistanceWithCueBall(targetBallHitZones);
        //update imagination ball position
        //aimline 
        //cueball move direction
        //tagetball move direction
        throw new System.NotImplementedException();
    }
    private Tuple<Circle?,bool> HittedTargetCirlce(List<Circle> hitZones)
    {
        Vector2 startPoint = new Vector2(m_cueBall.transform.position.x, m_cueBall.transform.position.z);
        Vector2 aimDireciton = new Vector2(m_aimDirection.x, m_aimDirection.z);
        foreach (Circle hitZone in hitZones)
        {
            Vector2 prjCenterOnAimLine = (Vector2)Vector3.Project(hitZone.m_center - startPoint, aimDireciton) + startPoint;
            if (hitZone.IsContain(prjCenterOnAimLine)) //hit
            {
                return new Tuple<Circle?, bool>(hitZone,true);
            }
        }
        return new Tuple<Circle?,bool>(null, false);
    }
}
