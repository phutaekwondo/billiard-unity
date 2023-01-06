using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCueBallProvider : MonoBehaviour
{
    public GameObject m_AvailableAreaSurface; //drag and drop here in inspector
    public GameObject m_rack; //drag and drop here in inspector
    public GameObject m_cueBall; //drag and drop here in inspector

    private float m_maxZ, m_minZ, m_maxX, m_minX;

    private void Start() 
    {
        m_maxX = m_AvailableAreaSurface.transform.position.x + m_AvailableAreaSurface.transform.localScale.x / 2;
        m_minX = m_AvailableAreaSurface.transform.position.x - m_AvailableAreaSurface.transform.localScale.x / 2;
        m_maxZ = m_AvailableAreaSurface.transform.position.z + m_AvailableAreaSurface.transform.localScale.z / 2;
        m_minZ = m_AvailableAreaSurface.transform.position.z - m_AvailableAreaSurface.transform.localScale.z / 2;
    }

    public Vector3 NearestPositionInAvailableArea( Vector3 position )
    {
        Vector3 nearestPosition = position;
        if ( position.x > m_maxX ) nearestPosition.x = m_maxX;
        if ( position.x < m_minX ) nearestPosition.x = m_minX;
        if ( position.z > m_maxZ ) nearestPosition.z = m_maxZ;
        if ( position.z < m_minZ ) nearestPosition.z = m_minZ;
        return nearestPosition;
    }

    public Vector3 NearestAvailablePosition(Vector3 cueBallPositionUpdate)
    {
        //avoid cueball is out of table
        Vector3 nearestPosition = NearestPositionInAvailableArea(cueBallPositionUpdate);
        //avoid cueball is overlap with other game objects
        //NEED TO IMPLEMENT

        return nearestPosition; //just a placeholder
    }
}
