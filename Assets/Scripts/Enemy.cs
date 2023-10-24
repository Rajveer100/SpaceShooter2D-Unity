using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    [SerializeField] private float enemySpeed = 4f;
    
    [SerializeField] private GameObject laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    
    private Player _player;

    private Animator _animator;
    private AudioSource _audioSource;
    
    private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");
    
    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        
        if (!_player)
            Debug.Log("The Player is Null!");
        
        if (!_animator)
            Debug.Log("The Enemy Animator is Null!");
        if (!_audioSource)
            Debug.Log("AudioSource on the Enemy is Null!");
    }

    private void Update() {
        CalculateMovement();

        if (Time.time > _canFire) {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i += 1) {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement() {
        transform.Translate(Vector3.down * (enemySpeed * Time.deltaTime));

        if (transform.position.y < -5f) {
            var randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();
            if (player)
                player.Damage();
            
            _animator.SetTrigger(OnEnemyDeath);
            enemySpeed = 0;
            
            _audioSource.Play();
            Destroy(gameObject, 2.8f);
        } 
        
        if (other.CompareTag("Laser")) {
            Destroy(other.transform.gameObject);
            if (_player)
                _player.AddScore(10);
            
            _animator.SetTrigger(OnEnemyDeath);
            enemySpeed = 0;
            
            _audioSource.Play();
            
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }
}
