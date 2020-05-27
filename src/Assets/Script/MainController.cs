using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGameEvent;
using Logic;
using WebManager;

namespace Controller
{
    public class MainController : MonoBehaviour
    {
        public Enemy solder, zombie;

        //private Dictionary<string, Enemy> enemyDict;

        void Start()
        {
            EnemyController.Initialized();
            GenerateEnemyFromLocal();
            GameEvent.Initialized();

            /*DontDestroyOnLoad(this);
            if (GameObject.Find(name).gameObject != this.gameObject)
                Destroy(this.gameObject);*/
        }

        void FixedUpdate()
        {
            foreach (var eventlist in GameEvent.GetEventLists())
            {
                switch (eventlist.Key)
                {
                    case GameEventType.EnemyDead:
                        foreach (var behaviour in eventlist.Value)
                        {
                            EnemyController.EnemyDead(behaviour);
                            LogicMain.chapter.AddEXP();
                            EnemyController.enemyDict.Remove(behaviour.name);
                        }
                        break;
                    default:
                        break;
                }
            }
            GameEvent.Clear();
            
            if (LogicMain.ackTask.ContainsKey("user_dead"))
            {
                var user_deadList = LogicMain.ackTask["user_dead"];
                if (user_deadList.Count > 0)
                {
                    foreach (var user in user_deadList)
                    {
                        Debug.Log($"收到user dead信息{user["name"]}");
                        if (EnemyController.enemyDict.ContainsKey(user["name"]))
                        {
                            Destroy(EnemyController.enemyDict[user["name"]].gameObject);
                        }
                    }
                }
                LogicMain.ackTask["enemy_dead"].Clear();
            }

            if (LogicMain.ackTask.ContainsKey("enemy_dead"))
            {
                var enemy_deadList = LogicMain.ackTask["enemy_dead"];
                if (enemy_deadList.Count > 0)
                {
                    foreach (var enemy in enemy_deadList)
                    {
                        EnemyDead(enemy);
                    }
                }
                LogicMain.ackTask["enemy_dead"].Clear();
            }
            if (LogicMain.ackTask.ContainsKey("enemy_generate"))
            {
                var enemy_generateList = LogicMain.ackTask["enemy_generate"];
                if (enemy_generateList.Count > 0)
                {
                    for(int i=0;i<enemy_generateList.Count;i++)
                    {
                        GenerateEnemy(enemy_generateList[i]);
                    }
                }
                LogicMain.ackTask["enemy_generate"].Clear();
            }
            if (LogicMain.ackTask.ContainsKey("enemy_move"))
            {
                var enemy_moveList = LogicMain.ackTask["enemy_move"];
                if (enemy_moveList.Count > 0)
                {
                    for (int i=0;i<enemy_moveList.Count;i++)
                    {
                        var enemy = enemy_moveList[i];
                        if (EnemyController.enemyDict!=null&&
                            !EnemyController.enemyDict.ContainsKey(enemy["name"]))
                        {
                            GenerateEnemy(enemy);
                        }
                        var target = GameObject.Find(enemy["target"]);
                        EnemyController.enemyDict[enemy["name"]].SetState(enemy["state"]);
                        EnemyController.enemyDict[enemy["name"]].target = target.gameObject;
                    }
                }
                LogicMain.ackTask["enemy_move"].Clear();
            }
            if (LogicMain.ackTask.ContainsKey("enemy_attack"))
            {
                var enemy_attackList = LogicMain.ackTask["enemy_attack"];
                if (enemy_attackList.Count > 0)
                {
                    for (int i = 0; i < enemy_attackList.Count; i++)
                    {
                        var enemy = enemy_attackList[i];
                        if (!EnemyController.enemyDict.ContainsKey(enemy["name"]))
                            GenerateEnemy(enemy);
                        var target = GameObject.Find(enemy["target"]);
                        EnemyController.enemyDict[enemy["name"]].SetState(enemy["state"]);
                        EnemyController.enemyDict[enemy["name"]].target = target.gameObject;
                    }
                }
                LogicMain.ackTask["enemy_attack"].Clear();
            }
        }

        private void OnDestroy()
        {
            Debug.Log("切换场景");
            //Client.Close();
        }

        private void EnemyDead(Dictionary<string, string> enemy)
        {
            Debug.Log($"收到enemy dead信息{enemy["name"]}");
            if (EnemyController.enemyDict.ContainsKey(enemy["name"]))
            {
                Destroy(EnemyController.enemyDict[enemy["name"]].gameObject);
            }
        }

        private void GenerateEnemy(Dictionary<string,string> enemy)
        {
            Enemy _enemy;
            if (enemy["type"] == "Soidier")
                _enemy = Instantiate(solder);
            else
                _enemy = Instantiate(zombie);
            _enemy.name = enemy["name"];
            _enemy.curHP = int.Parse(enemy["hp"]);
            _enemy.SetState(enemy["state"]);
            _enemy.transform.position =
                new Vector3(float.Parse(enemy["pos_x"]) / 2 - 30,
                float.Parse(enemy["pos_y"]),
                float.Parse(enemy["pos_z"]) / 2 - 30);
            if (enemy["target"] != "null")
            {
                var target = GameObject.Find(enemy["target"]);
                _enemy.target = target.gameObject;
            }
            EnemyController.enemyDict[enemy["name"]] = _enemy;
        }

        private void GenerateEnemyFromLocal()
        {
            var keys = new List<string>(EnemyController.enemyDict.Keys);
            for(int i=0;i<keys.Count;i++)
            {
                var key = keys[i];
                var value = EnemyController.enemyDict[key];
                Enemy _enemy;
                if (value is Zombie)
                {
                    Debug.Log(value.GetType());
                    _enemy = Instantiate(zombie);
                }
                else
                    _enemy = Instantiate(solder);
                _enemy.name = key;
                _enemy.curHP = value.curHP;
                _enemy.SetState(value.GetState());
                _enemy.transform.position = value.position;
                _enemy.target = value.target;
                EnemyController.enemyDict[key] = _enemy;
            }


            /*foreach (var each in EnemyController.enemyDict)
            {
                Enemy _enemy;
                if(each.Value is Zombie)
                {
                    Debug.Log(each.Value.GetType());
                    _enemy = Instantiate(zombie);
                }
                else
                    _enemy = Instantiate(solder);
                _enemy.name = each.Key;
                _enemy.curHP = each.Value.curHP;
                _enemy.SetState(each.Value.GetState());
                _enemy.transform.position = each.Value.position;
                _enemy.target = each.Value.target;
                EnemyController.enemyDict[each.Key] = _enemy;
            }*/
        }
    }

}