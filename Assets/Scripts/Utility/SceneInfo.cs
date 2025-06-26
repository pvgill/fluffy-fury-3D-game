using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script inspired by "Change Unity Scenes, Keep Data" by Restful Coder: https://www.youtube.com/watch?v=UDY0edZZSdo
[CreateAssetMenu(fileName = "SceneInfo", menuName = "PlayerInfo")]
public class SceneInfo : ScriptableObject
{
    public int playerHealth = 3;
    public int playerAmmo = 10;
}
