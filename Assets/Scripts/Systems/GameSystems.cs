using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

[EcsInject]
public class GameSystems : IEcsInitSystem, IEcsRunSystem, GameStateEventManager.Handler
{

    private static int GAME_WAIT_FOR_DATA = 0;
    private static int GAME_STARTED = 1;

    EcsWorld _world = null;

    private int _gameState = GAME_WAIT_FOR_DATA;

    void IEcsInitSystem.Initialize()
    {
        GameStateEventManager.getInstance().subscribe(this);
        connectToServer();

        onUpdateBattlefield(BattleField.SAMPLE_SMALL);
    }

    void IEcsInitSystem.Destroy()
    {
        GameStateEventManager.getInstance().unSubscibe(this);
    }

    void IEcsRunSystem.Run()
    {
        
    }

    private void connectToServer()
    {
        // TODO: connect with web socket
    }

    public void onUpdateBattlefield(string battlefield)
    {
        if (_gameState == GAME_WAIT_FOR_DATA)
        {

            var wallProcessor = new WallProcessor();
            var tanksProcessor = new TankProcessor();
            var undestroyableWallProcessor = new UndestroyableWallProcessor();
            var bulletProcessor = new BulletProcessor();

            char[][] field = BattleField.to2Dimension(battlefield);
            for (var i = 0; i < field.Length; i++)
            {
                for (var j = 0; j < field[i].Length; j++)
                {
                    wallProcessor.process(field, i, j);
                    tanksProcessor.process(field, i, j);
                    undestroyableWallProcessor.process(field, i, j);
                    bulletProcessor.process(field, i, j);
                }
            }

            int fieldSize = field.Length;

            initWalls(wallProcessor, fieldSize);
            initTanks(tanksProcessor, fieldSize);
        } else
        {
            // TODO: update field
        }
    }

    private void initWalls(WallProcessor wallProcessor, int fieldSize)
    {
        var walls = wallProcessor.getItems();
        Wall wall;
        Object wallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Wall.prefab", typeof(GameObject));
        GameObject unityObject;
        Vector3 position;
        Debug.Log("Walls count: " + walls.Count);
        for (var i = 0; i < walls.Count; i++)
        {
            Debug.Log("Wall: " + walls[i].row + ", " + walls[i].column);
            position = MapUtils.getWorldPosition(fieldSize, walls[i].row, walls[i].column);
            unityObject = Object.Instantiate(wallPrefab, position, Quaternion.Euler(0, 0, 0)) as GameObject;
            wall = SystemUtils.getComponent<Wall>(_world, unityObject);
            wall.column = walls[i].column;
            wall.row = walls[i].row;
            wall.position = position;
        }
    }

    private void initTanks(TankProcessor processor, int fieldSize)
    {
        var tanks = processor.getItems();
        Tank tank;
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tank.prefab", typeof(GameObject));
        GameObject unityObject;
        Vector3 position;
        Quaternion rotation;
        for (var i = 0; i < tanks.Count; i++)
        {
            rotation = MapUtils.getWorlRotation(tanks[i].direction);
            Debug.Log("Tank: " + tanks[i].direction + ">"+ rotation);
            position = MapUtils.getWorldPosition(fieldSize, tanks[i].row, tanks[i].column);
            unityObject = Object.Instantiate(prefab, position, rotation) as GameObject;
            tank = SystemUtils.getComponent<Tank>(_world, unityObject);
            tank.name = tanks[i].name;
            tank.direction = tanks[i].direction;
            tank.column = tanks[i].column;
            tank.row = tanks[i].row;
            tank.transform = unityObject.transform;
            tank.characterController = unityObject.GetComponent<CharacterController>();
        }
    }
}