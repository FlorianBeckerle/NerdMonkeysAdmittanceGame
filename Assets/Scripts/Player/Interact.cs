using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera cam;
    
    [Header("Pickup Settings")]
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private Transform holdArea;
    private Vector3 _curHoldPosition;

    private GameObject heldObject;
    private Rigidbody heldObjectRB;
    
    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 1.2f;
    [SerializeField] private float pickupForce = 150f;
    [SerializeField] private float obstacleBuffer = 0.1f; //pull back so the object doesn't clip, hopefully

    void Start()
    {
        _curHoldPosition = holdArea.position;
    }
    void Update()
    {
        //check if left click pressed
        if (InputRouter.instance.AttackPressed)
        {
            if (heldObject == null)
            {
                Debug.Log("Trying to pickup object");
                Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * pickupRange, Color.red, 5f);
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, pickupRange, pickupLayerMask))
                {
                    if (hit.transform.root != this.transform.root) // ignore self and children
                    {
                        PickupObject(hit.transform.gameObject);
                        Debug.Log("Found Object: " + hit.transform.name);
                    }
                }
            }
            
            if (heldObject != null)
            {
                CalculateNewHoldingPosition();
                //Move Object
                MoveObject();
            }
            
        }
        else if(heldObject != null)
        {
            DropObject();
        }

        
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.position) >= 0.1f)
        {
            Vector3 moveDirection = (_curHoldPosition - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickupForce);
        }
    }

    void CalculateNewHoldingPosition()
    {
        float holdDistance = Vector3.Distance(cam.transform.position, holdArea.position);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, holdDistance, obstacleLayerMask))
        {
            //something is blocking the hold position, pull the target point back slightly from the hit surface
            _curHoldPosition = hit.point - cam.transform.forward * obstacleBuffer;
        }
        else
        {
            //nothing blocking, use the normal hold position
            _curHoldPosition = holdArea.position;
        }
    }

    void PickupObject(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("PickupAble"))
        {
            heldObjectRB = obj.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.linearDamping = 10;
            //might disable this later
            heldObjectRB.constraints = RigidbodyConstraints.None;
            
            heldObjectRB.transform.SetParent(holdArea);
            
            heldObject = obj;
        }
    }
    
    void DropObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.linearDamping = 1;
        //might disable this later
        heldObjectRB.constraints = RigidbodyConstraints.None;
        
        heldObject.transform.parent = null;
        
        heldObject = null;
        
    }
}
