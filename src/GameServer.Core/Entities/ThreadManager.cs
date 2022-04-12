using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Core.Entities
{
    public class ThreadManager
    {
        private static readonly List<Action> ExecuteOnMainThreadList = new List<Action>();
        private static readonly List<Action> ExecuteCopiedOnMainThreadList = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;       

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }

            lock (ExecuteOnMainThreadList)
            {
                ExecuteOnMainThreadList.Add(_action);
                actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        public static void UpdateMain()
        {
            if (actionToExecuteOnMainThread)
            {
                ExecuteCopiedOnMainThreadList.Clear();
                lock (ExecuteOnMainThreadList)
                {
                    ExecuteCopiedOnMainThreadList.AddRange(ExecuteOnMainThreadList);
                    ExecuteOnMainThreadList.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < ExecuteCopiedOnMainThreadList.Count; i++)
                {
                    ExecuteCopiedOnMainThreadList[i]();
                }
            }
        }
    }
}

