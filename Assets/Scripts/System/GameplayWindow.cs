using System;
using DG.Tweening;
using Gameplay.Character;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayWindow : Singleton<GameplayWindow>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Scrollbar hpScrollbar;
    [SerializeField] private Scrollbar bossHpScrollbar;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject gameOver;
    
    private bool _isPause = false;
    private void Start()
    {
        canvas.worldCamera = Camera.main;
        var eventGameManager = EventGameManager.Instance;
        eventGameManager.OnCoinChange += UpdateCoin;
        eventGameManager.OnPlayerHpChange += UpdateHp;
        eventGameManager.OnBossSpawn += BossHpActive;
        eventGameManager.OnBossHpChange += UpdateBossHp;
        eventGameManager.OnBossDead += BossHpDeActive;
        eventGameManager.OnGetItem += ShowGetItem;
        UpdateCoin();
        UpdateHp();
        UpdateBossHp(null);
        
        eventGameManager.OnPause += Pause;
        if(LevelManager.Instance.currentLevel == 0)
            EnterGame();

        eventGameManager.OnPlayerDead += () => { gameOver.SetActive(true);};
    }
    private void OnDisable()
    {
        //pauseInput.action.Disable();
        var eventGameManager = EventGameManager.Instance;
        eventGameManager.OnCoinChange -= UpdateCoin;
        eventGameManager.OnPlayerHpChange -= UpdateHp;
        eventGameManager.OnBossSpawn -= BossHpActive;
        eventGameManager.OnBossHpChange -= UpdateBossHp;
        eventGameManager.OnBossDead -= BossHpDeActive;
        eventGameManager.OnGetItem -= ShowGetItem;
        eventGameManager.OnPause -= Pause;
    }

    private void Pause()
    {
        print("pasue");
        if (_isPause)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
            _isPause = false;
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            _isPause = true;
        }
    }

    public void EnterGame()
    {
        print("EnterGame");
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void StartGame()
    {
        print("StartGame");
        pauseMenu.SetActive(false);
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void RestartGame()
    {
        LevelManager.Instance.Coins = 0;
        EventGameManager.Instance.OnCoinChange?.Invoke();
        AbilityManager.Instance.ResetItem();
        Player.Instance.Hp = Player.Instance.maxHp;
        EnemySpawner.Instance.ResetTime();
        DeadBodyCleaner.Instance.Clear();
        var lm = LevelManager.Instance;
        lm.currentLevel = 6;
        SceneManager.LoadScene(lm.currentLevel);
        EnterGame();
    }
    

    private void ShowGetItem(string itemName)
    {
        var inVal = 0f;
        itemText.text = itemName;
        DOTween.To(() => inVal, x => inVal = x, 1, 4).OnComplete(() =>
        {
            itemText.text = "";
        });
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
        if (enemyController == null)
        {
            bossHpScrollbar.size = 1;
            return;
        }
        bossHpScrollbar.size = (float)enemyController.Hp / (float)enemyController.maxHp;
    }
    
    private void UpdateCoin()
    {
        var lm = LevelManager.Instance;
        coinText.text = lm.Coins.ToString();
    }
}
