using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera cam;
    [SerializeField] private HudInfo hudInfo;
    
    [Header("Pickup Settings")]
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private Transform holdArea;
    [SerializeField] private SpringJoint springJoint;
    private Vector3 _curHoldPosition;

    private GameObject heldObject;
    private Rigidbody heldObjectRB;
    
    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 1.2f;
    [SerializeField] private float pickupForce = 150f;
    [SerializeField] private float obstacleBuffer = 0.1f; //pull back so the object doesn't clip, hopefully

    
    //Draw Line
    private LineRenderer lr;
    
    void Start()
    {
        _curHoldPosition = holdArea.position;
        
        lr = holdArea.GetComponent<LineRenderer>();
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, pickupRange, pickupLayerMask))
        {
            if (hit.transform.root != this.transform.root) // ignore self and children
            {
                Ingredient i = hit.transform.gameObject.GetComponent<Ingredient>();
                if (i != null)
                {
                    hudInfo.UpdateInfoText(i.ingredientSO.displayName);    
                }
            }
        }
        else
        {
            hudInfo.UpdateInfoText("");
        }
        //check if left click pressed
        if (InputRouter.instance.AttackPressed)
        {
            if (heldObject == null && hit.transform != null)
            {
                Debug.Log("Trying to pickup object");
                Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * pickupRange, Color.red, 5f);
                //RaycastHit hit;
                //if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, pickupRange, pickupLayerMask))
                //{
                    if (hit.transform.root != this.transform.root) // ignore self and children
                    {
                        PickupObject(hit.transform.gameObject);
                        /*Debug.Log("Found Object: " + hit.transform.displayName);
                        hudInfo.UpdateInfoText(hit.transform.displayName);*/
                    }
                //}
            }
            
            if (heldObject != null)
            {
                //CalculateNewHoldingPosition();
                //Move Object
                //MoveObject();
                DrawLineBetweenObjects();
            }
            
        }
        else if(heldObject != null)
        {
            DropObject();
        }

        if (heldObject == null)
        {
            lr.enabled = false;
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
        heldObjectRB = obj.GetComponent<Rigidbody>();
        heldObject = obj;

        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = Vector3.zero;   // anchor at the body's own origin
        springJoint.connectedBody = heldObjectRB;

        heldObject.transform.parent = null;

        heldObjectRB.useGravity = true;
        heldObjectRB.linearDamping = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        if (lr != null) lr.positionCount = 2;
        
        lr.enabled = true;
    }
    
    void DropObject()
    {
        /*heldObjectRB.useGravity = true;
        heldObjectRB.linearDamping = 1;
        //might disable this later
        heldObjectRB.constraints = RigidbodyConstraints.None;*/
        springJoint.connectedBody = null;
        
        heldObject.transform.parent = null;
        
        heldObject = null;
        lr.positionCount = 0;
        
    }

    void DrawLineBetweenObjects()
    {
        lr.SetPosition(0, holdArea.transform.position);
        lr.SetPosition(1, heldObject.transform.position);
    }
}
