using UnityEngine;

public class RackPhysicsController : MonoBehaviour
{
    //REFERENCES
    private GameObject m_rack;

    //PUBLIC METHODS
    public void SetAllBallsIsKinematic( bool isKinematic )
    {
        Rigidbody[] rigidbodies = this.transform.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }
    }

    //PRIVATE METHODS
    private void Start() {
        m_rack = this.transform.gameObject;
    }
}
