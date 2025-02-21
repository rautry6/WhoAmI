using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject InventoryItemPrefab;

    public static InventoryUIManager Instance;


    public RectTransform uiPanel;
    public Transform ItemHolder;
    public float animationDuration = 0.5f;
    private bool isOnScreen = false;
    public TMP_Text ButtonText;

    public Transform onScreenPosition;
    public Transform offScreenPosition;

    InputSystem_Actions input;

    private void Awake()
    {
        Instance = this;

        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.UI.OpenInventory.Enable();
    }

    private void OnDisable()
    {
        input.UI.OpenInventory.Disable();
    }

    private void Update()
    {
        if(input.UI.OpenInventory.WasPressedThisFrame())
        {
            TogglePanel();
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


    public void TogglePanel()
    {
        if (isOnScreen)
        {
            uiPanel.DOAnchorPos(offScreenPosition.position, animationDuration);
            ButtonText.text = "v";
        }
        else
        {
            uiPanel.DOAnchorPos(onScreenPosition.position, animationDuration);
            ButtonText.text = "^";
        }

        isOnScreen = !isOnScreen;
    }
}
