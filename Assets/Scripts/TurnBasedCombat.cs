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

        CharacterModel heroModel = new CharacterModel("Héroe", 100, 20);
        CharacterModel enemyModel = new CharacterModel("Enemigo", 80, 15);

        heroController = new CharacterController(heroModel, heroView);
        enemyController = new CharacterController(enemyModel, enemyView);

        heroView.Setup(heroHealthBar, heroHealthText);
        enemyView.Setup(enemyHealthBar, enemyHealthText);

        heroController.UpdateView();
        enemyController.UpdateView();

        attackButton.onClick.AddListener(() => OnHeroAction(ActionType.Attack));
        healButton.onClick.AddListener(() => OnHeroAction(ActionType.Heal));
        defendButton.onClick.AddListener(() => OnHeroAction(ActionType.Defend));

        restartButton.onClick.AddListener(RestartBattle);
        quitButton.onClick.AddListener(QuitGame);

        endScreen.SetActive(false);
        statusText.text = "Comienza la batalla. ¡Elige una acción!";

        if (backgroundMusic != null)
            backgroundMusic.Play();
    }

    void OnHeroAction(ActionType action)
    {
        // Actualizar efectos antes de actuar
        heroController.UpdateStatusEffects();
        enemyController.UpdateStatusEffects();

        if (!turnManager.IsHeroTurn())
            return;

        if (heroController.model.IsStunned)
        {
            statusText.text = "Estás aturdido y pierdes el turno.";
            heroController.model.IsStunned = false;
            turnManager.NextTurn();
            StartCoroutine(EnemyTurnCoroutine());
            return;
        }

        DisableButtons();

        switch (action)
        {
            case ActionType.Attack:
                sfxSource.PlayOneShot(attackSFX);
                heroController.Attack(enemyController);

                // ✨ Efecto visual enemigo dañado
                enemyView.PlayShake();
                enemyView.PlayDamageFlash();
                break;

            case ActionType.Heal:
                sfxSource.PlayOneShot(healSFX);
                heroController.Heal(30);
                break;

            case ActionType.Defend:
                statusText.text = "El héroe se defiende.";
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
        heroController.UpdateStatusEffects();
        enemyController.UpdateStatusEffects();

        if (enemyController.IsDead())
            return;

        if (enemyController.model.IsStunned)
        {
            statusText.text = "¡El enemigo está aturdido y pierde el turno!";
            enemyController.model.IsStunned = false;
            StartCoroutine(HeroTurnCoroutine());
            return;
        }

        int randomChoice = Random.Range(0, 100);

        if (randomChoice < 60)
        {
            statusText.text = "¡El enemigo ataca!";
            sfxSource.PlayOneShot(attackSFX);
            enemyController.Attack(heroController);

            // ✨ Efecto visual héroe dañado
            heroView.PlayShake();
            heroView.PlayDamageFlash();
        }
        else if (randomChoice < 80)
        {
            statusText.text = "¡El enemigo te envenena!";
            var poison = StatusEffectFactory.CreateEffect("Poised"); 
            heroController.ApplyStatus(poison);
        }
        else if (randomChoice < 90)
        {
            statusText.text = "¡El enemigo intenta aturdirte!";
            var stun = StatusEffectFactory.CreateEffect("Stun");
            heroController.ApplyStatus(stun);
        }
        else
        {
            statusText.text = "¡El enemigo se fortalece!";
            var fortify = StatusEffectFactory.CreateEffect("Fortify");
            enemyController.ApplyStatus(fortify);
        }

        heroController.UpdateView();
        enemyController.UpdateView();

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
    }

    void EnableButtons()
    {
        attackButton.interactable = true;
        defendButton.interactable = true;
        healButton.interactable = true;
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
