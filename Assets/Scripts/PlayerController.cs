using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    private CharacterController characterController;

    public GameObject[] weapons;
    private int currentWeapon;

    public Transform cameraTransform;

    public float walkingSpeed = 7f;
    public float mouseSens = 10f;
    public float jumpSpeed = 10f;

    private float verticalAngle;
    private float horizontalAngle;
    private float verticalSpeed;

    private bool isGrounded;
    private float groundedTimer;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGrounded = true;
        groundedTimer = 0f;

        verticalSpeed = 0f;
        verticalAngle = 0f;
        horizontalAngle = transform.localEulerAngles.y;

        characterController = GetComponent<CharacterController>();

        currentWeapon = 0;
        UpdateWeapon();
    }

    private void Update()
    {
        if (characterController.isGrounded == false) 
        {
            if (isGrounded)
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)
                {
                    isGrounded =false;
                }
            }
        }
        else
        {
            isGrounded = true;
            groundedTimer = 0;
        }

        // 점프
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
        }

        // 평행이동
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }
        move = move * walkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);
        characterController.Move(move);

        // 좌/우 마우스
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens;
        horizontalAngle += turnPlayer;

        if (horizontalAngle > 360f)
        {
            horizontalAngle -= 360f;
        }
        if (horizontalAngle < 0f)
        {
            horizontalAngle += 360f;
        }

        Vector3 currentAngle = transform.localEulerAngles;
        currentAngle.y = horizontalAngle;
        transform.localEulerAngles = currentAngle;

        // 상/하 마우스
        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);
        currentAngle = cameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraTransform.localEulerAngles = currentAngle;

        // 낙하
        verticalSpeed -= 10f * Time.deltaTime;
        if (verticalSpeed < -10f)
        {
            verticalSpeed = -10f;
        }
        Vector3 verticalMove = new Vector3(0f, verticalSpeed, 0f);
        verticalMove = verticalMove * Time.deltaTime;
        CollisionFlags flag= characterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
        {
            verticalSpeed = 0;
        }

        // 무기 변경
        if (Input.GetButtonDown("ChangeWeapon"))
        {
            currentWeapon++;
            if (currentWeapon >= weapons.Length)
            {
                currentWeapon = 0;
            }
            UpdateWeapon();
        }
    }

    private void UpdateWeapon()
    {
        foreach (GameObject w in weapons)
        {
            w.SetActive(false);
        }

        weapons[currentWeapon].SetActive(true);
    }

    public void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("TellIamDie", 1f);
    }

    void TellIamDie()
    {
        GameManager.instance.GameOverScene();
    }
}