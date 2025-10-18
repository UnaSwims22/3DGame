using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour
{
    [Header("Button Settings")]
    public Button[] buttons;         //all buttons assigned here
    public Color normalColor = Color.white; //unselected
    public Color selectedColor = Color.grey;  //selected
    public float responseSpeed = 12f;

    [Header("Hand Settings")]
    public RectTransform rightHand;     //hand sprite goes here!
    public RectTransform leftHand;
    public Vector2 padding = new Vector2(60f, 25f);  //spacing between the hand and button edges
    public float handMoveSmooth = 10f;
    public Vector2 handOffsetR = new Vector2(0f, -50f);
    public Vector2 handOffsetL = new Vector2(0f, -50f);

    private int index = 0;
    private float inputCooldown = 0.15f;  //prevent spamming
    private float cooldownTimer = 0f;
    private RectTransform[] buttonRects;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonRects = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
            buttonRects[i] = buttons[i].GetComponent<RectTransform>();

        UpdateButtonVisuals();
        MoveHandsInstant();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        HandleInput();
        MoveHandsSmooth();
    }

    private void HandleInput()
    {
        if (cooldownTimer > 0f) return;

        bool changed = false;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = (index + 1) % buttons.Length;
            changed = true;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index--;
            if (index < 0) index = buttons.Length - 1;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[index].onClick.Invoke();
            changed = true;
        }

        if (changed)
        {
            cooldownTimer = 1f / responseSpeed;
            UpdateButtonVisuals();
        }
    }
    private void MoveHandsSmooth()
    {
        if (!leftHand || buttonRects.Length == 0) return;

        RectTransform targetButtonL = buttonRects[index];
        Vector2 buttonCenterL = leftHand.parent.InverseTransformPoint(targetButtonL.position);
        Vector2 buttonSizeL = targetButtonL.sizeDelta;
        Vector2 targetPosL = buttonCenterL + handOffsetL;
        Vector2 targetSize = buttonSizeL + padding;

        leftHand.anchoredPosition = Vector2.Lerp(leftHand.anchoredPosition, targetPosL, Time.deltaTime * handMoveSmooth * responseSpeed * 0.1f);
        leftHand.sizeDelta = Vector2.Lerp(leftHand.sizeDelta, targetSize, Time.deltaTime * handMoveSmooth * responseSpeed * 0.1f);

        
        
        if (!rightHand || buttonRects.Length == 0) return;

        RectTransform targetButtonR = buttonRects[index];
        Vector2 buttonCenterR = rightHand.parent.InverseTransformPoint(targetButtonR.position);
        Vector2 buttonSizeR = targetButtonR.sizeDelta;
        Vector2 targetPosR = buttonCenterR + handOffsetR;
        Vector2 targetSizeR = buttonSizeR + padding;

        rightHand.anchoredPosition = Vector2.Lerp(rightHand.anchoredPosition, targetPosR, Time.deltaTime * handMoveSmooth * responseSpeed * 0.1f);
        rightHand.sizeDelta = Vector2.Lerp(rightHand.sizeDelta, targetSize, Time.deltaTime * handMoveSmooth * responseSpeed * 0.1f);
    }


    private void MoveHandsInstant()
    {
        if (!leftHand || buttonRects.Length == 0) return;
        
        RectTransform targetButton = buttonRects[index];
        Vector2 buttonCenter = targetButton.anchoredPosition;
        Vector2 buttonSize = targetButton.sizeDelta;

        leftHand.anchoredPosition = buttonCenter;
        leftHand.sizeDelta = buttonSize + padding;

        
        
        
        if (!rightHand || buttonRects.Length == 0) return;

        RectTransform targetButtonR = buttonRects[index];
        Vector2 buttonCenterR = targetButtonR.anchoredPosition;
        Vector2 buttonSizeR = targetButtonR.sizeDelta;

        rightHand.anchoredPosition = buttonCenter;
        rightHand.sizeDelta = buttonSize + padding;

    }

    private void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == index) ? selectedColor : normalColor;
            buttons[i].colors = colors;
        }
    
    }
}
