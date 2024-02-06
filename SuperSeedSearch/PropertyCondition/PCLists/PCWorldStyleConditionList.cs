using System.Collections.Generic;
using SuperSeedSearch.ConstantEnum;


namespace SuperSeedSearch.PropertyCondition.PCStyle
{
    public static class PCWorldStyleConditionList
    {
        public static List<Condition> pcWorldStyleConditionList = null;

        static PCWorldStyleConditionList()
        {   
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.Dungeon, ConstantEnum.WorldGenPass.PreWorldGen, new List<string>{ConstantEnum.ConstraintNames.DungeonColor,ConstantEnum.ConstraintNames.DungeonSide});         
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.DesertMain, ConstantEnum.WorldGenPass.FullDesert, new List<string>{ConstantEnum.ConstraintNames.DesertStyleGuess, ConstraintNames.ExtensionDesertOasis, ConstraintNames.ExtensionDesertPotentialPyramid, ConstraintNames.ExtensionDesertlPyramid});         
            
            
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.MoonType, ConstantEnum.WorldGenPass.Reset, new List<string>{ConstantEnum.ConstraintNames.MoonType} );            
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.Ores, ConstantEnum.WorldGenPass.Reset, new List<string>{ConstraintNames.OreCopperOrTin,ConstraintNames.OreIronOrLead,ConstraintNames.OreSilverOrTungsten,ConstraintNames.OreGoldOrPlatinum} );
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.UndergroundCavernLayer, ConstantEnum.WorldGenPass.Terrain, new List<string>{ConstraintNames.UndergroundLayerSize, ConstraintNames.UndergroundLayerTilesFromTopWorld, ConstraintNames.UndergroundLayerTilesToSkyNPCSpawn, ConstraintNames.CavernLayerSize, ConstraintNames.CavernLayerTilesFromTopWorld, ConstraintNames.LavaLineDepth, ConstraintNames.WorldSurface2BuriedChestsStart });
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.HardmodeHallowSide, ConstantEnum.WorldGenPass.PreWorldGen, new List<string>{ConstantEnum.ConstraintNames.HardmodeHallowSide, ConstantEnum.ConstraintNames.HardmodeHallowAtJungleSnow} );            
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.Moss, ConstantEnum.WorldGenPass.PreWorldGen, new List<string>{ConstantEnum.ConstraintNames.GlowingMossType});         
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.JungleShrine, ConstantEnum.WorldGenPass.PreWorldGen, new List<string>{ConstantEnum.ConstraintNames.JungleShrineType}); 
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.OceanCaveDungeon, ConstantEnum.WorldGenPass.PreWorldGen, new List<string>{ConstantEnum.ConstraintNames.OceanCaveDungeon}); 
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.SalamanderGiantShellyCrawdad, ConstantEnum.WorldGenPass.PreWorldGen, new List<string>{ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_Salamander, ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_GiantShelly, ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_Crawdad, ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_Extinct, ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_UniqueCount  }); 

            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.Backgrounds, ConstantEnum.WorldGenPass.Reset, new List<string>{ConstantEnum.ConstraintNames.BackgroundStyle, ConstantEnum.ConstraintNames.BackgroundStyleForest1, ConstantEnum.ConstraintNames.BackgroundStyleForest2, ConstantEnum.ConstraintNames.BackgroundStyleForest3, ConstantEnum.ConstraintNames.BackgroundStyleForest4, ConstantEnum.ConstraintNames.BackgroundStyleCavern1, ConstantEnum.ConstraintNames.BackgroundStyleCavern2, ConstantEnum.ConstraintNames.BackgroundStyleCavern3, ConstantEnum.ConstraintNames.BackgroundStyleCavern4, ConstantEnum.ConstraintNames.BackgroundStyleForestChange1, ConstantEnum.ConstraintNames.BackgroundStyleForestChange2, ConstantEnum.ConstraintNames.BackgroundStyleForestChange3, ConstantEnum.ConstraintNames.BackgroundStyleCavernChange1, ConstantEnum.ConstraintNames.BackgroundStyleCavernChange2, ConstantEnum.ConstraintNames.BackgroundStyleCavernChange3}); 
            AddToList(ConstantEnum.PCWorldStyleTypePropCondTypes.Trees, ConstantEnum.WorldGenPass.Reset, new List<string>{ConstantEnum.ConstraintNames.TreeStyle, ConstantEnum.ConstraintNames.TreeStyle1, ConstantEnum.ConstraintNames.TreeStyle2, ConstantEnum.ConstraintNames.TreeStyle3, ConstantEnum.ConstraintNames.TreeStyle4 });    
        }
     
        static void AddToList(string name, ConstantEnum.WorldGenPass FirstEntryPoint, List<string> possibleContraints)
        {
            if (pcWorldStyleConditionList == null) pcWorldStyleConditionList = new List<Condition>();
            PropertyElement propel = new PropertyElement { CondType = ConstantEnum.PropertyType.Unique };

            List<Constraint> condelli = new List<Constraint>();

            foreach (Constraint condel in BasicConstraintList.pcWorldStyleConstraintList)
            {
                if(!possibleContraints.Contains(condel.name)) continue;

                Constraint condelNew = new Constraint(condel);

                if (FirstEntryPoint > condel.delayToworldGenPass)
                    condelNew.delayToworldGenPass = FirstEntryPoint;
                else
                    condelNew.delayToworldGenPass = condel.delayToworldGenPass;

                condelli.Add(condelNew);
            }


            Condition cond = new Condition(ConditionType.WorldStyleTypeProp, name, propel,condelli){isPersistent=true};
            //cond.firstEntryPoint = FirstEntryPoint;            


            pcWorldStyleConditionList.Add(cond);
        }


    }
}