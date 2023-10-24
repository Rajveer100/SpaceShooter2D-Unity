using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Laser : MonoBehaviour {

    [SerializeField] private float laserSpeed = 8f;

    private bool _isEnemyLaser;
    
    private void Update() {
        if (!_isEnemyLaser) {
            MoveUp();
        } else {
            MoveDown();
        }
    }

    void MoveUp() {
        transform.Translate(Vector3.up * (laserSpeed * Time.deltaTime));

        if (transform.position.y >= 8f) {
            if (transform.parent)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
    
    void MoveDown() {
        transform.Translate(Vector3.down * (laserSpeed * Time.deltaTime));

        if (transform.position.y < -8f) {
            if (transform.parent)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser() {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && _isEnemyLaser) {
            Player player = other.GetComponent<Player>();
            if (player)
                player.Damage();
        }
    }
}
