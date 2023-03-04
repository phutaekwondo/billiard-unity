using UnityEngine;
using System.Collections.Generic;

public class OverlappingChecker : MonoBehaviour
{
    //VARIABLES
    private List<GameObject> m_overlappingObjects;
    private bool m_isOverlapping = false;
    //REFERENCES
    //PUBLIC METHODS
    public bool IsOverlapping()
    {
        return m_isOverlapping;
    }
    public List<GameObject> GetOverlappingObjects()
    {
        return m_overlappingObjects;
    }
    //PRIVATE METHODS
    private void Start() 
    {
        m_overlappingObjects = new List<GameObject>();
    }
    private void Update() 
    {
        if ( m_overlappingObjects.Count > 0 )
        {
            m_isOverlapping = true;
        }
        else
        {
            m_isOverlapping = false;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        //if other.gameObject is not in m_overlappingObjects, add it
        if ( !m_overlappingObjects.Contains(other.gameObject) )
        {
            m_overlappingObjects.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        //if other.gameObject is in m_overlappingObjects, remove it
        if ( m_overlappingObjects.Contains(other.gameObject) )
        {
            m_overlappingObjects.Remove(other.gameObject);
        }
    }
}
