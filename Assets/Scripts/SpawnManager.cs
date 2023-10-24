using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    
    [SerializeField] private GameObject[] powerUps;

    private bool _stopSpawningEnemy;
    private bool _stopSpawningPowerUp;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    
    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3f);
        
        while (!_stopSpawningEnemy) {
            var randomX = Random.Range(-8f, 8f);
            var newEnemy = Instantiate(enemyPrefab, new Vector3(randomX, 7f, 0), Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerUpRoutine() {
        yield return new WaitForSeconds(3f);
        
        while (!_stopSpawningPowerUp) {
            var randomX = Random.Range(-8f, 8f);
            var randomId = Random.Range(0, 3);
            var newPowerUp = Instantiate(powerUps[randomId], new Vector3(randomX, 7f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath() {
        _stopSpawningEnemy = true;
        _stopSpawningPowerUp = true;
    }
}
