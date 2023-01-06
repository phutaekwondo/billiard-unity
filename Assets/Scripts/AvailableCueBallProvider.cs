using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCueBallProvider : MonoBehaviour
{
    public GameObject m_AvailableAreaSurface; //drag and drop here in inspector
    public GameObject m_rack; //drag and drop here in inspector
    public GameObject m_cueBall; //drag and drop here in inspector

    public Vector3 NearestAvailablePosition(Vector3 cueBallPositionUpdate)
    {
        //avoid cueball is out of table
        //avoid cueball is overlap with other game objects
        //NEED TO IMPLEMENT

        return cueBallPositionUpdate; //just a placeholder
    }
}
