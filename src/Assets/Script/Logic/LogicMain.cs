using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Logic
{
    public class LogInEvent : UnityEvent<string> { }
    public class UpdateEvent : UnityEvent<Dictionary<string, string>> { }
    public static class LogicMain
    {
        public volatile static Dictionary<string, List<Dictionary<string, string>>> ackTask;
        public volatile static User chapter;
        public volatile static LogInEvent logInEvent;
        public volatile static UpdateEvent chapterUpdateEvent;
        public static void Initialize()
        {
            ackTask = new Dictionary<string, List<Dictionary<string, string>>>();
            logInEvent = new LogInEvent();
            chapter = new User();
            chapterUpdateEvent = new UpdateEvent();
        }

        public static void ParseACK(Dictionary<string,string> ack)
        {
            if(!ackTask.ContainsKey(ack["calling"]))
            {
                ackTask[ack["calling"]] = new List<Dictionary<string, string>>();
            }
            ackTask[ack["calling"]].Add(ack);
        }
    }
}

