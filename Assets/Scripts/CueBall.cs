using UnityEngine;

public class CueBall : MonoBehaviour
{
    //REFERENCES
    private OverlappingChecker m_overlappingChecker;
    [SerializeField] private RackPhysicsController m_rackPhysicsController;

    //PRIVATE METHOS
    private void Start() 
    {
        m_overlappingChecker = this.transform.gameObject.GetComponent<OverlappingChecker>();
    }
    private void Update() 
    {
        if ( m_overlappingChecker.IsOverlapping() )
        {
            m_rackPhysicsController.SetAllBallsIsKinematic(true);
        }
    }
}
