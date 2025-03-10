using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Name;
    public RectTransform PinnedSprite;
    private bool pinned;

    public RectTransform boundary; // The area within which the UI moves
    private RectTransform rectTransform;
    private Vector2 minBounds, maxBounds;
    private bool isDragging = false;
    private bool isMoving = true;

    public float moveSpeed = 50f; // Speed of random movement
    private Vector2 randomDirection;


    // bool used to determine which ui the player is hovering over
    private bool hoverTarget = false;

    private Camera cam;

    public bool MakingConnection = false;
    public GameObject CurvedLinePrefab;

    private UICurvedLineController curvedLine;

    // info object for ui
    public Info info;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        boundary = transform.parent.GetComponent<RectTransform>();
        cam = Camera.main;
        CalculateBounds();
        StartRandomMovement();

        InfoManager.instance.AddInfo(this);
    }

    void Update()
    {
        if (!pinned && !isDragging && isMoving)
        {
            MoveRandomly();
        }


        if (!hoverTarget)
        {
            return;
        }

        // unpin
        if (pinned && Input.GetKeyDown(KeyCode.Mouse1))
        {
            InfoUnpinned();
        }

        // pin
        if (hoverTarget && Input.GetKeyDown(KeyCode.Mouse1))
        {
            InfoPinned();
        }

        // connect clues

        // early cancel if the connection is already made
        if(curvedLine != null && curvedLine.connectionMade) {
             

            // remove a connection
            if(hoverTarget && Input.GetKeyDown(KeyCode.Mouse1))
            {
                // get rid of previous line
                Destroy(curvedLine.gameObject);

                // alert manager of connection change
                InfoManager.instance.RemoveConnection(this);
            }

            return; 
        
        }

        // connection starting with current info
        if(!MakingConnection && pinned && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // make connection
            InfoManager.instance.MakeConnection(this);

            // alert manager a connection is attempting to be made
            InfoManager.instance.MakingConnection();

            // create and store curved line for later reference before passing it to manager
            curvedLine = Instantiate(CurvedLinePrefab, transform.parent).GetComponent<UICurvedLineController>();
            InfoManager.instance.currentLine = curvedLine;

            // set line start point to current info panel
            InfoManager.instance.currentLine.startUI = PinnedSprite;

            Debug.Log("line");

            return;
        }


        // connection not starting with current info
        if (MakingConnection && hoverTarget && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // return early if trying to make connection with self
            if(InfoManager.instance.currentLine.startUI == PinnedSprite) { return; }

            // pin info is not already
            if (!pinned) { InfoPinned(); }

            // make connection
            InfoManager.instance.MakeConnection(this);

            // update line end point
            InfoManager.instance.currentLine.uiLine.EndPointTransform = PinnedSprite;
            InfoManager.instance.currentLine.connectionMade = true;

            InfoManager.instance.ConnectionMade();

        }
    }

    void CalculateBounds()
    {
        // Use boundary's rect (which is in its local space)
        Vector2 boundarySize = boundary.rect.size;

        // Since anchoredPosition is relative to the parent's pivot,
        // get half the size and use that for min/max.
        Vector2 halfSize = boundarySize * 0.45f;

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
        PinnedSprite.gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTarget = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTarget = false;
    }

    public void InfoUnpinned()
    {
        pinned = false;
        PinnedSprite.gameObject.SetActive(false);
    }

    public void DestroyInfo()
    {
        // info

        Destroy(gameObject);
    }

    public void DestroyLine()
    {
        if (curvedLine != null)
        {
            Destroy(curvedLine.gameObject);
        }
    }
}
