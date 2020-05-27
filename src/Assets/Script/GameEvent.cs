using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameEvent
{
    public enum GameEventType
    {
        GameStart,
        GameOver,
        AddPlayer,
        DelPlayer,
        UpdateEnemy,
        EnemyDead,
    }

    public static class GameEvent
    {
        private static Dictionary<GameEventType, List<MonoBehaviour>> eventLists;
        public static void Initialized()
        {
            eventLists = new Dictionary<GameEventType, List<MonoBehaviour>>();
        }
        public static void RegisterEvent(GameEventType type, MonoBehaviour behaviour)
        {
            if (!eventLists.ContainsKey(type))
                eventLists[type] = new List<MonoBehaviour>();
            eventLists[type].Add(behaviour);
        }
        public static void UnRegisterEvent(GameEventType type)
        {
            eventLists[type] = null;
        }
        public static Dictionary<GameEventType, List<MonoBehaviour>> GetEventLists()
        {
            return eventLists;
        }
        public static void Clear()
        {
            foreach (var each in eventLists)
                each.Value.Clear();
        }
    }
}
