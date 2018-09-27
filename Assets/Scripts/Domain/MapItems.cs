using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItems
{
    public static Dictionary<char, string> MAP_KEYS;

    public static char WITHOUT_CHANGES = '#';
    public static char OUTBOUNDS = '@';

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
    public const string KEY_HEDGEHOG = "hedgehog";
    public const string KEY_BONUS_AMMO = "bonus_ammo";
    public const string KEY_MEDKIT = "medkit";
    public const string KEY_BULLET_UP = "bullet_up";
    public const string KEY_BULLET_RIGHT = "bullet_right";
    public const string KEY_BULLET_DOWN = "bullet_down";
    public const string KEY_BULLET_LEFT = "bullet_left";
    public const string KEY_WORM_HOLE = "worm_hole";
    public const string KEY_BOG = "bog";
    public const string KEY_SAND = "sand";
    public const string KEY_MOAT_HORIZONTAL = "moat_horizontal";
    public const string KEY_MOAT_VERTICAL = "moat_vertical";

    public const string PREFAB_WALL = "Wall";
    public const string PREFAB_BATTLE_WALL = "BattleWall";
    public const string PREFAB_TANK = "Tank";
    public const string PREFAB_BULLET = "Shell";
    public const string PREFAB_MEDKIT = "Medkit";
    public const string PREFAB_AMMO_BOX = "AmmoBox";
    public const string PREFAB_HEDGEHOG = "Hedgehog";

    public static Dictionary<string, IEnumerable<string>> PREFAB_TO_KEYS = new Dictionary<string, IEnumerable<string>>
    {
        { PREFAB_BATTLE_WALL, new string[] {KEY_BATTLE_WALL} },
        { PREFAB_BULLET, new string[] {KEY_BULLET} },
        { PREFAB_HEDGEHOG, new string[] {KEY_HEDGEHOG} },
        { PREFAB_AMMO_BOX, new string[] {KEY_BONUS_AMMO} },
        { PREFAB_MEDKIT, new string[] {KEY_MEDKIT} },
        {PREFAB_TANK, new string[]{
            MapItems.KEY_TANK_UP,
            MapItems.KEY_OTHER_TANK_UP,
            MapItems.KEY_TANK_LEFT,
            MapItems.KEY_OTHER_TANK_LEFT,
            MapItems.KEY_TANK_RIGHT,
            MapItems.KEY_OTHER_TANK_RIGHT,
            MapItems.KEY_TANK_DOWN,
            MapItems.KEY_OTHER_TANK_DOWN,
        }},
        { PREFAB_WALL, new string[] {
            MapItems.KEY_CONSTRUCTION,
            MapItems.KEY_CONSTRUCTION_DESTROYED_DOWN,
            MapItems.KEY_CONSTRUCTION_DESTROYED_UP,
            MapItems.KEY_CONSTRUCTION_DESTROYED_LEFT,
            MapItems.KEY_CONSTRUCTION_DESTROYED_RIGHT,
            MapItems.KEY_CONSTRUCTION_DESTROYED_DOWN_TWICE,
            MapItems.KEY_CONSTRUCTION_DESTROYED_UP_TWICE,
            MapItems.KEY_CONSTRUCTION_DESTROYED_LEFT_TWICE,
            MapItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_TWICE,
            MapItems.KEY_CONSTRUCTION_DESTROYED_LEFT_RIGHT,
            MapItems.KEY_CONSTRUCTION_DESTROYED_UP_DOWN,
            MapItems.KEY_CONSTRUCTION_DESTROYED_UP_LEFT,
            MapItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_UP,
            MapItems.CONSTRUCTION_DESTROYED_DOWN_LEFT,
            MapItems.KEY_CONSTRUCTION_DESTROYED_DOWN_RIGHT,
            MapItems.KEY_CONSTRUCTION_DESTROYED,
        }}
    };
}