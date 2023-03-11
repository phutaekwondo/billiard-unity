using System.Collections.Generic;
using UnityEngine;

public class GameObjectVisibilitySetter : MonoBehaviour
{
    public List<GameObject> m_hiddenObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        HideObjectsInHiddenObjectsList();
    }

    void HideObjectsInHiddenObjectsList()
    {
        // disable MeshRenderer component on all objects in the list and their child objects, recursively
        foreach (GameObject obj in m_hiddenObjects)
        {
            MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }
}
