using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour {
    
    [SerializeField] private float playerSpeed = 3.5f;
    private float _speedMultiplier = 2;
    
    [SerializeField] private int lives = 3;
    
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab;
    
    [SerializeField] private float fireRate = 0.5f;
    private float _canFire = -1f;

    private bool _tripleShotIsActive;
    private bool _speedBoostIsActive;
    
    private bool _shieldIsActive;
    [SerializeField] private GameObject shieldVisualiser;

    [SerializeField] private GameObject leftEngine, rightEngine;
    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    
    private int _score;

    [SerializeField] private AudioClip laserSoundClip;
    private AudioSource _audioSource;
    
    private void Start() {
        transform.position = Vector3.zero;
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();
        
        if (!_spawnManager) 
            Debug.Log("The SpawnManager is Null!");
        if (!_uiManager)
            Debug.Log("The UIManager is Null!");

        if (!_audioSource)
            Debug.Log("AudioSource on the player is Null!");
        else
            _audioSource.clip = laserSoundClip;
    }
    
    private void Update() {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            FireLaser();
    }

    private void FireLaser() {
        _canFire = Time.time + fireRate;

        var offsetPosition = new Vector3(0, 1.05f, 0);
        if (_tripleShotIsActive) {
            Instantiate(tripleShotPrefab, transform.position + offsetPosition, Quaternion.identity);
        } else {
            Instantiate(laserPrefab, transform.position + offsetPosition, Quaternion.identity);
        }
        
        _audioSource.Play();
    }

    public void Damage() {
        if (_shieldIsActive) {
            _shieldIsActive = false;
            shieldVisualiser.SetActive(false);
            return;
        }
        
        lives -= 1;

        if (lives == 2) {
            leftEngine.SetActive(true);
        } else if (lives == 1) {
            rightEngine.SetActive(true);
        }
        
        _uiManager.UpdateLives(lives);
        if (lives == 0) {
            _spawnManager.OnPlayerDeath();
            
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject);
        }
    }
    
    private void CalculateMovement() {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        var direction = new Vector3(horizontalInput, verticalInput, 0.0f);
        transform.Translate(direction * (playerSpeed * Time.deltaTime));
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0);
        
        if (transform.position.x > 11.3f) 
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x < -11.3f) 
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }

    public void TripleShotActive() {
        _tripleShotIsActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive() {
        _speedBoostIsActive = true;
        playerSpeed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldsActive() {
        _shieldIsActive = true;
        shieldVisualiser.SetActive(true);
    }

    IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5);
        _tripleShotIsActive = false;
    }
    
    IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(5);
        _speedBoostIsActive = false;
        playerSpeed /= _speedMultiplier;
    }

    public void AddScore(int points) {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
