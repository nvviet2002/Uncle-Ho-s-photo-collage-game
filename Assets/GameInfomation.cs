using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameInfomationScriptableObject", order = 1)]
public class GameInfomation : ScriptableObject
{
    public float snapDistance;
    public float moveSpeed;
    public float detectiveRange;
    public float starTime;

    public Sprite star;
    public Sprite yellowStar;

}
