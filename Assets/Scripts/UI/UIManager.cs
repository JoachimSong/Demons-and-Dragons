using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("Event Listening")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO newGameEvent;
    public VoidEventSO restartGameEvent;
    public VoidEventSO gameWinEvent;
    public VoidEventSO gameFinishEvent;
    public IntEventSO chestOpenEvent;
    public FloatEventSO syncVolumeEvent;
    [Header("Broadcast Event")]
    public VoidEventSO pauseEvent;
    [Header("Component")]
    public GameObject gameOverPanel;
    public GameObject tutorialPanel;
    public GameObject gameWinPanel;
    public GameObject trueEndingPanel;
    public GameObject falseEndingPanel;
    public GameObject storyPanel;
    public GameObject restartBtn;
    public GameObject woodGem;
    public GameObject waterGem;
    public GameObject fireGem;
    public GameObject lightGem;
    public GameObject darkGem;
    public TMP_Text storyText;
    public TMP_Text tutorialText;
    public Button settingsBtn;
    public Button tutorialConfirmBtn;
    public Button storyConfirmBtn;
    public Button saveBtn;
    public Button destoryBtn;
    public GameObject pausePanel;
    public Slider volumeSlider;
    public bool isWin;
    private void Awake()
    {
        settingsBtn.onClick.AddListener(TogglePausePanel);
        tutorialConfirmBtn.onClick.AddListener(ToggleTutorialPanel);
        storyConfirmBtn.onClick.AddListener(ToggleStoryPanel);
        saveBtn.onClick.AddListener(ToggleTEPanel);
        destoryBtn.onClick.AddListener(ToggleFEPanel);
    }

    private void ToggleFEPanel()
    {
        gameWinPanel.SetActive(false);
        falseEndingPanel.SetActive(true);
    }

    private void ToggleTEPanel()
    {
        gameWinPanel.SetActive(false);
        trueEndingPanel.SetActive(true);
    }

    private void ToggleStoryPanel()
    {
        storyPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void ToggleTutorialPanel()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnloadedSceneEvent;
        loadDataEvent.OnEventRaised += OnloadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnloadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
        chestOpenEvent.OnEventRaised += OnChestOpenEvent;
        newGameEvent.OnEventRaised += OnNewGameEvent;
        restartGameEvent.OnEventRaised += OnRestartGameEvent;
        gameWinEvent.OnEventRaised += OnGameWinEvent;
        gameFinishEvent.OnEventRaised += OnGameFinishEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnloadedSceneEvent;
        loadDataEvent.OnEventRaised -= OnloadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnloadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
        chestOpenEvent.OnEventRaised -= OnChestOpenEvent;
        newGameEvent.OnEventRaised -= OnNewGameEvent;
        restartGameEvent.OnEventRaised -= OnRestartGameEvent;
        gameWinEvent.OnEventRaised -= OnGameWinEvent;
        gameFinishEvent.OnEventRaised -= OnGameFinishEvent;
    }
    private void OnGameFinishEvent()
    {
        falseEndingPanel.SetActive(false);
        trueEndingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnGameWinEvent()
    {
        if (isWin == false)
        {
            isWin = true;
            Time.timeScale = 0;
            gameWinPanel.SetActive(true);
        }
    }

    private void OnRestartGameEvent()
    {
        isWin = false;
        gameOverPanel.SetActive(false);
    }

    private void OnNewGameEvent()
    {
        StartCoroutine(StartNewGame());

    }

    IEnumerator StartNewGame()
    {
        isWin = false;
        gameOverPanel.SetActive(false);
        woodGem.GetComponent<Image>().enabled = false;
        waterGem.GetComponent<Image>().enabled = false;
        fireGem.GetComponent<Image>().enabled = false;
        lightGem.GetComponent<Image>().enabled = false;
        darkGem.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(2.3f);
        tutorialPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(tutorialConfirmBtn.gameObject);
        Time.timeScale = 0;
    }
    private void OnChestOpenEvent(int num)
    {
        switch (num)
        {
            case 1:
                storyText.text = "获得道具 木元素宝石\n现在你可以在水中呼吸了";
                woodGem.GetComponent<Image>().enabled = true;
                //woodGem.SetActive(true);
                break;
            case 2:
                storyText.text = "获得道具 水元素宝石\n现在你可以进行二段跳了\n在空中按下 K 键即可再次进行跳跃";
                waterGem.GetComponent<Image>().enabled = true;
                break;
            case 3:
                storyText.text = "获得道具 火元素宝石\n现在你可以进行冲刺了\n在空中按下 U 键即可释放冲刺技能来向前飞行";
                fireGem.GetComponent<Image>().enabled = true;
                break;
            case 4:
                storyText.text = "获得道具 光元素宝石\n现在你可以发光了 尝试向黑暗处前进吧！\n按下 I 键即可释放光芒 再次按下以关闭";
                lightGem.GetComponent<Image>().enabled = true;
                break;
            case 5:
                storyText.text = "获得道具 暗元素宝石\n从黑暗中汲取力量来强化攻击吧！现在你可以发出更强力的攻击\n按下 O 键可以发射死神火球";
                darkGem.GetComponent<Image>().enabled = true;
                break;
            default:
                break;
        }
        storyPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(storyConfirmBtn.gameObject);
        Time.timeScale = 0;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.RaisedEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnloadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnUnloadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }
    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(percentage);
        playerStatBar.OnPowerChange(character);
    }
}
