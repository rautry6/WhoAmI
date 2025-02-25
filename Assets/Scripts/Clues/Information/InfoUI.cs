using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public TMP_Text Name;
    public GameObject PinnedSprite;
    private bool pinned;

    public RectTransform boundary; // The area within which the UI moves
    private RectTransform rectTransform;
    private Vector2 minBounds, maxBounds;
    private bool isDragging = false;
    private bool isMoving = true;

    public float moveSpeed = 50f; // Speed of random movement
    private Vector2 randomDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        CalculateBounds();
        StartRandomMovement();
    }

    void Update()
    {
        if (!pinned && !isDragging && isMoving)
        {
            MoveRandomly();
        }

        if (pinned && Input.GetKeyDown(KeyCode.Mouse1))
        {
            InfoUnpinned();
        }

        if (isDragging && Input.GetKeyDown(KeyCode.Mouse1))
        {
            InfoPinned();
        }
    }

    void CalculateBounds()
    {
        // Use boundary's rect (which is in its local space)
        Vector2 boundarySize = boundary.rect.size;

        // Since anchoredPosition is relative to the parent's pivot,
        // get half the size and use that for min/max.
        Vector2 halfSize = boundarySize * 0.45f;

        // If your draggable element's pivot is centered, this works well.
        // Note: We assume both boundary and element use the same coordinate space.
        minBounds = -halfSize;
        maxBounds = halfSize;
    }

    void StartRandomMovement()
    {
        InvokeRepeating(nameof(ChangeDirection), 0f, 1.5f); // Change direction every 1.5 seconds
    }

    void ChangeDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized; // Pick a new random direction
    }

    void MoveRandomly()
    {
        Vector2 newPos = rectTransform.anchoredPosition + randomDirection * moveSpeed * Time.deltaTime;

        // Clamp the position within bounds
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        rectTransform.anchoredPosition = newPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        isMoving = false; // Stop random movement while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(pinned) { return; }

        rectTransform.anchoredPosition += eventData.delta;

        // Clamp within boundary
        Vector2 clampedPos;
        clampedPos.x = Mathf.Clamp(rectTransform.anchoredPosition.x, minBounds.x, maxBounds.x);
        clampedPos.y = Mathf.Clamp(rectTransform.anchoredPosition.y, minBounds.y, maxBounds.y);

        rectTransform.anchoredPosition = clampedPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        isMoving = true; // Resume random movement
    }

    public void InfoPinned()
    {
        pinned = true;
        PinnedSprite.SetActive(true);
    }

    public void InfoUnpinned()
    {
        pinned = false;
        PinnedSprite.SetActive(false);
    }
}
