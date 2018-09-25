using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems
{
    public static Dictionary<char, string> MAP_KEYS;

    public static char WITHOUT_CHANGES = '#';

    public const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public const string KEY_NONE = "none";
    public const string KEY_BATTLE_WALL = "battle_wall";
    public const string KEY_BANG = "bang";
    public const string KEY_CONSTRUCTION = "construction";
    public const string KEY_CONSTRUCTION_DESTROYED_DOWN = "construction_destroyed_down";
    public const string KEY_CONSTRUCTION_DESTROYED_UP = "construction_destroyed_up";
    public const string KEY_CONSTRUCTION_DESTROYED_LEFT = "construction_destroyed_left";
    public const string KEY_CONSTRUCTION_DESTROYED_RIGHT = "construction_destroyed_right";
    public const string KEY_CONSTRUCTION_DESTROYED_DOWN_TWICE = "construction_destroyed_down_twice";
    public const string KEY_CONSTRUCTION_DESTROYED_UP_TWICE = "construction_destroyed_up_twice";
    public const string KEY_CONSTRUCTION_DESTROYED_LEFT_TWICE = "construction_destroyed_left_twice";
    public const string KEY_CONSTRUCTION_DESTROYED_RIGHT_TWICE = "construction_destroyed_right_twice";
    public const string KEY_CONSTRUCTION_DESTROYED_LEFT_RIGHT = "construction_destroyed_left_right";
    public const string KEY_CONSTRUCTION_DESTROYED_UP_DOWN = "construction_destroyed_up_down";
    public const string KEY_CONSTRUCTION_DESTROYED_UP_LEFT = "construction_destroyed_up_left";
    public const string KEY_CONSTRUCTION_DESTROYED_RIGHT_UP = "construction_destroyed_right_up";
    public const string CONSTRUCTION_DESTROYED_DOWN_LEFT = "construction_destroyed_down_left";
    public const string KEY_CONSTRUCTION_DESTROYED_DOWN_RIGHT = "construction_destroyed_down_right";
    public const string KEY_CONSTRUCTION_DESTROYED = "construction_destroyed";
    public const string KEY_BULLET = "bullet";
    public const string KEY_TANK_UP = "tank_up";
    public const string KEY_TANK_RIGHT = "tank_right";
    public const string KEY_TANK_DOWN = "tank_down";
    public const string KEY_TANK_LEFT = "tank_left";
    public const string KEY_OTHER_TANK_UP = "other_tank_up";
    public const string KEY_OTHER_TANK_RIGHT = "other_tank_right";
    public const string KEY_OTHER_TANK_DOWN = "other_tank_down";
    public const string KEY_OTHER_TANK_LEFT = "other_tank_left";
    public const string KEY_AI_TANK_UP = "ai_tank_up";
    public const string KEY_AI_TANK_RIGHT = "ai_tank_right";
    public const string KEY_AI_TANK_DOWN = "ai_tank_down";
    public const string KEY_AI_TANK_LEFT = "ai_tank_left";

}