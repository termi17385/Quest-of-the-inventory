using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : SerializedMonoBehaviour
{
    #region Variables
    #region Movement
    [Title("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField, MaxValue(1), Tooltip("Used to tweak the effect of gravity")] private float multiplier;
    #endregion
    #region Private
    private CharacterController cController;
    private float gravity = 9.8f;
    private float vSpeed;
    #endregion
    #region MouseVariables
    [Title("Mouse")]
    [SerializeField] private float mouseSensitivity;
    #endregion
    #region GroundCheck And RayCast
    [FoldoutGroup("GroundCheck and rayCast", false)]
    [SerializeField, ReadOnly] private float offset;
    [FoldoutGroup("GroundCheck and rayCast", false)]
    [SerializeField, ReadOnly] private float radius;
    [FoldoutGroup("GroundCheck and rayCast", false)]
    [SerializeField, ReadOnly] private LayerMask layermask;
    [FoldoutGroup("GroundCheck and rayCast", false)]
    [SerializeField] private float rayDist;
    [FoldoutGroup("GroundCheck and rayCast", false)]
    [ReadOnly] public bool lookingAtNpc = false; 
    #endregion
    #endregion
    private void Start() => cController = GetComponent<CharacterController>();
    private void Update()
    {
        MovePlayer();   
        MouseLook();
        InteractWithNPC();
    }
    
    /// <summary>
    /// Handles enabling dialogue when talking to npc's
    /// </summary>
    private void InteractWithNPC()
    {
        //todo: pause player when they are in dialog so that they dont look around when talking 
        var _transform = transform;
        var _cameraTransform = GetComponentInChildren<Camera>().transform;
        if (Physics.Raycast(_cameraTransform.transform.position, _cameraTransform.transform.forward, out var _hit, (rayDist * 0.5f)))
        {
            if (_hit.collider.CompareTag("NPC"))
            {
                lookingAtNpc = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    var _dialogueToLoad = _hit.collider.GetComponent<Dialogue>();
                    DialogueManager.dManager.LoadDialogue(_dialogueToLoad);
                }
            }
        }
        else lookingAtNpc = false;

        Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * (rayDist * 0.5f), Color.green);
    }
    
    #region Mouse and MovementControls
    private void MouseLook()
    {
        var _sensitivity = (mouseSensitivity * 10) * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Mouse X") * _sensitivity, 0);
    }
    private void MovePlayer()
    {
        #region Movement 
        // sets up the direction of movement for the player
        var _horizontal = Input.GetAxisRaw("Horizontal") * (speed * 10) * Time.deltaTime;
        var _vertical = Input.GetAxisRaw("Vertical") * (speed * 10) * Time.deltaTime;

        var _transform = transform;
        var _moveForward = _transform.forward * _vertical;
        var _moveSideWays = _transform.right * _horizontal;
        #endregion

        // makes sure the player doesnt go faster moving diagonally
        var _moveDirection = (_moveForward + _moveSideWays).normalized; 
        
        // checks if the player is grounded setting downwards speed to zero and letting them jump
        if (GroundCheck())
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space)) vSpeed = jumpSpeed;
        }
        
        // handles the effects of gravity
        else vSpeed -= (gravity * Time.deltaTime) * multiplier;
        _moveDirection.y = vSpeed;
        cController.Move(_moveDirection * Time.deltaTime);
    }
    #endregion
    #region GroundChecking
    /// <summary>
    /// uses a sphere to check if the player is touching the ground layer
    /// </summary>
    private bool GroundCheck()
    {
        var position = transform.position;
        position.y -= offset;
        return Physics.CheckSphere(position, radius, layermask);
    }
    /// <summary>
    /// Visualises the ground check
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * offset), radius);
    }
    #endregion
}
