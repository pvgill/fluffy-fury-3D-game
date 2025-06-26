using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int[] intermediateRoomDifficulties = new int[1]; // difficulties of rooms in order from first non-start level to second to last level
    public int roomsPerRoom = 2;
    Queue<string> levelOrder = new Queue<string>();

    private Dictionary<int, List<string>> levelsDictionary = new Dictionary<int, List<string>>();
    private Dictionary<string, string> levelDisplay = new Dictionary<string, string>();


    // Awake is called before start
    void Awake() // runs on generation of initial level
    {
        FillLevelDictionary();

        // Add levels between starting level and final boss
        List<string> levels = SelectLevels();
        for (int i = 0; i < intermediateRoomDifficulties.Length*roomsPerRoom; i++)
        {
            levelOrder.Enqueue(levels[i]);
        }

        // Add final boss Level
        levelOrder.Enqueue("FinalLevel");
    }


    List<string> SelectLevels()
    {
        List<string> newLevelOrder = new List<string>();
        for (int i = 0; i < intermediateRoomDifficulties.Length; i++)
        {
            for (int j = 0; j < roomsPerRoom; j++)
            {
                string level = ChooseLevel(intermediateRoomDifficulties[i]);
                newLevelOrder.Add(level);
            }
        }
        return newLevelOrder;
    }

    string ChooseLevel(int difficulty)
    {
        List<string> tempList = levelsDictionary[difficulty];
        
        return tempList[Random.Range(0, tempList.Count)];
    }

    public Queue<string> GetLevelOrder()
    {
        return levelOrder;
    }

    private void FillLevelDictionary()
    {

        levelsDictionary.Add(1,
            new List<string> {
                "Level1",
                "demo"
        }
        );

        levelDisplay.Add("Level1", "human");
        levelDisplay.Add("demo", "dog");

        levelsDictionary.Add(2,
            new List<string> {
                "2"
        }
        );

        levelDisplay.Add("2", "human");

        levelsDictionary.Add(3,
            new List<string> {
                "3"
            }
            );

        levelDisplay.Add("3", "human");
    }
}
