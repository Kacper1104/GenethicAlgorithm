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
        bool isMulti;

        //Constructors
        public Loader(string filepath, bool isMulti) { this.filepath = filepath; this.isMulti = isMulti; }
        //Getters and Setters
        public int KnapsackCount { get => knapsackCount; set => knapsackCount = value; }
        public int[] KnapsacksCapacities { get => knapsacksCapacities; set => knapsacksCapacities = value; }
        public int ItemCount { get => itemCount; set => itemCount = value; }
        public int[] ItemValues { get => itemValues; set => itemValues = value; }
        public int[] ItemWeights { get => itemWeights; set => itemWeights = value; }
        public int MainKnapsackIdx { get => mainKnapsackIdx; set => mainKnapsackIdx = value; }
        public int KnapsackCapacitiesSum { get => knapsackCapacitiesSum; set => knapsackCapacitiesSum = value; }
        public bool IsMulti { get => isMulti; set => isMulti = value; }
        //Methods
        public void readFile()
        {
            StreamReader sr = new StreamReader(filepath);
            string line;
            string[] split;
            if (isMulti)
            {
                line = sr.ReadLine(); //<knapsacks count>,<items count>
                split = line.Split(',');
                knapsackCount = int.Parse(split[0]);
                itemCount = int.Parse(split[1]);
                mainKnapsackIdx = int.MinValue;
                line = sr.ReadLine(); //<capacity_knapsack_0>,...,<capacity_knapsack_n>
                split = line.Split(',');
                knapsacksCapacities = new int[knapsackCount];
                knapsackCapacitiesSum = 0;
                for (int i = 0; i < knapsackCount; i++)
                {
                    knapsacksCapacities[i] = int.Parse(split[i]);
                    if (knapsacksCapacities[i] > mainKnapsackIdx)
                    {
                        mainKnapsackIdx = i;
                    }
                    knapsackCapacitiesSum += knapsacksCapacities[i];
                }
            }
            else
            {
                knapsackCount = 1;
                line = sr.ReadLine(); //<items count>,<knapsack capacity>
                split = line.Split(',');
                itemCount = int.Parse(split[0]);
                knapsacksCapacities = new int[] { int.Parse(split[1]) };
                knapsackCapacitiesSum = KnapsacksCapacities[0];
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
