using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("점프 설정")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float landingDuration = 0.3f;

    [Header("공격 설정")]
    public bool canMoveWhileAttacking = false;

    [Header("무기 설정")]
    public WeaponType currentWeapon = WeaponType.Fist;
    public enum WeaponType { Fist, Sword, Gun }

    [System.Serializable]
    public class WeaponData
    {
        public WeaponType type;
        public float attackDuration;
        public float cooldown;
        public string attackTriggerName;
    }

    public List<WeaponData> weaponDataList;

    [Header("컴포넌트")]
    public Animator animator;
    private CharacterController controller;
    private Camera playerCamera;

    private float currentSpeed;
    private bool isAttacking = false;
    private bool isLanding = false;
    private float landingTimer;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;
    private float attackCooldownTimer;

    [Header("총 설정")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float bulletSpeed = 25f;
    public float gunFireDelay = 0.4f;

    [Header("근접 무기 히트박스")]
    public Collider fistCollider;
    public Collider swordCollider;
    public float meleeActiveTime = 0.3f;

    public Camera cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        currentAmmo = maxAmmo;

        if (fistCollider) fistCollider.enabled = false;
        if (swordCollider) swordCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleCursorLock();

        HandleWeaponSwitch();
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
    }

    void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeapon = WeaponType.Fist;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeapon = WeaponType.Sword;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeapon = WeaponType.Gun;
    }

    void HandleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
                isAttacking = false;
            return;
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking && attackCooldownTimer <= 0)
        {
            WeaponData weapon = weaponDataList.Find(w => w.type == currentWeapon);
            if (weapon == null) return;

            isAttacking = true;
            attackTimer = weapon.attackDuration;
            attackCooldownTimer = weapon.cooldown;

            if (animator != null && !string.IsNullOrEmpty(weapon.attackTriggerName))
                animator.SetTrigger(weapon.attackTriggerName);

            // 기 타입에 따라 동작 분기
            if (currentWeapon == WeaponType.Gun)
            {


                //총은 애니메이션이 재생된 후 일정 시간 뒤에 발사
                StartCoroutine(ShootGunAfterAnimation(gunFireDelay));
            }
            else
            {
                StartCoroutine(ActivateMeleeHitbox(currentWeapon == WeaponType.Fist ? fistCollider : swordCollider));
            }
        }
    }

    IEnumerator ShootGunAfterAnimation(float delay)
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("탄환이 없습니다!");
            yield break;
        }

        //애니메이션 재생 후 기다림
        yield return new WaitForSeconds(delay);

        // 총알 생성 시점
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

        //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.velocity = firePoint.forward * bulletSpeed;
        //Destroy(bullet, 3f);

        currentAmmo--;
        Debug.Log($"총 발사! 남은 탄환: {currentAmmo}");
    }


    IEnumerator ActivateMeleeHitbox(Collider hitbox)
    {
        if (hitbox == null) yield break;

        hitbox.enabled = true;
        yield return new WaitForSeconds(meleeActiveTime);
        hitbox.enabled = false;
    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            if (!wasGrounded)
            {
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0)
                isLanding = false;
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null)
                animator.SetTrigger("jumpTrigger");
        }

        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        if (isAttacking && !canMoveWhileAttacking)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            if(!CameraSwitcher.isFirstPerson)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            currentSpeed = 0;
        }
    }

    void UpdateAnimator()
    {
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("Speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);
    }

    public void SetCursorLock(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    public void ToggleCursorLock()
    {
        bool shouldLock = Cursor.lockState != CursorLockMode.Locked;
        SetCursorLock(shouldLock);
    }

    public void SetUIMode(bool uiMode)
    {
        SetCursorLock(!uiMode);
    }
}
