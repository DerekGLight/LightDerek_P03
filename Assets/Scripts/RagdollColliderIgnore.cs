using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollColliderIgnore : MonoBehaviour
{
    [SerializeField] bool _ignoreGrandparent = false;
    [SerializeField] private Collider _mapGround;

    private Vector3 origin = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent != null)
        {
            //Debug.Log("Part " + transform.name + " parent: " + transform.parent.name);
            if(transform.parent.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }
        }   
        if(transform.childCount > 0)
        {
            //Debug.Log("Part " + transform.name + " child count: " + transform.childCount);
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

    private void Update()
    {
        float distFromZero = Vector3.Distance(origin, transform.position);
        if(distFromZero >= 28)
        {
            Physics.IgnoreCollision(_mapGround, GetComponent<Collider>(), true);
        }
        else
        {
            Physics.IgnoreCollision(_mapGround, GetComponent<Collider>(), false);
        }
    }
}
