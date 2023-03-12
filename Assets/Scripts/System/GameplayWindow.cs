using System;
using DG.Tweening;
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
    [SerializeField] private InputActionReference pauseInput;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject menu;
    
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
        pauseInput.action.performed += Pause;
        if(LevelManager.Instance.currentLevel == 0)
            EnterGame();
    }

    private void OnEnable()
    {
        pauseInput.action.Enable();
    }
    
    private void OnDestroy()
    {
        pauseInput.action.Disable();
    }
    
    private void Pause(InputAction.CallbackContext obj)
    {
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
        pauseMenu.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void StartGame()
    {
        pauseMenu.SetActive(false);
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void RestartGame()
    {
        var lm = LevelManager.Instance;
        lm.currentLevel = 6;
        SceneManager.LoadScene(lm.currentLevel);
        EnterGame();
    }
    
    private void OnDisable()
    {
        var eventGameManager = EventGameManager.Instance;
        eventGameManager.OnCoinChange -= UpdateCoin;
        eventGameManager.OnPlayerHpChange -= UpdateHp;
        eventGameManager.OnBossSpawn -= BossHpActive;
        eventGameManager.OnBossHpChange -= UpdateBossHp;
        eventGameManager.OnBossDead -= BossHpDeActive;
        eventGameManager.OnGetItem -= ShowGetItem;
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
        bossHpScrollbar.size = (float)enemyController.Hp / (float)enemyController.maxHp;
    }
    
    private void UpdateCoin()
    {
        var lm = LevelManager.Instance;
        coinText.text = lm.Coins.ToString();
    }
}
