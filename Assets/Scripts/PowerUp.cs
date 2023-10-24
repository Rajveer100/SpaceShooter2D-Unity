using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    
    [SerializeField] private float powerUpSpeed = 3f;
    [SerializeField] private int powerUpId;

    [SerializeField] private AudioClip clip;
    
    void Update() {
        transform.Translate(Vector3.down * (powerUpSpeed * Time.deltaTime));
        if (transform.position.y < -4.5f) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();
            
            AudioSource.PlayClipAtPoint(clip, transform.position);
            
            if (player) {
                switch (powerUpId) {
                    case 0: 
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
