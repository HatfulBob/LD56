using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int numberOfAnts = 40;
    public Ant.Direction directionToSpawnAnts;
    public GameObject antPrefab;
    public int savedAnts = 0;
    public GameObject victoryScreen;
    GameObject spawnObject;

    public float timeBetweenSpawns = 0.66f;
    public float timeBetweenSpawnsCurrent = 0;
    int antsSpawned = 0;


    private void Start()
    {
        spawnObject = FindObjectsByType<GameObject>(FindObjectsSortMode.None).First(x => x.name.Equals("Spawn"));


    }
    private void Update()
    {
        if(savedAnts == numberOfAnts-2)
        {
            victoryScreen.SetActive(true);
        }

        if(timeBetweenSpawnsCurrent> timeBetweenSpawns)
        {
            if(antsSpawned< numberOfAnts)
            {
                Instantiate(antPrefab, spawnObject.transform.position, spawnObject.transform.rotation,null);
                antPrefab.GetComponent<Ant>().directionFacing = directionToSpawnAnts;
                antsSpawned++;
                timeBetweenSpawnsCurrent = 0;
            }
        } else
        {
            timeBetweenSpawnsCurrent += Time.deltaTime;
        }
    }

    public void NextLevelButton()
    {
        int currentLevel = SceneManager.GetActiveScene().name.Last();
        SceneManager.LoadScene($"Level{currentLevel + 1}");
    }
}
