using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityStrength = 9.81f;
    private Vector3 currentGravityDirection = Vector3.down;
    private Vector3 pendingGravityDirection = Vector3.down;

    [Header("Hologram Settings")]
    [SerializeField] private Transform hologram;
    [SerializeField] private Transform player;
    [SerializeField] private float hologramDistance = 2f;
    [SerializeField] private float headHeightOffset = 2.3f;
    [SerializeField] private Vector3 hologramRotationOffset = new Vector3(-90, 0, 0);

    private bool isHologramActive = false;
    private bool gravityChangeConfirmed = false;
    private float hologramTimeout = 3f;
    private float lastHologramTime;

    void Start()
    {
        hologram.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { SetPendingGravity(player.forward); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { SetPendingGravity(-player.forward); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { SetPendingGravity(-player.right); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { SetPendingGravity(player.right); }

        if (Input.GetKeyDown(KeyCode.Return) && isHologramActive)
        {
            ApplyGravityChange();
        }

        if (isHologramActive && Time.time - lastHologramTime > hologramTimeout)
        {
            HideHologram();
        }
    }

    void LateUpdate()
    {
        if (gravityChangeConfirmed)
        {
            AlignPlayerWithGravity();
            gravityChangeConfirmed = false;
        }

        UpdateHologramTransform();
    }

    void SetPendingGravity(Vector3 newDirection)
    {
        pendingGravityDirection = newDirection;
        ShowHologram();
        lastHologramTime = Time.time;
    }

    void ApplyGravityChange()
    {
        currentGravityDirection = pendingGravityDirection;
        Physics.gravity = currentGravityDirection * gravityStrength;
        Debug.Log("Gravity Changed to: " + currentGravityDirection);

        gravityChangeConfirmed = true;
        HideHologram();
    }

    void AlignPlayerWithGravity()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.AlignPlayerWithGravity();
        }
    }

    void UpdateHologramTransform()
    {
        Vector3 playerHeadPosition = player.position + player.up * headHeightOffset;
        Vector3 hologramPosition = playerHeadPosition + pendingGravityDirection.normalized * hologramDistance;
        hologram.position = hologramPosition;

        Quaternion targetRotation = Quaternion.LookRotation(pendingGravityDirection, player.up);
        Quaternion offsetRotation = Quaternion.Euler(hologramRotationOffset);
        hologram.rotation = targetRotation * offsetRotation;
    }

    void ShowHologram()
    {
        isHologramActive = true;
        hologram.gameObject.SetActive(true);
        UpdateHologramTransform();
    }

    void HideHologram()
    {
        isHologramActive = false;
        hologram.gameObject.SetActive(false);
    }

    public Vector3 GetGravityDirection()
    {
        return currentGravityDirection;
    }
}
