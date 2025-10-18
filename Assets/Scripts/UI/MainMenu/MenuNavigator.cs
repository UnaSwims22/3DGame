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
    public RectTransform hands;     //hand sprite goes here!
    public Vector2 padding = new Vector2(60f, 25f);  //spacing between the hand and button edges
    public float handMoveSmooth = 10f;

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
        if (!hands || buttonRects.Length == 0) return;

        RectTransform targetButton = buttonRects[index];
        Vector2 buttonCenter = hands.parent.InverseTransformPoint(targetButton.position);
        Vector2 buttonSize = targetButton.sizeDelta;
        Vector2 targetPos = buttonCenter;
        Vector2 targetSize = buttonSize + padding;

        hands.anchoredPosition = Vector2.Lerp(hands.anchoredPosition, targetPos, Time.deltaTime * handMoveSmooth * responseSpeed * 0.1f);
        hands.sizeDelta = Vector2.Lerp(hands.sizeDelta, targetSize, Time.deltaTime * handMoveSmooth * responseSpeed * 0.1f);
    }

    private void MoveHandsInstant()
    {
        if (!hands || buttonRects.Length == 0) return;

        RectTransform targetButton = buttonRects[index];
        Vector2 buttonCenter = targetButton.anchoredPosition;
        Vector2 buttonSize = targetButton.sizeDelta;

        hands.anchoredPosition = buttonCenter;
        hands.sizeDelta = buttonSize + padding;
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
