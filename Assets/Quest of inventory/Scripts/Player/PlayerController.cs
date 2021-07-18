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
    #region GroundCheck
    [FoldoutGroup("GroundCheck", false)]
    [SerializeField, ReadOnly] private float offset;
    [FoldoutGroup("GroundCheck", false)]
    [SerializeField, ReadOnly] private float radius;
    [FoldoutGroup("GroundCheck", false)]
    [SerializeField, ReadOnly] private LayerMask layermask;
    #endregion
    #endregion

    private void Start() => cController = GetComponent<CharacterController>();
    private void Update()
    {
        MovePlayer();   
        MouseLook();
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
        var _horizontal = Input.GetAxisRaw("Horizontal") * (speed * 10) * Time.deltaTime;
        var _vertical = Input.GetAxisRaw("Vertical") * (speed * 10) * Time.deltaTime;

        var _transform = transform;
        var _moveForward = _transform.forward * _vertical;
        var _moveSideWays = _transform.right * _horizontal;
        #endregion

        var _moveDirection = (_moveForward + _moveSideWays).normalized;
        if (GroundCheck())
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space)) vSpeed = jumpSpeed;
        }
        else vSpeed -= (gravity * Time.deltaTime) * multiplier;
        _moveDirection.y = vSpeed;
        cController.Move(_moveDirection * Time.deltaTime);
    }
    #endregion

    #region GroundChecking
    private bool GroundCheck()
    {
        var position = transform.position;
        position.y -= offset;
        return Physics.CheckSphere(position, radius, layermask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * offset), radius);
    }
    #endregion
}
