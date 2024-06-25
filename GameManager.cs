using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> floorEnemies;
    public List<GameObject> skyEmemies;
    public List<GameObject> spaceEnemies;

    public List<ItemDropChance> items;
    public float enemySpawnRate;
    public float itemSpawnRate;
    public GameObject player;

    [Header("Flag")]
    public bool isInFloorStage;
    public bool isInSkyStage;
    public bool isInSpaceStage;

    bool summonMonster;
    bool summonItem;
    ScreenPositions screenPositions;

    private void Awake()
    {
        screenPositions = GameObject.Find("Main Camera").GetComponent<ScreenPositions>();
    }
    private void Update()
    {
        if (player.GetComponent<PlayerManager>().playerScoreManager.finalScore > 1)
        {

            if (!summonMonster)
            {
                StartCoroutine(SpawnEnemy());

            }
            if (!summonItem)
            {
                StartCoroutine(SpawnItems());
            }
        }

    }
    public IEnumerator SpawnEnemy()
    {
        summonMonster = true;
        yield return new WaitForSeconds(enemySpawnRate);
        Vector3 spawnPosition = new Vector3();

        spawnPosition.y = screenPositions.topSide;
        int spawnDirection = Random.Range(0, 2);
        if (spawnDirection == 0)
        {
            //Spawn Left Side
            spawnPosition.x = player.transform.position.x - Random.Range(3,10);
        }
        else
        {
            //Spawn Right Side
            spawnPosition.x = player.transform.position.x + Random.Range(3, 10);
        }
        GameObject enemy = null;
        if (isInFloorStage)
        {
            var RandomEnemy = Random.Range(0, floorEnemies.Count);
            enemy = Instantiate(floorEnemies[RandomEnemy], spawnPosition, Quaternion.identity);
        }
        else if (isInSkyStage)
        {
            var RandomEnemy = Random.Range(0, skyEmemies.Count);
            enemy = Instantiate(skyEmemies[RandomEnemy], spawnPosition, Quaternion.identity);
        }
        else if (isInSpaceStage)
        {
            var RandomEnemy = Random.Range(0, spaceEnemies.Count);
            enemy = Instantiate(spaceEnemies[RandomEnemy], spawnPosition, Quaternion.identity);
        }
        //Debug.Log(enemy);
        if (enemy != null)
        {
            Hazards enemyManager = null;
            if (enemy.transform.parent != null)
            {
                enemyManager = enemy.GetComponentInChildren<Hazards>();
            }
            else
            {
                enemyManager = enemy.GetComponent<Hazards>();
            }

            if(enemyManager != null)
            {
                if (enemyManager.usingDropPoint)
                {
                    enemy.transform.position = new Vector3(enemy.transform.position.x + Random.Range(-5, 5), enemy.transform.position.y, enemy.transform.position.z);
                }
                else if (enemyManager.goingBetweenTwoPoints)
                {
                    if (enemyManager.horizontalMovement)
                    {
                        if (spawnDirection == 0)
                        {
                            //Spawn Left Side
                            enemyManager.pointA.transform.position = enemy.transform.position;
                            enemyManager.pointB.transform.position = new Vector3(enemyManager.pointA.transform.position.x + (Random.Range(10, 30)), enemyManager.pointA.transform.position.y, enemyManager.pointA.transform.position.z);

                        }
                        else
                        {
                            enemyManager.pointB.transform.position = enemy.transform.position;
                            enemyManager.pointA.transform.position = new Vector3(enemyManager.pointB.transform.position.x - (Random.Range(10, 30)), enemyManager.pointB.transform.position.y, enemyManager.pointB.transform.position.z);

                        }
                    } else if (enemyManager.horizontalMovement)
                    {
                        if (spawnDirection == 0)
                        {
                            //Spawn Left Side
                            enemyManager.pointA.transform.position = enemy.transform.position;
                            enemyManager.pointB.transform.position = new Vector3(enemyManager.pointA.transform.position.x, enemyManager.pointA.transform.position.x + (Random.Range(10, 30)), enemyManager.pointA.transform.position.z);

                        }
                        else
                        {
                            enemyManager.pointB.transform.position = enemy.transform.position;
                            enemyManager.pointB.transform.position = new Vector3(enemyManager.pointA.transform.position.x, enemyManager.pointA.transform.position.x - (Random.Range(10, 30)), enemyManager.pointA.transform.position.z);

                        }
                    }

                }
                else if (enemyManager.usingGlide)
                {
                    var chooseDirection = Random.Range(0, 2);
                    if (spawnDirection == 0)
                    {
                        //Spawn Left Side
                        enemyManager.goLeft = false;
                        enemyManager.GoRight = true;
                        if (enemy.transform.localScale.x < 0)
                        {
                            enemy.transform.localScale = new Vector3(enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
                        }
                        else
                        {
                            enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
                        }
                        
                    }
                    else
                    {
                        //Spawn Right Side
                        enemyManager.goLeft = true;
                        enemyManager.GoRight = false;
                        if (enemy.transform.localScale.x < 0)
                        {
                            enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
                        }
                        else
                        {
                            enemy.transform.localScale = new Vector3(enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
                        }
                        enemy.transform.localScale = new Vector3(1, enemy.transform.localScale.y, enemy.transform.localScale.z);
                    }

                    if (chooseDirection == 0)
                    {
                        //go Down
                        enemyManager.glideUp = false;
                        enemyManager.glideDown = true;
                    }
                    else
                    {
                        //Go Up
                        enemyManager.glideUp = false;
                        enemyManager.glideDown = true;
                    }
                }
                else if (enemyManager.goingDirection)
                {
                    if (spawnDirection == 0)
                    {
                        //Spawn Left Side
                        enemyManager.goLeft = false;
                        enemyManager.GoRight = true;
                        enemy.transform.localScale = new Vector3(-1, enemy.transform.localScale.y, enemy.transform.localScale.z);
                    }
                    else
                    {
                        //Spawn Right Side
                        enemyManager.goLeft = true;
                        enemyManager.GoRight = false;
                        enemy.transform.localScale = new Vector3(1, enemy.transform.localScale.y, enemy.transform.localScale.z);
                    }
                }
            }
        }

        summonMonster = false;
    }
    public IEnumerator SpawnItems()
    {
        summonItem = true;
        yield return new WaitForSeconds(itemSpawnRate);
        Vector3 spawnPosition = new Vector3();

        spawnPosition.y = screenPositions.topSide;
        spawnPosition.x = Random.Range(screenPositions.leftSide, screenPositions.rightSide);
        var RandomItem = Random.Range(0, items.Count);


        foreach (ItemDropChance item in items)
        {
            if (ShouldDropItem(item))
            {
                var spawnedItem = Instantiate(items[RandomItem].item, spawnPosition, Quaternion.identity);
            }
        }
        summonItem = false;
    }
    bool ShouldDropItem(ItemDropChance item)
    {
        var ChanceForItemToSpawn = Random.Range(0, 100);
        return ChanceForItemToSpawn <= item.dropChanceOutOf100;
    }

    [System.Serializable]
    public class ItemDropChance
    {
        public GameObject item;
        public int dropChanceOutOf100;
        public ItemDropChance(GameObject _item, int _dropChance)
        {
            item = _item;
            dropChanceOutOf100 = _dropChance;
        }
    }
}
