using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;


    [Header("Inventory UI")]
    public RectTransform inventoryPanel;
    public Transform ItemHolder;
    public GameObject InventoryItemPrefab;
    public float animationDuration = 0.5f;
    private bool invOnScreen = false;
    public TMP_Text ButtonText;

    public Transform invOnScreenPosition;
    public Transform invOffScreenPosition;

    [Header("Clue UI")]
    public RectTransform cluePanel;
    public GameObject cluePrefab;
    public float clueAnimDuration = 0.5f;
    public bool clueOnScreen = false;

    public Transform clueOnScreenPosition;
    public Transform clueOffScreenPosition;

    InputSystem_Actions input;

    private void Awake()
    {
        Instance = this;

        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.UI.OpenInventory.Enable();
        input.UI.OpenClue.Enable();
    }

    private void OnDisable()
    {
        input.UI.OpenInventory.Disable();
        input.UI.OpenClue.Disable();
    }

    private void Update()
    {
        if(input.UI.OpenInventory.WasPressedThisFrame())
        {
            ToggleInventoryPanel();
        }

        if(input.UI.OpenClue.WasPressedThisFrame() )
        {
            ToggleCluePanel();
        }
    }

    public void ItemAdded(Item itemToAdd)
    {
        ItemUI itemUI = Instantiate(InventoryItemPrefab, ItemHolder.transform).GetComponent<ItemUI>();
        itemUI.spriteRenderer.sprite = itemToAdd.ItemSprite;
    }

    public void ItemRemoved(int indexToRemove)
    {
        // destroy the item at the specified index
        Destroy(ItemHolder.transform.GetChild(indexToRemove));
    }


    public void ToggleInventoryPanel()
    {
        if(clueOnScreen)
        {
            ToggleCluePanel();
        }

        if (invOnScreen)
        {
            inventoryPanel.DOAnchorPos(invOffScreenPosition.position, animationDuration);
            ButtonText.text = "v";
        }
        else
        {
            inventoryPanel.DOAnchorPos(invOnScreenPosition.position, animationDuration);
            ButtonText.text = "^";
        }

        invOnScreen = !invOnScreen;
    }

    public void ToggleCluePanel()
    {
        if(invOnScreen)
        {
            ToggleInventoryPanel();
        }

        if(clueOnScreen)
        {
            cluePanel.DOAnchorPos(clueOffScreenPosition.position, animationDuration);
            //ButtonText.text = "v";
        }
        else
        {
            cluePanel.DOAnchorPos(clueOnScreenPosition.position, animationDuration);
            //ButtonText.text = "^";
        }

        clueOnScreen = !clueOnScreen;
    }
}
