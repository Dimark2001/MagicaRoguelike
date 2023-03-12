using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWindow : Singleton<GameplayWindow>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Scrollbar hpScrollbar;
    private void Start()
    {
        canvas.worldCamera = Camera.main;
        var eventGameManager = EventGameManager.Instance;
        eventGameManager.OnCoinChange += UpdateCoin;
        eventGameManager.OnPlayerHpChange += UpdateHp;
        UpdateCoin();
        UpdateHp();
    }

    private void UpdateHp()
    {
        var lm = LevelManager.Instance;
        hpText.text = lm.player.hp.ToString();
        hpScrollbar.size = (float)lm.player.hp / (float)lm.player.maxHp;
    }
    
    private void UpdateCoin()
    {
        var lm = LevelManager.Instance;
        coinText.text = lm.Coins.ToString();
    }
}
