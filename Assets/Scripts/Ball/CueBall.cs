using UnityEngine;

public class CueBall : MonoBehaviour
{
    //REFERENCES
    private OverlappingChecker m_overlappingChecker;
    [SerializeField] private RackPhysicsController m_rackPhysicsController;

    //PUBLIC METHODS
    public void SetCueBallPhysicsEnabled(bool isEnabled)
    {
        Rigidbody cueBallRigidbody = this.GetComponent<Rigidbody>();
        if ( !isEnabled )
        {
            cueBallRigidbody.velocity = Vector3.zero;
            cueBallRigidbody.angularVelocity = Vector3.zero;
        }
        cueBallRigidbody.isKinematic = !isEnabled;
        this.GetComponent<Collider>().isTrigger = !isEnabled;
    }
    //PRIVATE METHOS
    private void Start() 
    {
        m_overlappingChecker = this.transform.gameObject.GetComponent<OverlappingChecker>();
    }
    private void Update() 
    {
        m_rackPhysicsController.SetAllBallsIsKinematic(m_overlappingChecker.IsOverlapping());
    }
}
