using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using SuperSeedSearch.ConstantEnum;


namespace SuperSeedSearch.PropertyCondition.PCChests
{
    public static class PCChestsConditionList
    {

        public static List<Condition> pcChestsConditionList = null;



        static PCChestsConditionList()
        {
            AddToList(ConstantEnum.PCChestsCondTypes.EveryChest, ConstantEnum.WorldGenPass.MicroBiomes);
            AddToList(ConstantEnum.PCChestsCondTypes.AlmostAnyChest, ConstantEnum.WorldGenPass.FloatingIslandHouses);
            AddToList(ConstantEnum.PCChestsCondTypes.DungeonPyramidLivingChest, ConstantEnum.WorldGenPass.LivingTrees);
        }

        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> ContainsItem = (p, c) =>
        {

            foreach (var item in Main.chest[p.chestIndex].item)
            {
                int type = item.type;
                if (ContainsItemList.MappingIsAlso.ContainsKey((short)type))
                    type = ContainsItemList.MappingIsAlso[(short)type];

                if (type == (int)c.targetValue.valueDouble && item.stack >= c.ParameterValue1.valueDouble)
                {
                    return c.targetValue.GetValueAsString();
                }
            }
            return "not " + c.targetValue.GetValueAsString();
        };

        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> ContainsModifier = (p, c) =>
        {
            int target = Storage.Stats.GetID4Name(typeof(PrefixID) ,c.targetValue.valueString );
            if(target<0) target = 0;

            foreach (var item in Main.chest[p.chestIndex].item)
            {
                int type = item.type;
                if( type == ItemID.None) continue;
                int prefix = item.prefix;

                if ( prefix == target  )
                {
                    return c.targetValue.GetValueAsString();
                }
            }
            return "not " + c.targetValue.GetValueAsString();
        };




        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> IsLocked = (p, c) =>
        {  
            return Chest.IsLocked(p.posX, p.posY)?Helpers.EnumHelper<ConstantEnum.TrueFalse>.GetNameOFEnumVariable(ConstantEnum.TrueFalse.True):Helpers.EnumHelper<ConstantEnum.TrueFalse>.GetNameOFEnumVariable(ConstantEnum.TrueFalse.False);
        };

        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasChestType = (p, c) =>
        {               
            return ChestID2ChestType.id2Type(p.chestIndex).ToString().Equals(c.targetValue.GetValueAsString())?c.targetValue.GetValueAsString():"not " + c.targetValue.GetValueAsString();
        };
        

        static void AddToList(string name, ConstantEnum.WorldGenPass FirstEntryPoint)
        {
            if (pcChestsConditionList == null) pcChestsConditionList = new List<Condition>();
            List<PropertyElement> propel = new List<PropertyElement>();

            propel.Add(new PropertyElement { CondType = ConstantEnum.PropertyType.Chest, isActive = true });


            List<Constraint> condelli = new List<Constraint>();

            foreach (Constraint condel in BasicConstraintList.pcBasicDistanceConstraintList)
            {
                Constraint condelNew = new Constraint(condel);

                if (FirstEntryPoint > condel.delayToworldGenPass)
                    condelNew.delayToworldGenPass = FirstEntryPoint;
                else
                    condelNew.delayToworldGenPass = condel.delayToworldGenPass;

                condelli.Add(condelNew);
            }

            condelli.Add(BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.ContainsItem, ContainsItem, FirstEntryPoint, CustomChestDisplayFun));
            condelli.Add(BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.ContainsItemID, ContainsItem, FirstEntryPoint, CustomChestDisplayFun));
            condelli.Add(BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.ContainsModifier, ContainsModifier, FirstEntryPoint, CustomChestDisplayFun));
            condelli.Add(BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.IsLocked, IsLocked, FirstEntryPoint));            
            condelli.Add(BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.ChestType, HasChestType , FirstEntryPoint));

            pcChestsConditionList.Add(new Condition(ConditionType.Chest, name, propel, condelli));
        }

        private static string CustomChestDisplayFun(Constraint con, bool isFirst)
        {
            string txt = "";
            txt += (!isFirst ? ", " : "Chest ") + con.targetValue.valueString;          
            return txt;
        }

    }

}