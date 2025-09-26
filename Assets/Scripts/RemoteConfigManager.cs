using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using System.Threading.Tasks;

public class RemoteConfigManager : MonoBehaviour
{
    struct userAttributes { }
    struct appAttributes { }

    [Header("Referencias del combate")]
    public TurnBasedCombat combat;   // arrastrá aquí el objeto con TurnBasedCombat

    [Header("Variables descargadas")]
    public int enemyDamage;
    public bool isDoubleXP;
    public float backgroundMusicVolume;
    public string victoryMessage;
    public int maxPlayerHealth;

    [Header("Variables de economía")]
    public int goldPerWin;
    public int gemsPerWin;

    async void Start()
    {
        // Inicializar Unity Services + login anónimo
        await InitializeRemoteConfigAsync();

        // Suscribirse y pedir configs
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("[RemoteConfig] Sesión anónima iniciada");
        }
    }

    void ApplyRemoteSettings(ConfigResponse response)
    {
        // 🔽 Descargo las variables de Remote Config (con valores por defecto)
        enemyDamage = RemoteConfigService.Instance.appConfig.GetInt("enemyDamage", 15);
        isDoubleXP = RemoteConfigService.Instance.appConfig.GetBool("isDoubleXP", false);
        backgroundMusicVolume = RemoteConfigService.Instance.appConfig.GetFloat("backgroundMusicVolume", 0.5f);
        victoryMessage = RemoteConfigService.Instance.appConfig.GetString("victoryMessage", "¡Victoria!");
        maxPlayerHealth = RemoteConfigService.Instance.appConfig.GetInt("maxPlayerHealth", 100);

        goldPerWin = RemoteConfigService.Instance.appConfig.GetInt("goldPerWin", 50);
        gemsPerWin = RemoteConfigService.Instance.appConfig.GetInt("gemsPerWin", 1);

        // 🔧 Aplicar cambios en combate
        if (combat != null)
        {
            if (combat.backgroundMusic != null)
                combat.backgroundMusic.volume = backgroundMusicVolume;

            if (combat.heroObject != null)
                combat.heroObject.InitializeCharacter(maxPlayerHealth);
            
            combat.overrideVictoryMessage = victoryMessage;
            combat.enemyBaseDamage = enemyDamage;
            combat.ApplyRemoteConfigOverrides();
        }

        // 🔧 Aplicar cambios en ResourceManager (si lo tenés)
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.goldPerWin = goldPerWin;
            ResourceManager.Instance.gemsPerWin = gemsPerWin;
        }

        Debug.Log($"[RemoteConfig] EnemyDamage={enemyDamage}, XPx2={isDoubleXP}, " +
                  $"Volume={backgroundMusicVolume}, VictoryMsg={victoryMessage}, " +
                  $"MaxHP={maxPlayerHealth}, GoldPerWin={goldPerWin}, GemsPerWin={gemsPerWin}");
    }
}
