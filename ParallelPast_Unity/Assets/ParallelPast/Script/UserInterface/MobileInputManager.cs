using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInputManager : MonoBehaviour
{

    private GameManager gameManager;
    private LevelManager levelManager;

    [Header("REFERENCES")]
    [SerializeField]
    private RectTransform _joystick;
    [SerializeField]
    private RectTransform _arrows;
    [SerializeField]
    private RectTransform _jumpButton;
    [SerializeField]
    private RectTransform _interatButton;

    [Space(20)]
    [SerializeField]
    private Image _interactImage;

    private Sprite _interactIcon;

    [Header("PARAMETERS")]
    [SerializeField]
    private float _buttonOpacityOff;
    [SerializeField]
    private float _buttonOpacityOn;

    [Space(10)]
    [SerializeField]
    private float _iconOpacityOff;
    [SerializeField]
    private float _iconOpacityOn;

    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _interactIcon = _interactImage.sprite;

        gameManager = GameManager.Instance;
        levelManager = LevelManager.Instance;

        gameManager.userDataManager.OnChangeGuiSize.AddListener(SetGUISize);
        gameManager.userDataManager.OnChangeControlMod.AddListener(CallChangeControl);

        SetGUISize();
        SetControlType(gameManager.userDataManager.IsArrow);

        levelManager.OnNewGhost.AddListener(ResetInteractButton);
        SetUp(levelManager.CurrentGhost.attachController);
    }

    public void SetUp(PlayerController playerController)
    {
        _playerController = playerController;

        playerController.OnEnterInteraction.AddListener(HighlightInteraction);
        playerController.OnExitInteraction.AddListener(UnhighlightInteraction);
        playerController.OnGetPower.AddListener(SetUpPowerIcon);
    }

    private void OnDisable()
    {
        gameManager.userDataManager.OnChangeGuiSize.RemoveListener(SetGUISize);
    }
    public void SetGUISize()
    {
        _joystick.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.5f, gameManager.userDataManager.GuiScale);
        _arrows.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.5f, gameManager.userDataManager.GuiScale);
        _jumpButton.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.5f, gameManager.userDataManager.GuiScale);
        _interatButton.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.5f, gameManager.userDataManager.GuiScale);

        Vector2 pos = new Vector3(_jumpButton.anchoredPosition.x, _interatButton.anchoredPosition.y);

        _jumpButton.anchoredPosition = pos + new Vector2(0, _interatButton.sizeDelta.y * _jumpButton.localScale.y);
    }
    public void CallChangeControl()
    {
        SetControlType(gameManager.userDataManager.IsArrow);
    }
    public void SetControlType(bool isArrow)
    {
        if (isArrow)
        {
            _joystick.gameObject.SetActive(false);
            _arrows.gameObject.SetActive(true);
        }
        else
        {
            _arrows.gameObject.SetActive(false);
            _joystick.gameObject.SetActive(true);
        }
    }

    public void HighlightInteraction()
    {
        _interactImage.color = new Color(1, 1, 1, _iconOpacityOn);
        _interatButton.GetComponent<Image>().color = new Color(1, 1, 1, _buttonOpacityOn);

        _interactImage.sprite = _interactIcon;
    }

    public void UnhighlightInteraction()
    {
        

        if(_playerController.CurrentPower != null)
        {
            SetUpPowerIcon(_playerController.CurrentPower);
            return;
        }

        _interactImage.color = new Color(1, 1, 1, _iconOpacityOff);
        _interatButton.GetComponent<Image>().color = new Color(1, 1, 1, _buttonOpacityOff);
    }

    public void SetUpPowerIcon(So_CharacterPower power)
    {

        _interactImage.color = new Color(1, 1, 1, _iconOpacityOn);
        _interatButton.GetComponent<Image>().color = new Color(1, 1, 1, _buttonOpacityOn);

        _interactImage.sprite = power.PowerIcon;
    }

    public void ResetInteractButton()
    {
        _interactImage.sprite = _interactIcon;
        UnhighlightInteraction();

        SetUp(levelManager.CurrentGhost.attachController);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
