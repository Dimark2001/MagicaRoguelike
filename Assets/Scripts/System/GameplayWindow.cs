using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWindow : Singleton<GameplayWindow>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Scrollbar hpScrollbar;
    [SerializeField] private Scrollbar bossHpScrollbar;
    private void Start()
    {
        canvas.worldCamera = Camera.main;
        var eventGameManager = EventGameManager.Instance;
        eventGameManager.OnCoinChange += UpdateCoin;
        eventGameManager.OnPlayerHpChange += UpdateHp;
        eventGameManager.OnBossSpawn += BossHpActive;
        eventGameManager.OnBossHpChange += UpdateBossHp;
        eventGameManager.OnBossDead += BossHpDeActive;
        UpdateCoin();
        UpdateHp();
        DontDestroyOnLoad(gameObject);

    }

    private void UpdateHp()
    {
        var lm = LevelManager.Instance;
        hpText.text = lm.player.Hp.ToString();
        hpScrollbar.size = (float)lm.player.Hp / (float)lm.player.maxHp;
    }

    private void BossHpActive()
    {
        bossHpScrollbar.gameObject.SetActive(true);
    }
    
    private void BossHpDeActive()
    {
        bossHpScrollbar.gameObject.SetActive(false);
    }
    
    private void UpdateBossHp(EnemyController enemyController)
    {
        bossHpScrollbar.size = (float)enemyController.Hp / (float)enemyController.maxHp;
    }
    
    private void UpdateCoin()
    {
        var lm = LevelManager.Instance;
        coinText.text = lm.Coins.ToString();
    }
}
