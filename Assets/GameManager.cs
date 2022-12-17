using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RackGenerator;

public class GameManager : MonoBehaviour
{
    RackGenerator m_rackGenerator = new RackGenerator();
    public GameObject m_rack = null; // Drag and drop the rack prefab here


    private void Start() 
    {
        
    }
}
