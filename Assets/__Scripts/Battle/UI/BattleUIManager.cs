using DG.Tweening;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public GameOverPage gameOverPage;
    public GameOverPage gameWinPage;
    public PlayerStatsBarManager playerStatsBarManager;
    public CanvasGroup enterScenePage;
    public CanvasGroup blackCoverCanvasGroup;
    public PowerUpUIManager powerUpUIManager;
    public EnemyPowerUpHint enemyPowerUpHint;

    private void Awake()
    {
        blackCoverCanvasGroup.gameObject.SetActive(false);
        powerUpUIManager.gameObject.SetActive(false);
        enterScenePage.gameObject.SetActive(true);
        enterScenePage.DOFade(0, 3f).From(1).SetEase(Ease.Linear).OnComplete(() =>
        {
            enterScenePage.gameObject.SetActive(false);
            
        });
        Global.Battle.battleUIManager = this;
        gameOverPage.gameObject.SetActive(false);
        
        
    }
}
