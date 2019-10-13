using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genethic_Algorithm
{
    class Loader
    {
        string filepath;
        int itemCount;
        int capacity;
        int dimensions;
        double minspeed;
        double maxspeed; 

        //City[] cities;
        //Item[] items;

        public Loader(string filepath) { this.filepath = filepath; }

        public int Dimensions { get => dimensions; set => dimensions = value; }

        void readFile()
        {
            StreamReader sr = new StreamReader(filepath);
            sr.ReadLine(); //problem name
            sr.ReadLine(); //knapsack
            dimensions = int.Parse(sr.ReadLine());
            itemCount = int.Parse(sr.ReadLine());
            capacity = int.Parse(sr.ReadLine());
            minspeed = int.Parse(sr.ReadLine());
            maxspeed = int.Parse(sr.ReadLine());
            sr.ReadLine(); //ratio
            sr.ReadLine(); //weight type
            sr.ReadLine(); //city table pattern
            //cities = new City[dimensions];
            for (int i = 0; i < dimensions; i++)
            {

            }
        }
    }
}
