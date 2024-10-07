using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    List<Ant> ants;

    public float timeBetweenSpawns = 0.66f;
    public float timeBetweenSpawnsCurrent = 0;
    int antsSpawned = 0;


    private void Start()
    {
        spawnObject = FindObjectsByType<GameObject>(FindObjectsSortMode.None).First(x => x.name.Equals("Spawn"));
        ants = new List<Ant>();

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
                var obj = Instantiate(antPrefab, spawnObject.transform.position, spawnObject.transform.rotation,null);
                obj.GetComponent<Ant>().directionFacing = directionToSpawnAnts;
                ants.Add(obj.GetComponent<Ant>());
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
        int currentLevel = (int)SceneManager.GetActiveScene().name.Last();
        SceneManager.LoadScene($"Level{currentLevel + 1}");
    }

    internal bool IsThereAnAnt(int x, int y)
    {
            foreach (Ant ant in ants) {
            if (ant.gameObject.activeSelf)
            {
                if(ant.xPosition==x && ant.yPosition == y)
                {
                    return true;
                }
            }
        }

        return false;

    }
}
