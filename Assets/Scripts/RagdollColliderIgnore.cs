using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollColliderIgnore : MonoBehaviour
{
    [SerializeField] bool _ignoreGrandparent = false;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent != null)
        {
            Debug.Log("Part " + transform.name + " parent: " + transform.parent.name);
            if(transform.parent.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }
        }   
        if(transform.childCount > 0)
        {
            Debug.Log("Part " + transform.name + " child count: " + transform.childCount);
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<Collider>() != null)
                {
                    Physics.IgnoreCollision(transform.GetChild(i).GetComponent<Collider>(), GetComponent<Collider>(), true);
                }
            }
        }

        if(_ignoreGrandparent == true)
        {
            Physics.IgnoreCollision(transform.parent.parent.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }
}
