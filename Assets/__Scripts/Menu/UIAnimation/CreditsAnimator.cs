using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CreditsAnimator : MonoBehaviour
{
    private MenuInputActions menuInputActions;
    
    public Vector3 startPositionOffset;
    public float animationTime;
    public float appearTime;
    public float accelerateFactor;

    public GameObject panel;
    public GameObject camera;
    public GameObject particles;
    public GameObject backButton;
    public GetPresetChip rewarder;

    private bool isShowing;
    private CanvasGroup canvasGroup;
    private CanvasGroup panelCanvasGroup;
    private Tweener panelMovement;
    private Tweener canvasAppear;
    private Tweener particleAppear;
    private Tweener scrollStart;

    void Awake()
    {
        menuInputActions = new MenuInputActions();
        menuInputActions.UI.Enable();
        
        isShowing = false;
        camera.SetActive(false);
        particles.SetActive(false);
        // ToggleButton(false);
        backButton.SetActive(false);
        canvasGroup = GetComponent<CanvasGroup>();
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();

        EventCenter.GetInstance().AddEventListener(Global.Events.MenuCreditsHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuCreditsShow, Show);
        menuInputActions.UI.CreditScrenExit.performed += OnCancel;
        menuInputActions.UI.CreditScrollAccelerate.started += OnAccelerate;
        menuInputActions.UI.CreditScrollAccelerate.canceled += OnNormalSpeed;

        SetUpTweenerMove();
        SetUpTweenerAlphaLerp();
    }

    // public void ToggleButton(bool isEnabled)
    // {
    //     var button = backButton.GetComponent<Button>();
    //     button.interactable = isEnabled;
    //     var handler = backButton.GetComponent<MainMenuButtonHoverHandler>();
    //     handler.enabled = isEnabled;
    // }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!isShowing) return;
        
        Debug.Log("CreditsAnimator: OnCancel");
        Global.MainMenu.menuController.CreditsToMain();
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (!isShowing) return;
        
        panelMovement.timeScale = accelerateFactor;
    }
    
    public void OnNormalSpeed(InputAction.CallbackContext context)
    {
        if (!isShowing) return;
        
        panelMovement.timeScale = 1;
    }
    
    public void Hide()
    {
        particleAppear.Kill();
        scrollStart.Kill();

        isShowing = false;
        camera.SetActive(false);
        particles.SetActive(false);
        // ToggleButton(false);
        backButton.SetActive(false);

        DOTween.To(()=> panelCanvasGroup.alpha, x=> panelCanvasGroup.alpha = x, 0f, 0.2f).SetEase(Ease.OutQuad);
        
        canvasAppear.timeScale = 3;
        canvasAppear.PlayBackwards();
        DOTween.To(() => 0, x => _ = x, 1f, appearTime)
            .OnComplete(() => { panelMovement.Pause(); });
    }

    public void Show()
    {
        panelCanvasGroup.alpha = 1;
        
        panelMovement.Restart();
        panelMovement.Pause();
        camera.SetActive(true);
        backButton.SetActive(true);

        canvasAppear.isBackwards = false;
        canvasAppear.Restart();
        
        particleAppear = DOTween.To(() => 0, x => _ = x, 1f, appearTime)
            .OnComplete(() =>
            {
                particles.SetActive(true);
            });
        scrollStart = DOTween.To(() => 0, x => _ = x, 1f, appearTime * 1.5f)
            .OnComplete(() =>
            {
                // ToggleButton(true);
                particles.SetActive(true);
                panelMovement.isBackwards = false;
                panelMovement.Play();
                isShowing = true;
            });
    }
    
    private void SetUpTweenerMove()
    {
        var localPosition = panel.transform.localPosition;
        panelMovement = panel.transform.DOLocalMove(localPosition, animationTime)
            .From(localPosition + startPositionOffset).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                backButton.SetActive(true);
                
                int prefKey = PlayerPrefs.GetInt("GetCreditMenuChip", 0);
                if (prefKey == 0)
                {
                    rewarder?.GetChip();
                    PlayerPrefs.SetInt("GetCreditMenuChip", 1);
                    Global.MainMenu.playerDataManager.SaveToDisk();
                }
            });
        panelMovement.SetAutoKill(false).Pause();
    }

    private void SetUpTweenerAlphaLerp()
    {
        canvasAppear = DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 1f, appearTime)
            .SetEase(Ease.InCubic).From(0f).SetEase(Ease.OutQuad);
        canvasAppear.SetAutoKill(false).Pause();
    }
}
