using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    public enum GameMode { FP, VR }

    [Header("Accessibility")]
    public Handed handedness;
    public GameMode gameMode;

    [Header("UI")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private GameObject _failedPanel;
    [SerializeField] private GameObject _successPanel;
    [SerializeField] private float _canvasFadeTime = 2f;
    [SerializeField] private Material _skyboxMaterial;

    [Header("Audio")]
    [SerializeField] private AudioSource _bmgSource;
    [SerializeField] private AudioClip _caughtMusic;
    [SerializeField] private AudioClip _successMusic;

    private PlayerInput _playerInput;
    private FirstPersonController _fpController;
    private bool _isFadingIn;
    private float _fadeLevel;
    private bool _isGoalReached;

    private float _initialSkyboxAtmosphereThickness;
    private Color _initialSkyboxColor;
    private float _initialSkyboxExposure;

    private void Start()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            enemy.onInvestigate.AddListener(EnemyInvestigating);
            enemy.onPlayerFound.AddListener(PlayerFound);
            enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
        }

        GameObject player = GameObject.FindWithTag("Player");

        if (player)
        {
            _playerInput = player.GetComponent<PlayerInput>();
            _fpController = player.GetComponent<FirstPersonController>();
        }
        else { Debug.Log("There is no player in the scene"); }

        _canvasGroup.alpha = 0;
        _failedPanel.SetActive(false);
        _successPanel.SetActive(false);

        ResetShaderValues();
        _initialSkyboxAtmosphereThickness = _skyboxMaterial.GetFloat("_AtmosphereThickness");
        _initialSkyboxColor = _skyboxMaterial.GetColor("_SkyTint");
        _initialSkyboxExposure = _skyboxMaterial.GetFloat("_Exposure");
    }

    private void Update()
    {
        CanvasFading();
    }

    private void EnemyReturnToPatrol()
    {
    }

    private void PlayerFound(Transform enemyThatFoundPlayer)
    {
        Debug.Log("Player found");

        if (_isGoalReached) return;

        if (gameMode == GameMode.FP)
        {
            _isFadingIn = true;
            _failedPanel.SetActive(true);
            _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);

            DeactivateInput();
        }
        else { StartCoroutine((GameOverFadeOutSaturation(0.4f))); }

        PlayBGM(_caughtMusic);
    }

    private IEnumerator GameOverFadeOutSaturation(float _startDelay = 0f)
    {
        yield return new WaitForSeconds(_startDelay);

        Time.timeScale = 0; // Pause the game

        float fade = 0f;

        while (fade < 1f)
        {
            // Increase fade even if timeScale = 0 (game paused)
            fade += Time.unscaledTime / _canvasFadeTime;
            Shader.SetGlobalFloat("_AllWhite", fade);
            _skyboxMaterial.SetFloat("_AtmosphereThickness", Mathf.Lerp(_initialSkyboxAtmosphereThickness, 0.7f, fade));
            _skyboxMaterial.SetColor("_SkyTint", Color.Lerp(_initialSkyboxColor, Color.white, fade));
            _skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(_initialSkyboxExposure, 8, fade));

            yield return null;
        }

        // As the timeScale is currently = 0 (game paused) (PlayerFound), we need to use WaitForSecondsRealtime
        yield return new WaitForSecondsRealtime(2f);
        RestartScene();
    }

    private void OnDestroy()
    {
        ResetShaderValues();
    }

    private void ResetShaderValues()
    {
        Shader.SetGlobalFloat("_AllWhite", 0);

        // Set skybox initial values
        _skyboxMaterial.SetFloat("_AtmosphereThickness", _initialSkyboxAtmosphereThickness);
        _skyboxMaterial.SetColor("_SkyTint", _initialSkyboxColor);
        _skyboxMaterial.SetFloat("_Exposure", _initialSkyboxExposure);
    }

    public void GoalReached()
    {
        _isFadingIn = true;
        _isGoalReached = true;
        _successPanel.SetActive(true);
        DeactivateInput();
        PlayBGM(_successMusic);
    }

    private void DeactivateInput()
    {
        _playerInput.DeactivateInput();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CanvasFading()
    {
        if (_isFadingIn)
        {
            if (_fadeLevel < 1f)
            {
                // _canvasFadeTime: amount of seconds to wait until  _fadeLevel reaches 1
                _fadeLevel += Time.deltaTime / _canvasFadeTime;
            }
        }
        else
        {
            if (_fadeLevel > 0f) { _fadeLevel -= Time.deltaTime / _canvasFadeTime; }
        }

        _canvasGroup.alpha = _fadeLevel;
    }

    private void EnemyInvestigating()
    {
    }

    public void RestartScene()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ResetShaderValues();
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayBGM(AudioClip newBgm)
    {
        if (_bmgSource.clip == newBgm) return; // avoid playing the clip twice

        _bmgSource.clip = newBgm;
        _bmgSource.Play();
    }
}
