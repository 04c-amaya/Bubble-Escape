using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangery : MonoBehaviour
{
    public RectTransform background;
    public changeScenery groundLevel;
    public changeScenery skyLevel;
    public changeScenery spaceLevel;
    public GameManager gameManager;

    [Header("Options")]
    public float changeBackgroundSpeed = 100.0f;
    public float dynamicSceneYlevel;
    public float distForSceneChange = 10.0f;
    public float increaseGravityPerStage = 1.0f;
    private bool isUsingdynamicSceneChange;
    private bool valueChanged;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        StageChange();
    }

    public void StageChange()
    {
        if (!isUsingdynamicSceneChange)
        {
            if (gameManager.isInFloorStage)
            {
                background.anchoredPosition = Vector3.Lerp(background.anchoredPosition, new Vector3(background.anchoredPosition.x, groundLevel.backgroundScenePositionY), Time.deltaTime * changeBackgroundSpeed / 100f);
                if (!valueChanged)
                {
                    gameManager.enemySpawnRate -= groundLevel.increaseEnemySpawns;
                    gameManager.player.GetComponent<PlayerManager>().gravityScale -= groundLevel.increaseGravity;
                    valueChanged = true;
                }
                if (gameManager.player.transform.position.y > skyLevel.yLevelToNewScene && gameManager.player.transform.position.y < spaceLevel.yLevelToNewScene)
                {
                    CurrentStage(false, true, false);
                }
            }
            else if (gameManager.isInSkyStage)
            {
                background.anchoredPosition = Vector3.Lerp(background.anchoredPosition, new Vector3(background.anchoredPosition.x, skyLevel.backgroundScenePositionY), Time.deltaTime * changeBackgroundSpeed / 100f);
                if (!valueChanged)
                {
                    gameManager.enemySpawnRate -= skyLevel.increaseEnemySpawns;
                    gameManager.player.GetComponent<PlayerManager>().gravityScale -= skyLevel.increaseGravity;
                    valueChanged = true;
                }
                if (gameManager.player.transform.position.y > spaceLevel.yLevelToNewScene)
                {
                    CurrentStage(false, false, true);
                }
            }
            else if (gameManager.isInSpaceStage)
            {
                Debug.Log("Is in Space");
                background.anchoredPosition = Vector3.Lerp(background.anchoredPosition, new Vector3(background.anchoredPosition.x, spaceLevel.backgroundScenePositionY), Time.deltaTime * changeBackgroundSpeed / 100f);
                if (!valueChanged)
                {
                    gameManager.enemySpawnRate -= spaceLevel.increaseEnemySpawns;
                    gameManager.player.GetComponent<PlayerManager>().gravityScale -= spaceLevel.increaseGravity;
                    valueChanged = true;
                }
                if (gameManager.player.transform.position.y > spaceLevel.yLevelToNewScene + ((spaceLevel.yLevelToNewScene + spaceLevel.yLevelToNewScene) / 2))
                {
                    isUsingdynamicSceneChange = true;
                }
            }
        }
        else
        {
            if (gameManager.player.transform.position.y - dynamicSceneYlevel >= distForSceneChange)
            {
                bool changedScene = false;
                valueChanged = false;
                if (gameManager.isInSpaceStage && !changedScene)
                {
                    background.anchoredPosition = Vector3.Lerp(background.anchoredPosition, new Vector3(background.anchoredPosition.x, groundLevel.backgroundScenePositionY), Time.deltaTime * changeBackgroundSpeed / 100f);
                    gameManager.isInSkyStage = true;
                    gameManager.isInSpaceStage = false;
                    changedScene = true;
                }
                else if (gameManager.isInSkyStage && !changedScene)
                {
                    background.anchoredPosition = Vector3.Lerp(background.anchoredPosition, new Vector3(background.anchoredPosition.x, spaceLevel.backgroundScenePositionY), Time.deltaTime * changeBackgroundSpeed / 100f);
                    gameManager.isInSkyStage = false;
                    gameManager.isInSpaceStage = true;
                    changedScene = true;
                }

                dynamicSceneYlevel = gameManager.player.transform.position.y * 2;

                if (!valueChanged)
                {
                    gameManager.enemySpawnRate -= .05f;
                    gameManager.player.GetComponent<PlayerManager>().gravityScale -= increaseGravityPerStage;
                    valueChanged = true;
                }
            }
        }
    }

    public void CurrentStage(bool ground, bool sky, bool space)
    {
        valueChanged = false;
        gameManager.isInFloorStage = ground;
        gameManager.isInSkyStage = sky;
        gameManager.isInSpaceStage = space;
    }
}

[System.Serializable]
public class changeScenery
{
    public float backgroundScenePositionY;
    public float yLevelToNewScene;
    public float increaseEnemySpawns;
    public float increaseGravity;
}