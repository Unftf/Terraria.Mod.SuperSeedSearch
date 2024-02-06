using System.Collections.Generic;
using SuperSeedSearch.UI;
using SuperSeedSearch.PropertyCondition;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Helpers;
using System;

namespace SuperSeedSearch.PropertyCondition
{
    public static class AllTypeList
    {

        public static AllConditionTypeList pcAllcondTypeList = null;



        static AllTypeList()
        {
            List<PropertyCondition.ConditionTypeList> allcond = new List<PropertyCondition.ConditionTypeList>();
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.Tile, PropertyCondition.PCTiles.PCTilesConditionList.pcTilesConditionList,true));
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.Chest, PropertyCondition.PCChests.PCChestsConditionList.pcChestsConditionList,true));
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.RNGnumber, PropertyCondition.PCRandomValue.PCRandomValueConditionList.pcRandomValueList,false));
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.WorldStyleTypeProp, PropertyCondition.PCStyle.PCWorldStyleConditionList.pcWorldStyleConditionList,false));
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.Object, PropertyCondition.PCObject.PCObjectConditionList.pcObjectConditionList,true));
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.PointOfInterest, PropertyCondition.PCPoint.PCPointOfInterestList.pcPointOfInteerestConditionList,false));
            allcond.Add(new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.Biome, PropertyCondition.PCBiome.PCBiomeConditionList.pcBiomeConditionList,true));


            pcAllcondTypeList = new PropertyCondition.AllConditionTypeList(allcond);
        }
        public static Condition FindCondition(ConditionType condType, string condName){            
            foreach(var cl in pcAllcondTypeList.typeConditionList){
                if(cl.conditionType == condType){
                    foreach(var cond in cl.suitableTypeConditions){                        
                        if(cond.name.Equals(condName) ){
                            return new Condition(cond);
                        }
                    }
                }
            }            
            return null;
        }

    }

}