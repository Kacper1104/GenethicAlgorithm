using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genethic_Algorithm
{
    class Loader
    {
        string filepath;
        int knapsackCount;
        int[] knapsacksCapacities;
        int itemCount;
        int[] itemValues;
        int[] itemWeights;
        int mainKnapsackIdx;
        int knapsackCapacitiesSum;

        //Constructors
        public Loader(string filepath) { this.filepath = filepath; }
        //Getters and Setters
        public int KnapsackCount { get => knapsackCount; set => knapsackCount = value; }
        public int[] KnapsacksCapacities { get => knapsacksCapacities; set => knapsacksCapacities = value; }
        public int ItemCount { get => itemCount; set => itemCount = value; }
        public int[] ItemValues { get => itemValues; set => itemValues = value; }
        public int[] ItemWeights { get => itemWeights; set => itemWeights = value; }
        public int MainKnapsackIdx { get => mainKnapsackIdx; set => mainKnapsackIdx = value; }
        public int KnapsackCapacitiesSum { get => knapsackCapacitiesSum; set => knapsackCapacitiesSum = value; }
        //Methods
        public void readFile()
        {
            StreamReader sr = new StreamReader(filepath);
            string line = sr.ReadLine(); //<knapsacks count>,<items count>
            string[] split = line.Split(',');
            knapsackCount = int.Parse(split[0]);
            itemCount = int.Parse(split[1]);
            mainKnapsackIdx = int.MinValue;
            line = sr.ReadLine(); //<capacity_knapsack_0>,...,<capacity_knapsack_n>
            split = line.Split(',');
            knapsacksCapacities = new int[knapsackCount];
            knapsackCapacitiesSum = 0;
            for(int i = 0; i < knapsackCount; i++)
            {
                knapsacksCapacities[i] = int.Parse(split[i]);
                if(knapsacksCapacities[i] > mainKnapsackIdx)
                {
                    mainKnapsackIdx = i;
                }
                knapsackCapacitiesSum += knapsacksCapacities[i];
            }
            itemValues = new int[itemCount];
            itemWeights = new int[itemCount];
            for(int i = 0; i < itemCount; i++)
            {
                line = sr.ReadLine(); //<value_item_i>,<weight_item_i>
                split = line.Split(',');
                itemValues[i] = int.Parse(split[0]);
                itemWeights[i] = int.Parse(split[1]);
            }
            sr.Close();
        }
    }
}
