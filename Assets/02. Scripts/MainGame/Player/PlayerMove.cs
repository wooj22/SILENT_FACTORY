using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // �̵� �ӵ�
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float crouchSpeed = 1.5f;
    [SerializeField] float jumpHeight = 2f;

    // ȸ�� �ӵ�
    [SerializeField] float lookSpeedX = 2f;
    [SerializeField] float lookSpeedY = 2f;

    // ������Ʈ
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

        // Rigidbody�� ȸ�� ���� ��Ȱ��ȭ
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

    /// ������
    private void MovePlayer()
    {
        // �ӵ� set
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

    /// ����
    private void Jump()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    /// ���� ��ȯ
    private void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        // ���� ȸ�� ����
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        // ī�޶� ���� ȸ��
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // �÷��̾� �¿� ȸ��
        transform.Rotate(Vector3.up * mouseX);
    }
}
