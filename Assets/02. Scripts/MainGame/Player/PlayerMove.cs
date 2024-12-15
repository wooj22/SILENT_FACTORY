using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 이동 속도
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float crouchSpeed = 1.5f;
    [SerializeField] float jumpHeight = 2f;

    // 회전 속도
    [SerializeField] float lookSpeedX = 2f;
    [SerializeField] float lookSpeedY = 2f;

    // 컴포넌트
    private Rigidbody rb;
    private Camera playerCamera;
    private float currentSpeed;
    private float rotationX = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Rigidbody의 회전 제어 비활성화
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MovePlayer();
        RotateView();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// 움직임
    private void MovePlayer()
    {
        // 속도 set
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 moveDirection = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).normalized;
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);
    }

    /// 점프
    private void Jump()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    /// 방향 전환
    private void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        // 상하 회전 제한
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        // 카메라 상하 회전
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // 플레이어 좌우 회전
        transform.Rotate(Vector3.up * mouseX);
    }
}
