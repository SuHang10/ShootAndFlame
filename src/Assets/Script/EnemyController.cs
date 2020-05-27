using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebManager;
using Logic;

namespace Controller
{
    public static class EnemyController
    {
        public static Dictionary<string, Enemy> enemyDict;

        public static void Initialized()
        {
            if(enemyDict==null)
                enemyDict = new Dictionary<string, Enemy>();
        }

        public static void EnemyDead(MonoBehaviour behaviour)
        {
            Enemy enemy = behaviour as Enemy;
            ShowReward(enemy);
            var enemyMsg = MessageConverter.OnEnemyDeadMsg(enemy);
            Client.SendMsg(enemyMsg);
            Object.Destroy(behaviour.gameObject);
        }
        private static void ShowReward(Enemy enemy)
        {
            Debug.Log(enemy.name);
            Debug.Log("奖励为：" + enemy.reward.name);
            var re = Object.Instantiate(enemy.reward);
            re.transform.position = enemy.transform.position + new Vector3(0, 0.5f, 0);
        }

        /*public static void RecvEnemyDead(List<Dictionary<string, string>> enemy_deadList)
        {
            if (enemy_deadList.Count > 0)
            {
                foreach (var enemy in enemy_deadList)
                {
                    Debug.Log($"收到enemy dead信息{enemy["name"]}");
                    if (enemyDict.ContainsKey(enemy["name"]))
                    {
                        Destroy(enemyDict[enemy["name"]].gameObject);
                    }
                }
            }
            LogicMain.ackTask["enemy_dead"].Clear();
        }

        private void Enemy_Dead(Dictionary<string, string> enemy)
        {
            Debug.Log($"收到enemy dead信息{enemy["name"]}");
            if (enemyDict.ContainsKey(enemy["name"]))
            {
                Destroy(enemyDict[enemy["name"]].gameObject);
            }
        }*/
    }
}
