using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;      // drag EnemyPrefab here
    public float gridSize = 1.25f;         // size of one tile
    public TimerManager timerManager;   // reference to your timer manager

    public Vector2 gridOrigin;

    [Header("Grid Coordinates (grid units)")]
    [SerializeField] public Vector2Int[] pathCells;


    [SerializeField] private int gridWidth = 8;
    [SerializeField] private int gridHeight = 8;
    private HashSet<Vector2Int> pathSet;

    void Start()
    {
        pathSet = new HashSet<Vector2Int>(pathCells);
        SpawnEnemies();
    }



    void SpawnEnemies()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);

                if (pathSet.Contains(cell))
                    continue;

                SpawnEnemy(cell);
            }
        }
    }

    void SpawnEnemy(Vector2Int cell)
    {
        Vector3 worldPos = new Vector3(
            gridOrigin.x + cell.x * gridSize,
            gridOrigin.y + cell.y * gridSize,
            0f
        );

        GameObject enemyObj = Instantiate(enemyPrefab, worldPos, Quaternion.identity, transform);

        Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        enemyScript.timerManager = timerManager;
    }
}