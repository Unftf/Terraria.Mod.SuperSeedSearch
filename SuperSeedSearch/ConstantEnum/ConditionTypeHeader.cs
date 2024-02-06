using System.Collections.Generic;


namespace SuperSeedSearch.ConstantEnum
{
    public static class ConditionTypeHeader
    {
        public static readonly Dictionary<ConditionType, string> conditionTypeHeader = new Dictionary<ConditionType, string>{
             {ConditionType.Unknown , ConditionTypeNames.Unknown},
            {ConditionType.Chest  , ConditionTypeNames.Chest},
            {ConditionType.Tile   , ConditionTypeNames.Tile},
            {ConditionType.RNGnumber   , ConditionTypeNames.RNGnumber},            
            {ConditionType.WorldStyleTypeProp , ConditionTypeNames.WorlStyleTypeProp },
            {ConditionType.Object , ConditionTypeNames.Object },
            {ConditionType.PointOfInterest , ConditionTypeNames.PointOfInterest },
            {ConditionType.Biome , ConditionTypeNames.Biome },


            };
    }
}