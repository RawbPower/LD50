using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralLevel : MonoBehaviour
{
    public int chunkWidth, chunkHeight;
    public int platformsPerChunk;
    public float smoothness;
    public float flatness;
    public float groundBaseHeight;
    public float seed;
    public bool randomSeed = true;
    public TileBase groundTile;
    public Tilemap groundTilemap;
    public bool sunset = false;
    public bool dusk = false;

    public Transform player;

    public GameObject[] spawnEntities;
    public int spawnsPerChunk;
    public GameObject[] spawnPickups;
    public int pickupsPerChunk;

    public GameObject sunsetChunk;
    public GameObject sunsetSpawn;
    public GameObject duskSpawn;

    private int[,] currentMap;
    private int lastPlayerXPos = 0;
    private int duskChunks = 0;

    public static ProceduralLevel instance;
    private PlatformPresets platformPresets;

    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            Player playerEntity = FindObjectOfType<Player>();
            player = playerEntity.transform;
        }

        if (groundTilemap == null)
        {
            groundTilemap = FindObjectOfType<Tilemap>();
        }

        platformPresets = new PlatformPresets();
        int playerXPosition = Mathf.RoundToInt(player.position.x) / chunkWidth;
        GenerateChunk(ref currentMap, playerXPosition);
        GenerateChunk(ref currentMap, (playerXPosition + 1) * chunkWidth);
        lastPlayerXPos = Mathf.Max(playerXPosition, lastPlayerXPos);
    }

    // Update is called once per frame
    void Update()
    {
        int playerXPosition = Mathf.RoundToInt(player.position.x) / chunkWidth;
        if (lastPlayerXPos < playerXPosition)
        {
            if (sunset)
            {
                GenerateSunsetChunk(ref currentMap, (playerXPosition + 1) * chunkWidth);
                sunset = false;
            }
            else if (dusk)
            {
                GenerateChariotChunk(ref currentMap, (playerXPosition + 1) * chunkWidth);
                duskChunks++;
                if (duskChunks == 2)
                {
                    if (!duskSpawn.gameObject.activeSelf)
                    {
                        duskSpawn.transform.position = new Vector3(player.position.x - 20, 11, 0);
                        duskSpawn.gameObject.SetActive(true);
                        FindObjectOfType<GlobalAudioManager>().Play("Chariot");
                        FindObjectOfType<GlobalAudioManager>().Stop("NightTheme");
                        FindObjectOfType<GlobalAudioManager>().Play("ChariotTheme");
                    }
                }
            }
            else
            {
                GenerateChunk(ref currentMap, (playerXPosition + 1) * chunkWidth);
            }

            if (lastPlayerXPos > 0)
            {
                ClearChunk(ref currentMap, (lastPlayerXPos - 1) * chunkWidth);
            }
            lastPlayerXPos = Mathf.Max(playerXPosition, lastPlayerXPos);
        }
    }

    private void GenerateChunk(ref int[,] map, int offset)
    {
        if (randomSeed)
        {
            seed = Random.Range(0.0f, 100000000.0f);
        }
        map = InitializeArray(chunkWidth, chunkHeight, true);
        int maxHeight = GenerateLevel(ref map);
        GeneratePlatforms(ref map, maxHeight + 2, platformsPerChunk);
        BuildMap(map, groundTilemap, groundTile, offset);
        SpawnEntities(offset);
        SpawnPickups(offset);
    }

    private void GenerateSunsetChunk(ref int[,] map, int offset)
    {
        sunsetChunk.transform.position = new Vector3((float)offset + chunkWidth * 0.5f, -1, 0);
        sunsetSpawn.transform.position =  new Vector3((float)offset + chunkWidth * 0.5f, chunkHeight, 0);
        sunsetSpawn.gameObject.SetActive(true);

        SpawnEntities(offset);
        SpawnPickups(offset);
    }

    private void GenerateChariotChunk(ref int[,] map, int offset)
    {
        map = InitializeArray(chunkWidth, chunkHeight, true);
        int maxHeight = GenerateChariotLevel(ref map);
        GeneratePlatforms(ref map, maxHeight + 4, platformsPerChunk);
        BuildMap(map, groundTilemap, groundTile, offset);
        SpawnEntities(offset);
        SpawnPickups(offset);
    }

    private void ClearChunk(ref int[,] map, int offset)
    {
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                groundTilemap.SetTile(new Vector3Int(offset + x, y, 0), null);
            }
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeSelf && enemy.transform.position.x < offset + chunkWidth)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    private void RegenerateLevel()
    {
        groundTilemap.ClearAllTiles();
        int playerXPosition = Mathf.RoundToInt(player.position.x) / chunkWidth;
        GenerateChunk(ref currentMap, playerXPosition);
        GenerateChunk(ref currentMap, (playerXPosition + 1) * chunkWidth);
        lastPlayerXPos = Mathf.Max(playerXPosition, lastPlayerXPos);
    }

    public int[,] InitializeArray(int width, int height, bool empty)
    {
        int[,] generatedMap = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                generatedMap[x,y] = (empty) ? 0 : 1;
            }
        }

        return generatedMap;
    }

    public int GenerateLevel(ref int [,] map)
    {
        int perlinHeight;
        int maxHeight = 0;
        for (int x = 0; x < chunkWidth; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * chunkHeight / flatness + groundBaseHeight);
            maxHeight = Mathf.Max(maxHeight, perlinHeight);
            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = 1;
            }
        }
        return maxHeight;
    }

    public int GenerateChariotLevel(ref int[,] map)
    {
        int perlinHeight;
        int maxHeight = 0;
        for (int x = 0; x < chunkWidth; x++)
        {
            perlinHeight = Mathf.RoundToInt(chunkHeight / 2.0f) + 3;
            maxHeight = Mathf.Max(maxHeight, perlinHeight);
            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = 1;
            }
        }
        return maxHeight;
    }

    public void GeneratePlatforms(ref int[,] map, int baseHeight, int platformsPerChunk)
    {
        int stepSize = chunkWidth / platformsPerChunk;

        for (int i = 0; i < platformsPerChunk; i++)
        {
            int[,] platform = platformPresets.presets[Random.Range(0, platformPresets.presets.Count - 1)];
            int platformBaseHeight = baseHeight + (int)Random.Range(-3, 3);
            if ((i * stepSize) + (stepSize / 2) + platform.GetLength(1) < chunkWidth && baseHeight + platform.GetLength(0) - 1 < chunkHeight)
            {
                for (int x = 0; x < platform.GetLength(1); x++)
                {
                    for (int y = 0; y < platform.GetLength(0); y++)
                    {
                        map[(i*stepSize) + (stepSize / 2) + x, baseHeight + (platform.GetLength(0) - 1 - y)] = platform[y, x];
                    }
                }
            }
        }
    }

    public void BuildMap(int[,] map, Tilemap groundTileMap, TileBase groundTileBase, int offset)
    {
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(offset + x, y, 0), groundTileBase);
                }
            }
        }
    }

    public void SpawnEntities(int offset)
    {
        int stepSize = chunkWidth / spawnsPerChunk;

        for (int i = 0; i < spawnsPerChunk; i++)
        {
            int randomIndex = Mathf.FloorToInt(Random.Range(0, spawnEntities.Length - 0.00001f));
            Vector3 spawnPosition = new Vector3(offset + (i * stepSize) + (stepSize / 2) + 0.5f, chunkHeight, 0);
            Instantiate(spawnEntities[randomIndex], spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnPickups(int offset)
    {
        int stepSize = chunkWidth / pickupsPerChunk;

        for (int i = 0; i < spawnsPerChunk; i++)
        {
            int randomIndex = Mathf.FloorToInt(Random.Range(0, spawnPickups.Length - 0.00001f));
            Vector3 spawnPosition = new Vector3(offset + (i * stepSize) + (stepSize / 2) + 0.5f, chunkHeight, 0);
            Instantiate(spawnPickups[randomIndex], spawnPosition, Quaternion.identity);
        }
    }
}
