using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [Header("Button Settings")]
    public Button[] buttons;
    public Animator[] buttonAnimators; // Each button has its own Animator
    public float inputCooldown = 0.2f;
    public float handSmooth = 10f;

    [Header("Hand Settings")]
    public RectTransform leftHand;
    public RectTransform rightHand;
    public Vector2 padding = new Vector2(60f, 25f);
    
    public float handMoveSmooth = 10f;
    public Vector2 handOffsetR = new Vector2(0f, -50f);
    public Vector2 handOffsetL = new Vector2(0f, -50f);

    [Header("Audio")]
    public AudioSource navigateSound;
    public AudioSource selectSound;

    private int index = 0;
    private float cooldownTimer = 0f;

    private RectTransform[] buttonRects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonRects = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
            buttonRects[i] = buttons[i].GetComponent<RectTransform>();

        // Initialize first button as selected
        SetSelected(index, true);
        MoveHandsInstant();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        HandleInput();
        MoveHandsSmooth();
    }

    void HandleInput()
    {
        if (cooldownTimer > 0f) return;
        bool changed = false;

        // Move Down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            changed = true;
            SetSelected(index, false);
            index++;
            if (index >= buttons.Length) index = 0;
            SetSelected(index, true);
        }

        // Move Up
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changed = true;
            SetSelected(index, false);
            index--;
            if (index < 0) index = buttons.Length - 1;
            SetSelected(index, true);
        }

        // Submit / Click
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            selectSound?.Play();
            buttonAnimators[index].SetTrigger("pressed");
            buttons[index].onClick.Invoke();
        }

        if (changed)
        {
            navigateSound?.Play();
            cooldownTimer = inputCooldown;
        }
    }

    void SetSelected(int i, bool value)
    {
        if (buttonAnimators[i])
            buttonAnimators[i].SetBool("selected", value);
    }

    void MoveHandsSmooth()
    {
        if (!leftHand || !rightHand) return;

        RectTransform targetButton = buttonRects[index];
        Vector2 targetCenterL = targetButton.anchoredPosition + handOffsetL;
        Vector2 targetCenterR = targetButton.anchoredPosition + handOffsetR;
        Vector2 targetSize = targetButton.sizeDelta + padding;

        leftHand.anchoredPosition = Vector2.Lerp(leftHand.anchoredPosition, targetCenterL, Time.deltaTime * handSmooth);
        rightHand.anchoredPosition = Vector2.Lerp(rightHand.anchoredPosition, targetCenterR, Time.deltaTime * handSmooth);

        leftHand.sizeDelta = Vector2.Lerp(leftHand.sizeDelta, targetSize, Time.deltaTime * handSmooth);
        rightHand.sizeDelta = Vector2.Lerp(rightHand.sizeDelta, targetSize, Time.deltaTime * handSmooth);
    }

    void MoveHandsInstant()
    {
        if (!leftHand || !rightHand) return;
        RectTransform targetButton = buttonRects[index];
        Vector2 targetCenter = targetButton.anchoredPosition + handOffsetL;

        Vector2 targetSize = targetButton.sizeDelta + padding;

        leftHand.anchoredPosition = targetCenter;
        rightHand.anchoredPosition = targetCenter;
        leftHand.sizeDelta = targetSize;
        rightHand.sizeDelta = targetSize;
    }

    // Called by Event Trigger on Hover
    public void OnButtonHover(int newIndex)
    {
        if (index == newIndex) return;

        SetSelected(index, false);
        index = newIndex;
        SetSelected(index, true);
        navigateSound?.Play();
    }


}
