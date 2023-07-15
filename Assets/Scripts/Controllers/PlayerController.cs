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
    private Vector2 direction;

    private CharacterController characterController;

    public Camera mainCamera;

    public Transform lookTarget;

    public LayerMask lMask;
    private Vector2 correctedDirection;
    private Transform movementReference;

    private Animator animatorCon;

    private void OnEnable()
    {
        // Better than on awake?
        characterController = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
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
        // Gather direction of movement.
        // Movement dpending on camera pos.
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        correctedDirection = direction;
        // 
        var mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, mainCamera.farClipPlane, lMask))
        {
            lookTarget.position = hit.point;
            var ra = Quaternion.LookRotation(lookTarget.position - transform.position, Vector3.up);
            transform.eulerAngles = new Vector3(0f, ra.eulerAngles.y, 0f);
        }
    }

    private void LateUpdate()
    {
        // Distance between this and ground then this is the amount that gets moved downwards.
        Vector3 movement = new Vector3(correctedDirection.x * walkingSpeed, 0f, walkingSpeed * correctedDirection.y);
        animatorCon.SetFloat("XSpeed", correctedDirection.x);
        animatorCon.SetFloat("YSpeed", correctedDirection.y);
         
        movement = movementReference.TransformDirection(movement);
        CollisionFlags collisionFlags = characterController.Move(movement * Time.deltaTime);
    }
}
