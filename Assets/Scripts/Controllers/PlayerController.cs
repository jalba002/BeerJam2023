using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Probably move on the direction of the char.
    // Snap to ground with no gravity at all.
    // Insta-rotate to places. High speeds.

    public float walkingSpeed = 10f;
    public float rotationSpeed = 120f;

    public float activationRange = 1f;
    private Vector2 direction;

    private CharacterController characterController;

    public Camera mainCamera;

    public Transform lookTarget;

    public LayerMask lMask;
    private Vector2 correctedDirection;
    private Transform movementReference;

    private Animator animatorCon;

    private bool mouseClicked = false;

    private void OnEnable()
    {
        // Better than on awake?
        characterController = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
        if (lookTarget == null)
        {
            lookTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            lookTarget.gameObject.name = "[GENERATED] - Player Cursor";
            var col = lookTarget.GetComponent<Collider>();
            if (col != null) Destroy(col);
            //lookTarget.GetComponent<MeshRenderer>().enabled = false; 
        }
        if (movementReference != null) 
        { 
            Destroy(movementReference.gameObject); 
        }
        movementReference = new GameObject("[GENERATED] - Player Movement Reference").transform;
        movementReference.rotation = mainCamera.transform.rotation;
        movementReference.eulerAngles = new Vector3(0f, movementReference.eulerAngles.y, 0f);
    }

    private void Start()
    {
        animatorCon = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        mouseClicked = Input.GetMouseButtonDown(0);
        // Gather direction of movement.
        // Movement dpending on camera pos.
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        correctedDirection = direction.normalized;

        var mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, mainCamera.farClipPlane, lMask))
        {
            lookTarget.position = hit.point;
            var ra = Quaternion.LookRotation(lookTarget.position - transform.position, Vector3.up);
            float rotationTotal = Mathf.Lerp(transform.eulerAngles.y, ra.eulerAngles.y, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0f, rotationTotal, 0f);

            if (mouseClicked)
            {
                float distance = Vector3.Distance(transform.position, hit.collider.bounds.center);
                //Debug.Log("Hit distance! :" + distance);
                //Debug.DrawLine(transform.position, hit.collider.bounds.center, Color.red, 10f);
                if (distance < activationRange)
                {
                    hit.collider.gameObject?.GetComponent<IActivatable>()?.Activate(this.gameObject.transform);
                }
            }
        }

        animatorCon.SetFloat("XSpeed", correctedDirection.x);
        animatorCon.SetFloat("YSpeed", Vector3.Dot(correctedDirection, transform.forward));
    }

    private void LateUpdate()
    {
        // Distance between this and ground then this is the amount that gets moved downwards.
        Vector3 movement = new Vector3(correctedDirection.x * walkingSpeed, Physics.gravity.y, walkingSpeed * correctedDirection.y);         
        movement = movementReference.TransformDirection(movement);
        CollisionFlags collisionFlags = characterController.Move(movement * Time.deltaTime);
    }
}
