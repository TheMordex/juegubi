using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnBasedCombat : MonoBehaviour
{
    private CharacterController heroController;
    private CharacterController enemyController;

    private TurnManager turnManager;

    [Header("Configuración")]
    public float turnDelay = 2f;

    [Header("UI")]
    public Slider heroHealthBar;
    public Slider enemyHealthBar;
    public TextMeshProUGUI heroHealthText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI statusText;
    public GameObject endScreen;
    public TextMeshProUGUI endMessageText;

    [Header("Botones")]
    public Button attackButton;
    public Button defendButton;
    public Button healButton;
    //public Button fortifyButton;
    //public Button stunButton;
    public Button restartButton;
    public Button quitButton;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;
    public AudioSource sfxSource;
    public AudioClip attackSFX;
    public AudioClip healSFX;

    [Header("Referencias visuales")]
    public CharacterView heroView;
    public CharacterView enemyView;

    void Start()
    {
        turnManager = new TurnManager();

        // Inicializamos MVC de héroe y enemigo
        CharacterModel heroModel = new CharacterModel("Héroe", 100, 20);
        CharacterModel enemyModel = new CharacterModel("Enemigo", 80, 15);

        heroController = new CharacterController(heroModel, heroView);
        enemyController = new CharacterController(enemyModel, enemyView);

        // Inicializamos UI y botones
        heroView.Setup(heroHealthBar, heroHealthText);
        enemyView.Setup(enemyHealthBar, enemyHealthText);
        heroController.UpdateView();
        enemyController.UpdateView();

        attackButton.onClick.AddListener(() => OnHeroAction(ActionType.Attack));
        healButton.onClick.AddListener(() => OnHeroAction(ActionType.Heal));
        defendButton.onClick.AddListener(() => OnHeroAction(ActionType.Defend));
        //fortifyButton.onClick.AddListener(() => OnHeroAction(ActionType.Fortify));
        //stunButton.onClick.AddListener(() => OnHeroAction(ActionType.Stun));

        restartButton.onClick.AddListener(RestartBattle);
        quitButton.onClick.AddListener(QuitGame);

        endScreen.SetActive(false);
        statusText.text = "Comienza la batalla. ¡Elige una acción!";

        if (backgroundMusic != null)
            backgroundMusic.Play();
    }

    void OnHeroAction(ActionType action)
    {
        if (!turnManager.IsHeroTurn())
            return;

        DisableButtons();

        switch (action)
        {
            case ActionType.Attack:
                sfxSource.PlayOneShot(attackSFX);
                heroController.Attack(enemyController);
                break;

            case ActionType.Heal:
                sfxSource.PlayOneShot(healSFX);
                heroController.Heal(15);
                break;

            case ActionType.Defend:
                statusText.text = "El héroe se defiende.";
                break;

            case ActionType.Fortify:
                statusText.text = "El héroe se fortalece.";
                break;

            case ActionType.Stun:
                statusText.text = "El héroe intenta aturdir al enemigo.";
                break;
        }

        CheckBattleState();
        if (!enemyController.IsDead())
        {
            turnManager.NextTurn();
            StartCoroutine(EnemyTurnCoroutine());
        }
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        yield return new WaitForSeconds(turnDelay);
        EnemyTurn();
    }

    void EnemyTurn()
    {
        if (enemyController.IsDead())
            return;

        statusText.text = "El enemigo ataca!";
        sfxSource.PlayOneShot(attackSFX);

        enemyController.Attack(heroController);
        heroController.UpdateView();

        CheckBattleState();

        if (!heroController.IsDead())
        {
            turnManager.NextTurn();
            StartCoroutine(HeroTurnCoroutine());
        }
    }

    private IEnumerator HeroTurnCoroutine()
    {
        yield return new WaitForSeconds(turnDelay);
        if (!heroController.IsDead())
        {
            statusText.text = "Tu turno. Elige una acción.";
            EnableButtons();
        }
    }

    void CheckBattleState()
    {
        if (heroController.IsDead())
        {
            EndBattle("Derrota");
        }
        else if (enemyController.IsDead())
        {
            EndBattle("Victoria");
        }
    }

    void EndBattle(string result)
    {
        statusText.text = $"Fin del combate: {result}";
        endMessageText.text = result;
        endScreen.SetActive(true);
        DisableButtons();

        if (backgroundMusic != null && backgroundMusic.isPlaying)
            backgroundMusic.Stop();

        if (sfxSource != null)
        {
            if (result == "Victoria" && victoryMusic != null)
                sfxSource.PlayOneShot(victoryMusic);
            else if (result == "Derrota" && defeatMusic != null)
                sfxSource.PlayOneShot(defeatMusic);
        }
    }

    void DisableButtons()
    {
        attackButton.interactable = false;
        defendButton.interactable = false;
        healButton.interactable = false;
        //fortifyButton.interactable = false;
        //stunButton.interactable = false;
    }

    void EnableButtons()
    {
        attackButton.interactable = true;
        defendButton.interactable = true;
        healButton.interactable = true;
        //fortifyButton.interactable = true;
        //stunButton.interactable = true;
    }

    void RestartBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void QuitGame()
    {
        SceneManager.LoadScene(1);
    }
}
