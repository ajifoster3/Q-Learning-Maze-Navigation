using System;
using System.Collections.Generic;
using System.Linq;

namespace test
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            
            List<int> stateList = new List<int> { 1, 2, 11, 12, 13, 23, 24, 25, 26, 36, 46, 56, 57, 58, 59, 33, 43, 53, 63, 73, 83, 84 };
            List<List<double>> qValues = generateQValues(stateList);
            for(int i = 0; i < 1000; i++)
            {
                qValues = episode(stateList, qValues);
            }

            int state = 1;
            int end = 84;
            Console.Clear();
            while (state != end)
            {
                Console.WriteLine(state);
                state = transitionFunction(state, eGreedyerAction(state, qValues, stateList), stateList);         
            }

        }


        public static List<List<double>> episode(List<int> stateList, List<List<double>> qValues)
        {
            int goalstate = 84;
            int state = startingState(stateList);
            int i = 0;
            while(state != goalstate)
            {
                int action = eGreedyAction(state, qValues, stateList);
                int newState = transitionFunction(state, action, stateList);
                int reward = rewardFunction(state, action);
                qValues = updateQ(qValues, state, action-1, newState, reward, stateList);
                state = newState;
                i++;
            }
            return qValues;

        }

        public static void printMaze(List<int> stateList)
        {
            for(int i = 9; i >= 0; i--)
            {
                for (int j = 0; j < 10 ; j++)
                {
                    if (stateList.Contains((j + 1) + (i * 10)))
                        Console.Write("x");
                    else
                        Console.Write("O");
                            
                }
                Console.WriteLine();
            }
        }

        public static int rewardFunction(int state, int action)
        {
            return ((state == 83) && (action == 2)) ? 10 : 0;
        }

        public static int transitionFunction(int state, int action, List<int> stateList)
        {
            int newState;
            switch (action)
            {
                case 1:
                    newState = state < 91 ? state + 10: state;
                    break;
                case 2:
                    newState = !(state % 10 == 0) ? state + 1 : state;
                    break;
                case 3:
                    newState = 10 < state ? state - 10 : state;
                    break;
                case 4:
                    newState = !((state - 1) % 10 == 0) ? state - 1 : state;
                    break;
                default:
                    return state;
            }
            return stateList.Contains(newState) ? newState : state;
        }

        public static int startingState(List<int> stateList)
        {
            
            return stateList[random.Next(0, stateList.Count)];
        }

        public static List<List<double>> generateQValues(List<int> stateList)
        {
            List<List<double>> qValues = new List<List<double>>();
            stateList.ForEach(value => {
                qValues.Add(new List<double> {
                    (random.NextDouble() * .09) + .01,
                    (random.NextDouble() * .09) + .01,
                    (random.NextDouble() * .09) + .01,
                    (random.NextDouble() * .09) + .01
                });
            });
            return qValues;
        }

        public static int eGreedyAction(int state, List<List<double>> qValues, List<int> stateList)
        {
            if(random.Next(0, 9) == 0) { return random.Next(1, 4); }
            int action = qValues[stateList.IndexOf(state)].IndexOf(qValues[stateList.IndexOf(state)].Max());
            return action + 1;
        }

        public static int eGreedyerAction(int state, List<List<double>> qValues, List<int> stateList)
        {
            int action = qValues[stateList.IndexOf(state)].IndexOf(qValues[stateList.IndexOf(state)].Max());
            return action + 1;
        }

        public static List<List<double>> updateQ(List<List<double>> qValues, int state, int action, int newState, int reward, List<int> stateList)
        {
            qValues[stateList.IndexOf(state)][action] = qValues[stateList.IndexOf(state)][action] + (.2*(reward + (.9*qValues[stateList.IndexOf(newState)].Max()- qValues[stateList.IndexOf(state)][action])));
            return qValues;
        }
    }
}
