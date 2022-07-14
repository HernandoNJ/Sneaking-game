using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _failedPanel;
    [SerializeField] private GameObject _successPanel;
    [SerializeField] private AudioSource _bmgSource;
    [SerializeField] private AudioClip _caughtMusic;
    [SerializeField] private AudioClip _successMusic;
    
    [SerializeField] private float _canvasFadeTime = 2f;
    
    private PlayerInput _playerInput;
    private bool _isFadingIn;
    private FirstPersonController _fpController;
    private bool _isGoalReached;

    private float _fadeLevel;

    private void Start()
    {
        FindEnemies();
        FindThePlayer();
        CanvasInitialValues();
    }

    private void Update()
    {
        CanvasFading();
    }

    private void FindEnemies()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (var enemy in enemies)
        {
            enemy.onInvestigate.AddListener(EnemyInvestigating);
            enemy.onPlayerFound.AddListener(PlayerFound);
            enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
        }
    }

    private void FindThePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player)
        {
            _playerInput = player.GetComponent<PlayerInput>();
            _fpController = player.GetComponent<FirstPersonController>();
        }
        else { Debug.Log("There is no player in the scene"); }
    }

    private void CanvasInitialValues()
    {
        _canvasGroup.alpha = 0;
        _failedPanel.SetActive(false);
        _successPanel.SetActive(false);
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

    private void PlayerFound(Transform enemyThatFoundPlayer)
    {
        if (_isGoalReached) return;
        _isFadingIn = true;
        _failedPanel.SetActive(true);
        _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);
        DeactivateInput();
        PlayBGM(_caughtMusic);
    }

    public void GoalReached()
    {
        _isGoalReached = true;
        _isFadingIn = true;
        _successPanel.SetActive(true);
        DeactivateInput();
        PlayBGM(_successMusic);
    }

    private void EnemyInvestigating()
    {
    }

    public void RestartScene()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayBGM(AudioClip newBgm)
    {
        if (_bmgSource.clip == newBgm) return; // avoid playing the clip twice
         
        _bmgSource.clip = newBgm;
        _bmgSource.Play();
    }

    private void DeactivateInput()
    {
        _playerInput.DeactivateInput();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void EnemyReturnToPatrol()
    {
    }

}
