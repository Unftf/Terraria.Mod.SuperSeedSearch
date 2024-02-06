using System.Collections;
using System;
using System.Collections.Generic;


namespace SuperSeedSearch.Storage
{
    public static class WorldInfo
    {
        public static Hashtable Info = new Hashtable();

        public static void SetValue(object key, object value){            
            if(Info.Contains(key)) Info[key] = value;
            else Info.Add(key,value);
        }

        public static string GetValueAsString(object key){
            return Info[key].ToString();
        }
        public static string GetValueAsStringOrEmptyIfNotExists(object key){
            return HasKey(key)?Info[key].ToString():"";
        }

        public static double GetValueAsDouble(object key){
            return (double)Info[key];
        }
        public static int GetValueAsInt(object key){            
            return (int)Info[key];
        }

        public static int[] GetValueAsIntArray(object key){            
            return (int[])Info[key];
        }

        public static bool HasKey(object key){
            return Info.ContainsKey(key);
        }
        public static bool HasKeyValue(object key, string value){
            return Info.ContainsKey(key)?Info[key].ToString().Equals(value):false;
        }
        public static bool HasKeyDifferentToValue(object key, string value){
            return Info.ContainsKey(key)?!Info[key].ToString().Equals(value):false;
        }


        public static class RNGNumbers{
            public static bool predicted;
            public static int forSeed;
            public static double RNGN1;
            public static double RNGN2;
            public static double RNGN3;
            public static double RNGN4;
            public static double RNGN5;
            public static double RNGN6;
            public static double RNGN7;
            public static double RNGN8;
            public static double RNGN9;
            public static void Clear(){
                predicted = false;
                forSeed = -1;
                RNGN1=-1;
                RNGN2=-1;
                RNGN3=-1;
                RNGN4=-1;
                RNGN5=-1;
                RNGN6=-1;
                RNGN7=-1;
                RNGN8=-1;
                RNGN9=-1;
            }
        }

        public static int PredictedSpawnHeight = -1;
      
        public static int PredictedSpawnX = -1;

        public static int JungleMainEntranceX = -1;
        public static int JungleMainEntranceYapprox = -1;

        public static int DesertTopOfSurfaceCenterX = -1;
        public static int DesertTopOfSurfaceCenterYapprox = -1;

        public static int SnowTopOfSurfaceCenterX = -1;
        public static int SnowTopOfSurfaceCenterY = -1;


        public static int JungleBeachHeight = -1;
        public static int JungleBeachX = -1;
        public static int DungeonBeachHeight = -1;
        public static int DungeonBeachX = -1;


    }


}