using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DarknessMechanic : MonoBehaviour
{
    public float darknessTime;
    public float dayTime;
    public float timeToDamagePlayer;
    public int damageToDealToPlayerInDarkness;
    public float distanceFromCampfireToNotTakeDarknessDamage;

    [Header("References")]
    public Light playerLight;
    public Light directionalLight;
    public Volume globalVolume;
    public RectTransform clockPointer;
    public GameObject clockPanel;

    [Header("Darkness Light Settings")]
    public float baseSkyboxExposure;
    public float directionalLightIntensityInDarkness;
    public float vignetteIntensityInDarkness;
    public float skyboxExposureInDarkness;
    public float playerLightIntensityInDarkness;

    [Header("Internal - Don't Mess")]
    public Vignette globalVolumeVignette;
    public float darkness_t;
    public float damagePlayer_t;
    public bool isDark;
    public bool isPlayerCloseToLightSource;
    public float baseVignetteIntensity;
    public float baseDirectionalLightIntensity;
    public float darknessAnimation_t;
    public bool playingDarknessAnimation;
    public float basePlayerLightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        darkness_t = 10;
        clockPanel.SetActive(true);
        RenderSettings.skybox.SetFloat("_Exposure", baseSkyboxExposure);
        baseDirectionalLightIntensity = directionalLight.intensity;
        globalVolume.profile.TryGet(out globalVolumeVignette);
        baseVignetteIntensity = globalVolumeVignette.intensity.value;
        basePlayerLightIntensity = playerLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.controller.uiController.visivelpause == true) return;

        float dt = Time.deltaTime;

        { // Switch time of day
            darkness_t -= dt;
            if (darkness_t <= 0f)
            {
                isDark = !isDark;
                playingDarknessAnimation = true;
                if (isDark)
                {
                    darkness_t += darknessTime;
                    damagePlayer_t = timeToDamagePlayer;
                    darknessAnimation_t = 0f;
                }
                else
                {
                    darkness_t += dayTime;
                    darknessAnimation_t = 1f;
                }
            }
        }

        { // Update Clock
            if (isDark)
            {
                clockPointer.Rotate(-Vector3.forward, (180f*dt)/darknessTime);
            }
            else
            {
                clockPointer.Rotate(-Vector3.forward, (180f*dt)/dayTime);
            }
        }

        { // Animation
            if (playingDarknessAnimation)
            {
                if (isDark)
                {
                    darknessAnimation_t += dt;
                }
                else if (!isDark)
                {
                    darknessAnimation_t -= dt;
                }
                playingDarknessAnimation = darknessAnimation_t > 0f && darknessAnimation_t < 1f;
                darknessAnimation_t = Mathf.Clamp(darknessAnimation_t, 0f, 1f);
                UpdateDarknessAnimation();
            }
        }

        { // Check if player is close to a light source
            if (isDark && !playingDarknessAnimation)
            {
                bool playerWasCloseToLightSourceLastFrame = isPlayerCloseToLightSource;
                isPlayerCloseToLightSource = LanternaPlayer.lanternaPlayer.ligar;
                if (!isPlayerCloseToLightSource)
                {
                    for (int i = 0; i < GameController.controller.campFires.Length; i++)
                    {
                        if (Vector3.Distance(GameController.controller.Player.transform.position, GameController.controller.campFires[i].transform.position) < distanceFromCampfireToNotTakeDarknessDamage)
                        {
                            isPlayerCloseToLightSource = true;
                            break;
                        }
                    }
                }

                if (!isPlayerCloseToLightSource && playerWasCloseToLightSourceLastFrame)
                {
                    damagePlayer_t = timeToDamagePlayer;
                }
            }
        }

        { // Damage player over time
            if (isDark && !isPlayerCloseToLightSource && !playingDarknessAnimation)
            {
                damagePlayer_t -= dt;
                if (damagePlayer_t <= 0f)
                {
                    GameController.controller.TomaDano(damageToDealToPlayerInDarkness);
                    damagePlayer_t += timeToDamagePlayer;
                }
            }
        }

        { // Light following player
            if (isDark)
            {
                playerLight.transform.position = GameController.controller.Player.transform.position;
            }
        }
    }

    private void OnDestroy()
    {
        RenderSettings.skybox.SetFloat("_Exposure", baseSkyboxExposure);
    }

    void UpdateDarknessAnimation()
    {
        directionalLight.intensity = Mathf.Lerp(baseDirectionalLightIntensity, directionalLightIntensityInDarkness, darknessAnimation_t);
        globalVolumeVignette.intensity.value = Mathf.Lerp(baseVignetteIntensity, vignetteIntensityInDarkness, darknessAnimation_t);
        RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(baseSkyboxExposure, skyboxExposureInDarkness, darknessAnimation_t));
        playerLight.intensity = Mathf.Lerp(basePlayerLightIntensity, playerLightIntensityInDarkness, darknessAnimation_t);
    }
}
