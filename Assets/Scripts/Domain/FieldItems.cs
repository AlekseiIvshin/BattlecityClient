using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems
{
    public static char WITHOUT_CHANGES = '#';

    public const int None = 0; //  незанятая клетка
    public const int BattleWall = 1; //  неразрушаемая стена
    public const int Bang = 2; //  взрыв снаряда
    public const int BonusAmmo = 3; //  бонусные патроны(+5)
    public const int Medkit = 4; //  доп.жизнь(+1)
    public const int Construction = 5; //  разрушаемая стена(нужно 3 попадания для разрушения)
    public const int ConstructionDestroyedDown = 6; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const int ConstructionDestroyedUp = 7; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const int ConstructionDestroyedLeft = 8; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const int ConstructionDestroyedRight = 9; //  разрушаемая стена(нужно 2 попадания для разрушения)
    public const int ConstructionDestroyedDownTwice = 10; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedUpTwice = 11; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedLeftTwice = 12; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedRightTwice = 13; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedLeftRight = 14; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedUpDown = 15; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedUpLeft = 16; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedRightUp = 17; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedDownLeft = 18; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyedDownRight = 19; //  разрушаемая стена(нужно 1 попадание для разрушения)
    public const int ConstructionDestroyed = 20; //  разрушаемая стена(уничтоженная)
    public const int Hedgehog = 21; //  еж
    public const int BulletUp = 22;
    public const int BulletRight = 22;
    public const int BulletDown = 22;
    public const int BulletLeft = 22;
    public const int TankUp = 23; //  танк игрока, повернут наверх
    public const int TankRight = 24; //  танк игрока, повернут направо
    public const int TankDown = 25; //  танк игрока, повернут вниз
    public const int TankLeft = 26; //  танк игрока, повернут налево
    public const int OtherTankUp = 27; //  танк игрока соперника, повернут наверх
    public const int OtherTankRight = 28; //  танк игрока соперника, повернут направо
    public const int OtherTankDown = 29; //  танк игрока соперника, повернут вниз
    public const int OtherTankLeft = 30; //  танк игрока соперника, повернут налево
    public const int AiTankUp = 31; //  танк ai, повернут вверх
    public const int AiTankRight = 32; //  танк ai, повернут направо
    public const int AiTankDown = 33; //  танк ai, повернут вних
    public const int AiTankLeft = 34; //  танк ai, повернут налево
    public const int WormHole = 35; //  телепорт
    public const int Bog = 36; //  болото(танк застревает и не может перемещаться по карте)
    public const int Sand = 37; //  песок(танку нужна 2 хода для пересечения клетки с песком)
    public const int MoatHorizontal = 38; //  ров(танку нужна 2 хода для пересечения клетки со рвом)
    public const int MoatVertical = 39; //  ров(танку нужна 2 хода для пересечения клетки со рвом)

    private const string FIELD = " ☼Ѡ◊☺╬╩╦╠╣╨╥╞╡│─┌┐└┘ ͱ↥↦↧↤▲►▼◄˄˃˅˂?»¿«ʘ@□=‖";
    private const string FIELD_ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public static string SYMBOLS = FIELD_ALPHA;
    public static char[] SYMBOLS_ARRAY = FIELD_ALPHA.ToCharArray();

    // v******************** V2 ***************************v //
    //public const int None = 0; //  незанятая клетка
    //public const int BattleWall = 1; //  неразрушаемая стена
    //public const int Bang = 2; //  взрыв снаряда
    //public const int BonusAmmo = 3; //  бонусные патроны(+5)
    //public const int Medkit = 4; //  доп.жизнь(+1)
    //public const int Construction = 5; //  разрушаемая стена(нужно 3 попадания для разрушения)
    //public const int ConstructionDestroyedDown = 6; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const int ConstructionDestroyedUp = 7; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const int ConstructionDestroyedLeft = 8; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const int ConstructionDestroyedRight = 9; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const int ConstructionDestroyedDownTwice = 10; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedUpTwice = 11; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedLeftTwice = 12; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedRightTwice = 13; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedLeftRight = 14; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedUpDown = 15; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedUpLeft = 16; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedRightUp = 17; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedDownLeft = 18; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyedDownRight = 19; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const int ConstructionDestroyed = 20; //  разрушаемая стена(уничтоженная)
    //public const int Hedgehog = 21; //  еж
    //public const int BulletUp = 22;
    //public const int BulletRight = 23;
    //public const int BulletDown = 24;
    //public const int BulletLeft = 25;
    //public const int TankUp = 26; //  танк игрока, повернут наверх
    //public const int TankRight = 27; //  танк игрока, повернут направо
    //public const int TankDown = 28; //  танк игрока, повернут вниз
    //public const int TankLeft = 29; //  танк игрока, повернут налево
    //public const int OtherTankUp = 30; //  танк игрока соперника, повернут наверх
    //public const int OtherTankRight = 31; //  танк игрока соперника, повернут направо
    //public const int OtherTankDown = 32; //  танк игрока соперника, повернут вниз
    //public const int OtherTankLeft = 33; //  танк игрока соперника, повернут налево
    //public const int AiTankUp = 34; //  танк ai, повернут вверх
    //public const int AiTankRight = 35; //  танк ai, повернут направо
    //public const int AiTankDown = 36; //  танк ai, повернут вних
    //public const int AiTankLeft = 37; //  танк ai, повернут налево
    //public const int WormHole = 38; //  телепорт
    //public const int Bog = 39; //  болото(танк застревает и не может перемещаться по карте)
    //public const int Sand = 40; //  песок(танку нужна 2 хода для пересечения клетки с песком)
    //public const int MoatHorizontal = 41; //  ров(танку нужна 2 хода для пересечения клетки со рвом)
    //public const int MoatVertical = 42; //  ров(танку нужна 2 хода для пересечения клетки со рвом)


    //private const string FIELD = " ☼Ѡ◊☺╬╩╦╠╣╨╥╞╡│─┌┐└┘ ͱ↥↦↧↤▲►▼◄˄˃˅˂?»¿«ʘ@□=‖";
    //private const string FIELD_ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    // ^******************** V2 ***************************^ //




    //public const char NONE = ' '; //  незанятая клетка
    //public const char BATTLE_WALL = '☼'; //  неразрушаемая стена
    //public const char BANG = 'Ѡ'; //  взрыв снаряда
    //public const char BONUS_AMMO = '◊'; //  бонусные патроны(+5)
    //public const char MEDKIT = '☺'; //  доп.жизнь(+1)
    //public const char CONSTRUCTION = '╬'; //  разрушаемая стена(нужно 3 попадания для разрушения)
    //public const char CONSTRUCTION_DESTROYED_DOWN = '╩'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const char CONSTRUCTION_DESTROYED_UP = '╦'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const char CONSTRUCTION_DESTROYED_LEFT = '╠'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const char CONSTRUCTION_DESTROYED_RIGHT = '╣'; //  разрушаемая стена(нужно 2 попадания для разрушения)
    //public const char CONSTRUCTION_DESTROYED_DOWN_TWICE = '╨'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_UP_TWICE = '╥'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_LEFT_TWICE = '╞'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_RIGHT_TWICE = '╡'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_LEFT_RIGHT = '│'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_UP_DOWN = '─'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_UP_LEFT = '┌'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_RIGHT_UP = '┐'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_DOWN_LEFT = '└'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED_DOWN_RIGHT = '┘'; //  разрушаемая стена(нужно 1 попадание для разрушения)
    //public const char CONSTRUCTION_DESTROYED = ' '; //  разрушаемая стена(уничтоженная)
    //public const char HEDGEHOG = 'ͱ'; //  еж
    //public const char BULLET_UP = '↥';
    //public const char BULLET_RIGHT = '↦';
    //public const char BULLET_DOWN = '↧';
    //public const char BULLET_LEFT = '↤';
    //public const char TANK_UP = '▲'; //  танк игрока, повернут наверх
    //public const char TANK_RIGHT = '►'; //  танк игрока, повернут направо
    //public const char TANK_DOWN = '▼'; //  танк игрока, повернут вниз
    //public const char TANK_LEFT = '◄'; //  танк игрока, повернут налево
    //public const char OTHER_TANK_UP = '˄'; //  танк игрока соперника, повернут наверх
    //public const char OTHER_TANK_RIGHT = '˃'; //  танк игрока соперника, повернут направо
    //public const char OTHER_TANK_DOWN = '˅'; //  танк игрока соперника, повернут вниз
    //public const char OTHER_TANK_LEFT = '˂'; //  танк игрока соперника, повернут налево
    //public const char AI_TANK_UP = '?'; //  танк AI, повернут вверх
    //public const char AI_TANK_RIGHT = '»'; //  танк AI, повернут направо
    //public const char AI_TANK_DOWN = '¿'; //  танк AI, повернут вних
    //public const char AI_TANK_LEFT = '«'; //  танк AI, повернут налево
    //public const char WORM_HOLE = 'ʘ'; //  телепорт
    //public const char BOG = '@'; //  болото(танк застревает и не может перемещаться по карте)
    //public const char SAND = '□'; //  песок(танку нужна 2 хода для пересечения клетки с песком)
    //public const char MOAT_HORIZONTAL = '='; //  ров(танку нужна 2 хода для пересечения клетки со рвом)
    //public const char MOAT_VERTICAL = '‖'; //  ров(танку нужна 2 хода для пересечения клетки со рвом)

}