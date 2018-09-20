using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems
{
    public static char WITHOUT_CHANGES = '#';

    public const char NONE = ' '; //  незанятая клетка
    public const char BATTLE_WALL = '☼'; //  неразрушаемая стена
    public const char BANG = 'Ѡ'; //  взрыв снаряда
    public const char BONUS_AMMO = '◊'; //  бонусные патроны(+5)
    public const char MEDKIT = '☺'; //  доп.жизнь(+1)
    public const char CONSTRUCTION = '╬'; //  разрушаемая стена(нужно 3 попадания для разрушения)
    public const char CONSTRUCTION_DESTROYED_DOWN = '╩'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const char CONSTRUCTION_DESTROYED_UP = '╦'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const char CONSTRUCTION_DESTROYED_LEFT = '╠'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const char CONSTRUCTION_DESTROYED_RIGHT = '╣'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const char CONSTRUCTION_DESTROYED_DOWN_TWICE = '╨'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_UP_TWICE = '╥'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_LEFT_TWICE = '╞'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_RIGHT_TWICE = '╡'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_LEFT_RIGHT = '│'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_UP_DOWN = '─'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_UP_LEFT = '┌'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_RIGHT_UP = '┐'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_DOWN_LEFT = '└'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED_DOWN_RIGHT = '┘'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const char CONSTRUCTION_DESTROYED = ' '; //  разрушаемая стена(уничтоженная)
    public const char HEDGEHOG = 'ͱ'; //  еж
    public const char BULLET_UP = '↥';
    public const char BULLET_RIGHT = '↦';
    public const char BULLET_DOWN = '↧';
    public const char BULLET_LEFT = '↤';
    public const char TANK_UP = '▲'; //  танк игрока, повернут наверх
    public const char TANK_RIGHT = '►'; //  танк игрока, повернут направо
    public const char TANK_DOWN = '▼'; //  танк игрока, повернут вниз
    public const char TANK_LEFT = '◄'; //  танк игрока, повернут налево
    public const char OTHER_TANK_UP = '˄'; //  танк игрока соперника, повернут наверх
    public const char OTHER_TANK_RIGHT = '˃'; //  танк игрока соперника, повернут направо
    public const char OTHER_TANK_DOWN = '˅'; //  танк игрока соперника, повернут вниз
    public const char OTHER_TANK_LEFT = '˂'; //  танк игрока соперника, повернут налево
    public const char AI_TANK_UP = '?'; //  танк AI, повернут вверх
    public const char AI_TANK_RIGHT = '»'; //  танк AI, повернут направо
    public const char AI_TANK_DOWN = '¿'; //  танк AI, повернут вних
    public const char AI_TANK_LEFT = '«'; //  танк AI, повернут налево
    public const char WORM_HOLE = 'ʘ'; //  телепорт
    public const char BOG = '@'; //  болото(танк застревает и не может перемещаться по карте)
    public const char SAND = '□'; //  песок(танку нужна 2 хода для пересечения клетки с песком)
    public const char MOAT_HORIZONTAL = '='; //  ров(танку нужна 2 хода для пересечения клетки со рвом)
    public const char MOAT_VERTICAL = '‖'; //  ров(танку нужна 2 хода для пересечения клетки со рвом)
}
