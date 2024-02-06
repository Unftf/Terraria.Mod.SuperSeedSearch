using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using SuperSeedSearch.Storage;
using System.Collections.Generic;
//using ReLogic.Reflection; // IdDictionary Search = IdDictionary.Create<WallID, ushort>();
using System;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Helpers;
using System.IO;
using Terraria.WorldBuilding;


namespace SuperSeedSearch.WorldGenMod
{
    public class Analyser
    {        

        ConditionScheduler conditionSched = null;
        public Analyser(ConditionScheduler conditionSched){
            this.conditionSched = conditionSched;            
        }

        List<  Tuple<string, int, int, int>  > ChestItemList = null;//Todo own class

        public void CheckOutPropElm(ref PropertyCondition.PropertyElement prop, ConstantEnum.WorldGenPass lastPassNameID)
        {       
            //TODO5 optimize     
            foreach (var item in conditionSched.ListAllCond)
            {                    
                if (item.firstEntryPoint > lastPassNameID) continue;
                bool isValid = false;
                if (item.CheckIfPropertyElementIsValid(prop)) {isValid = item.EvaluateConstraintList(prop, lastPassNameID); }//lastWorldGenPassID

                //todo export?
                if(isValid && lastPassNameID == ConstantEnum.WorldGenPass.FinalCleanup){                    
                    if(prop.CondType == ConstantEnum.PropertyType.Chest){                    
                        bool HasItem = false;
                        ConstantEnum.ChestType ChestType = ChestType.Other;
                        Tuple<string, int, int, int> chetyp = null;
                        Tuple<string, int, int, int> modtyp = null;
                        foreach (PropertyCondition.Constraint ce in item.constrElemChosenList)
                        {
                            if (ce.name.Equals( ConstantEnum.ConstraintNames.ContainsItem ))
                            {
                                HasItem = true;
                                int itemType = (int)ce.targetValue.valueDouble;    
                                ChestItemList.Add( new Tuple<string, int, int, int>(ce.targetValue.valueString, (int)ce.targetValue.valueDouble ,prop.posX,prop.posY) );

                            }              
                            else if( ce.name.Equals( ConstantEnum.ConstraintNames.ChestType  ) ) {
                                ChestType = ConstantEnum.ChestID2ChestType.id2Type(prop.chestIndex);
                                if(ChestType == ChestType.DeadMansChest)
                                    chetyp = new Tuple<string, int, int, int>(ce.targetValue.valueString, ItemID.DeadMansChest ,prop.posX,prop.posY); 
                            }
                            else if( ce.name.Equals( ConstantEnum.ConstraintNames.ContainsModifier  ) && !ce.targetValue.Equals(ConstantEnum.ConstantsStrings.None) ) {
                                    modtyp = new Tuple<string, int, int, int>(ce.targetValue.valueString, Main.chest[prop.chestIndex].item[0].type ,prop.posX,prop.posY); 
                            }
                        }
                        if(ChestType>=0 && chetyp != null){
                            ChestItemList.Add( chetyp );
                        }
                        if(!HasItem && modtyp != null){
                            ChestItemList.Add( modtyp );
                        }
                    }
                    if(prop.CondType == ConstantEnum.PropertyType.Tile){
                        if( ObjectType.dictObjectStringToItemType.ContainsKey(item.name) ){
                                
                            var orign = Helpers.Helpers.GetObjectOrigin(prop.posX,prop.posY);                        
                            ChestItemList.Add(new Tuple<string, int, int, int>(item.name, ObjectType.dictObjectStringToItemType[item.name], orign==null?prop.posX:orign.Item1,orign==null?prop.posY:orign.Item2) );
                        } 
                        else if( item.name.Equals( PCObjectCondTypes.Statue) ){
                            bool hasType = false;
                            foreach (PropertyCondition.Constraint ce in item.constrElemChosenList){
                                if (ce.name.Equals( ConstantEnum.ConstraintNames.StatueType )){
                                    string statName = ce.targetValue.valueString;
                                    var statueType = EnumHelper<ConstantEnum.StatueType>.GetValueFromName(statName);
                                    if( ConstantEnum.StatueTypes.dictStatueTypeToItemId.ContainsKey( statueType   ) ){
                                        hasType = true;
                                        int id = ConstantEnum.StatueTypes.dictStatueTypeToItemId[statueType];
                                        var orign = Helpers.Helpers.GetObjectOrigin(prop.posX,prop.posY);
                                        ChestItemList.Add( new Tuple<string, int, int, int>(ce.targetValue.valueString, id ,orign.Item1,orign.Item2) );
                                    }

                                }
                            }
                            if(!hasType){
                               /*
                                StatueType statueType = StatueTypes.tile2Type(prop.posX,prop.posY);
                                if(statueType!=StatueType.Other){
                                    int id = ConstantEnum.StatueTypes.dictStatueTypeToItemId[statueType];
                                    var orign = Helpers.Helpers.GetObjectOrigin(prop.posX,prop.posY);
                                    ChestItemList.Add( new Tuple<string, int, int, int>(""+statueType, id ,orign.Item1,orign.Item2) );
                                }*/
                            }


                        }
                    }
                }
            }
           
        }

        public void SetRNGValues(int currentSeed){
            //UnifiedRandom rand = new UnifiedRandom(currentSeed);
            //WorldInfo.RNGNumbers.RNGN1 = rand.NextDouble();
            //WorldInfo.RNGNumbers.RNGN2 = rand.NextDouble();
            //WorldInfo.RNGNumbers.RNGN3 = rand.NextDouble();

            //direct compute faster
            WorldInfo.RNGNumbers.RNGN1 = ((1121899819L*currentSeed+ 1559595546L)%((1L<<31)-1)) * 4.6566128752457969E-10;//###ToDo proof at start if still true with ne update
            WorldInfo.RNGNumbers.RNGN2 = (( 630111683L*currentSeed+ 1755192844L)%((1L<<31)-1)) * 4.6566128752457969E-10;
            WorldInfo.RNGNumbers.RNGN3 = ((1501065279L*currentSeed+ 1649316166L)%((1L<<31)-1)) * 4.6566128752457969E-10; if(currentSeed==1122996615)WorldInfo.RNGNumbers.RNGN3 = 4.6566128752457969E-10;//sligthly wrong (real = calc +6) for for some seeds, 1122996615 overflow, smallest seed 161844078
            WorldInfo.RNGNumbers.RNGN4 = (( 458365203L*currentSeed+ 1198642031L)%((1L<<31)-1)) * 4.6566128752457969E-10;
            WorldInfo.RNGNumbers.RNGN5 = (( 969558243L*currentSeed+  442452829L)%((1L<<31)-1)) * 4.6566128752457969E-10;
            WorldInfo.RNGNumbers.RNGN6 = ((1876681249L*currentSeed+ 1200195957L)%((1L<<31)-1)) * 4.6566128752457969E-10; if(currentSeed==2059702593)WorldInfo.RNGNumbers.RNGN3 = 0.99999999953433870729874974;//slightly wrong for some numbers: real = calc -2, underflow by 2059702593
            WorldInfo.RNGNumbers.RNGN7 = (( 962194431L*currentSeed+ 1945678308L)%((1L<<31)-1)) * 4.6566128752457969E-10;
            WorldInfo.RNGNumbers.RNGN8 = ((1077359051L*currentSeed+ 949569752L)%((1L<<31)-1)) * 4.6566128752457969E-10;
            WorldInfo.RNGNumbers.RNGN9 = ((265679591L*currentSeed+ 2099272109L)%((1L<<31)-1)) * 4.6566128752457969E-10;
             
             
            WorldInfo.RNGNumbers.predicted = true;
            WorldInfo.RNGNumbers.forSeed = currentSeed;


        }
        
        public void AnalyseSeedRNG(int currentSeed)
        {   
            if(!conditionSched.HasAnyThing4WGPass(ConstantEnum.WorldGenPass.PreWorldGen)) return;

            PropertyCondition.PropertyElement propelm = new PropertyCondition.PropertyElement();
            propelm.CondType = ConstantEnum.PropertyType.RNGnumber;
            propelm.lastWorldGenPass = ConstantEnum.WorldGenPass.PreWorldGen;
            
            CheckOutPropElm(ref propelm, propelm.lastWorldGenPass);
        }
        public void AnalyseUniquePropsBeforeWG()
        {
            if(!conditionSched.HasAnyThing4WGPass(ConstantEnum.WorldGenPass.PreWorldGen)) return;     
            AnalyseUniqueProps(ConstantEnum.WorldGenPass.PreWorldGen);                
        }

        public void AnalyseUniqueProps(ConstantEnum.WorldGenPass lastPassNameID)
        {   
            PropertyCondition.PropertyElement propelm = new PropertyCondition.PropertyElement();
            propelm.CondType = ConstantEnum.PropertyType.Unique;
            propelm.lastWorldGenPass = lastPassNameID;            
            CheckOutPropElm(ref propelm, propelm.lastWorldGenPass);
        }

        public void AnalyseTiles(ConstantEnum.WorldGenPass lastPassNameID)
        {
            Tile t;
            PropertyCondition.PropertyElement propelm = new PropertyCondition.PropertyElement();
            propelm.CondType = ConstantEnum.PropertyType.Tile;
            propelm.lastWorldGenPass = lastPassNameID;//this.lastWorldGenPassID;

            const int border = 10; //TODO lookup exact value
            for (propelm.posX = border; propelm.posX < Main.maxTilesX - border; propelm.posX++)
                for (propelm.posY = border; propelm.posY < Main.maxTilesY - border; propelm.posY++)
                {
                    t = Main.tile[propelm.posX, propelm.posY];
                    propelm.isActive = t.HasTile;
                    propelm.IDTile = (short)t.TileType;
                    propelm.frameX = t.TileFrameX; propelm.frameY = t.TileFrameY;
                    propelm.IDWall = (short)t.WallType;
                    
                    CheckOutPropElm(ref propelm, lastPassNameID);

                }
        }

        public void AnalyseChests(ConstantEnum.WorldGenPass lastPassNameID)
        {
            ChestItemList = null;
            if(lastPassNameID==ConstantEnum.WorldGenPass.FinalCleanup ) ChestItemList = new List<Tuple<string, int, int, int>>();

            Tile t;
            PropertyCondition.PropertyElement propelm = new PropertyCondition.PropertyElement();
            propelm.CondType = ConstantEnum.PropertyType.Chest;
            propelm.lastWorldGenPass = lastPassNameID;//this.lastWorldGenPassID;
            for (int i = 0; i < Main.maxChests; i++)
            {
                if (Main.chest[i] == null || Main.chest[i].item[0] == null || Main.chest[i].item[0].type == ItemID.None) continue;
                
                propelm.chestIndex = (short)i;
                propelm.posX = (short)Main.chest[i].x;
                propelm.posY = (short)Main.chest[i].y;
                propelm.isChestLocked = Chest.IsLocked(propelm.posX, propelm.posY);
                propelm.isActive = true;

                t = Main.tile[propelm.posX, propelm.posY];
                propelm.IDTile = (short)t.TileType;
                propelm.frameX = t.TileFrameX; propelm.frameY = t.TileFrameY;
                propelm.IDWall = (short)t.WallType;
                CheckOutPropElm(ref propelm, lastPassNameID);
            }

            if(ChestItemList!=null) {
                Storage.WorldInfo.SetValue(ConstantEnum.Statistics.FoundMatchingChestItems, ChestItemList );            
            }
            

        }



    }
}