using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace SuperSeedSearch.ConstantEnum
{
    public static class ConditionElementValueList
    {

        public enum KeyWords
        {
            [Display(Name = "default")]
            defaultValue,
            distance,
            [Display(Name = ConstantEnum.ConstraintNames.DistanceToSpawndEuclidParam1)]
            scaleX,
            depth,
            height,
            seed,
            [Display(Name = ConstantEnum.ConstraintNames.ContainsItem)]
            ContainsItem,            
            [Display(Name = ConstantEnum.ConstraintNames.ContainsItemParam1)]
            MinStackSize,
            [Display(Name = ConstantEnum.ConstraintNames.ContainsModifier)]
            ContainsModifier,
            [Display(Name = ConstantEnum.ConstraintNames.ContainsItemID)]
            ContainsItemID,            
            [Display(Name = ConstantEnum.ConstraintNames.ContainsItemIDParam1)]
            MinStackSizeID,
            [Display(Name = ConstantEnum.ConstraintNames.IsLocked)]
            Islocked,
            [Display(Name = ConstantEnum.ConstraintNames.ChestType)]
            ChestType,  
            [Display(Name = ConstantEnum.ConstraintNames.RNGNG )]
            RandomNumber,
            [Display(Name = ConstantEnum.ConstraintNames.MoonType )]
            MoonType,
            [Display(Name = ConstantEnum.ConstraintNames.DungeonColor )]
            DungeonColor,
            [Display(Name = ConstantEnum.ConstraintNames.DungeonSide )]
            DungeonSide,
            [Display(Name = ConstantEnum.ConstraintNames.DesertStyleGuess )]
            DesertStyleGuess,
            [Display(Name = ConstantEnum.ConstraintNames.HardmodeHallowSide )]
            HallowBiomeSide,
            [Display(Name = ConstantEnum.ConstraintNames.HardmodeHallowAtJungleSnow )]
            HallowAtJungleSnow,
            [Display(Name = ConstantEnum.ConstraintNames.OreCopperOrTin )]
            OreCopperOrTin,
            [Display(Name = ConstantEnum.ConstraintNames.OreIronOrLead )]
            OreIronOrLead,
            [Display(Name = ConstantEnum.ConstraintNames.OreSilverOrTungsten )]
            OreSilverOrTungsten,
            [Display(Name = ConstantEnum.ConstraintNames.OreGoldOrPlatinum )]
            OreGoldOrPlatinum,
            [Display(Name = ConstantEnum.ConstraintNames.StatueType)]
            StatueType,  
            [Display(Name = ConstantEnum.ConstraintNames.ObjectType)]
            ObjectType, 
            pathlength,
            [Display(Name = ConstantEnum.ConstraintNames.TilesOnDungeonSide)]
            TilesOnDungeonSide,
            [Display(Name = ConstantEnum.ConstraintNames.TilesOnTempleSide)]
            TilesOnTempleSide,
            StopSearchAfterTestingSeeds,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesNearby)]
            HasTilesNearby,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesNearbyParam1)]
            radius,

            [Display(Name = ConstantEnum.ConstraintNames.HasTilesAbove)]
            HasTilesAbove,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesAboveParam1)]
            dist,
            
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesBelow)]
            HasTilesBelow,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesAboveBelow)]
            HasTilesAboveBelow,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesLeft)]
            HasTilesLeft,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesRight)]
            HasTilesRight,
            [Display(Name = ConstantEnum.ConstraintNames.HasTilesLeftRight)]
            HasTilesLeftRight,

            
            [Display(Name = ConstantEnum.ConstraintNames.HasBGWallNearby)]
            HasBGWallNearby,
            [Display(Name = ConstantEnum.ConstraintNames.HasBGWallDifferentNearby)]
            HasBGWallDifferentNearby,    
            [Display(Name = ConstantEnum.ConstraintNames.GlowingMossType )]
            GlowingMossType,
            [Display(Name = ConstantEnum.ConstraintNames.JungleShrineType )]
            JungleShrineType,
            [Display(Name = ConstantEnum.ConstraintNames.OceanCaveDungeon )]
            OceanCaveDungeon,


            [Display(Name = ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_Salamander )]
            SalamanderGiantShellyCrawdad_Salamander,
            

            [Display(Name = ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_GiantShelly )]
            SalamanderGiantShellyCrawdad_GiantShelly,
            

            [Display(Name = ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_Crawdad )]
            SalamanderGiantShellyCrawdad_Crawdad,

            [Display(Name = ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_Extinct )]
            SalamanderGiantShellyCrawdad_Extinct,
            [Display(Name = ConstantEnum.ConstraintNames.SalamanderGiantShellyCrawdad_UniqueCount )]
            SalamanderGiantShellyCrawdad_UniqueCount,


            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyle )]
            BackgroundStyle,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleForest1 )]
            BackgroundStyleForest1,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleForest2 )]
            BackgroundStyleForest2,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleForest3 )]
            BackgroundStyleForest3,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleForest4 )]
            BackgroundStyleForest4,

            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleCavern1 )]
            BackgroundStyleCavern1,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleCavern2 )]
            BackgroundStyleCavern2,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleCavern3 )]
            BackgroundStyleCavern3,
            [Display(Name = ConstantEnum.ConstraintNames.BackgroundStyleCavern4 )]
            BackgroundStyleCavern4,

            [Display(Name = ConstantEnum.ConstraintNames.TreeStyle )]
            TreeStyle,
            [Display(Name = ConstantEnum.ConstraintNames.TreeStyle1 )]
            TreeStyle1,
            [Display(Name = ConstantEnum.ConstraintNames.TreeStyle2 )]
            TreeStyle2,
            [Display(Name = ConstantEnum.ConstraintNames.TreeStyle3 )]
            TreeStyle3,
            [Display(Name = ConstantEnum.ConstraintNames.TreeStyle4 )]
            TreeStyle4,




            [Display(Name = ConstantEnum.ConstraintNames.WestLeftOf )]
            WestLeftOf,
            [Display(Name = ConstantEnum.ConstraintNames.EastRigthOf )]
            EastRigthOf,
            [Display(Name = ConstantEnum.ConstraintNames.XCoordinate )]
            XCoordinate,
            [Display(Name = ConstantEnum.ConstraintNames.YCoordinate )]
            YCoordinate,
                    
            [Display(Name = ConstantEnum.ConstraintNames.RNGNideltaParam1 )]
            RNGiScaleMin,
            [Display(Name = ConstantEnum.ConstraintNames.RNGNideltaParam2 )]
            RNGiScaleMax,
            [Display(Name = ConstantEnum.ConstraintNames.RNGNideltaParam3 )]
            RNGiDelta,
            [Display(Name = ConstantEnum.ConstraintNames.RNGNiParameters )]
            RNGiFind,
            [Display(Name = ConstantEnum.ConstraintNames.RNGNiParametersParam1 )]
            RNGiStart,
            [Display(Name = ConstantEnum.ConstraintNames.RNGNiParametersParam2 )]
            RNGiStepSize,
            [Display(Name = ConstantEnum.ConstraintNames.RNGNiParametersParam3 )]
            RNGiMaxIndex,


            [Display(Name = ConstantEnum.ConstraintNames.ExtensionDesertPotentialPyramid )]
            ExtensionDesertPotentialPyramid,
            [Display(Name = ConstantEnum.ConstraintNames.ExtensionDesertlPyramid )]
            ExtensionDesertlPyramid,
            [Display(Name = ConstantEnum.ConstraintNames.ExtensionDesertOasis )]
            ExtensionDesertOasis,

        }

        public static readonly List<string> TrueFalseList = Helpers.EnumHelper<ConstantEnum.TrueFalse>.GetAllEnumVariablesAsString();

        const int roundTo = 5;

        public static List<string> generateList(double from, double to, double stepSize = 1, Func<double, double> finalFun = null)
        {
            if (finalFun == null)
                finalFun = (a) => a;
            List<string> list = new List<string>();
            for (double i = from; i <= to; i += stepSize)
            {
                list.Add($"{Math.Round(  finalFun(i), roundTo) }");
            }
            return list;
        }

        public static List<string> AddInverseElements(List<string> list, double offset = 0)
        {
            List<string> listf = new List<string>();
            list.ForEach(s =>
            {
                double val = Helpers.BasicFunctions.ParseAnyDoubleNumber(s);
                if (val != 0 && val != 1)
                    listf.Add($"{Math.Round(1.0 / val + offset, roundTo)}");
            });
            if (Helpers.BasicFunctions.ParseAnyDoubleNumber(listf[0]) < Helpers.BasicFunctions.ParseAnyDoubleNumber(list[0]))
            {
                listf.Reverse();
                listf.AddRange(list);
            }
            else
            {
                list.Reverse();
                listf.AddRange(list);
                listf.Reverse();
            }

            return listf;
        }

        public static List<string> defaultDistFunX = generateList(0, 100,10).Concat(generateList(125, 250,25).Concat(generateList(300, 500,50).Concat(generateList(600, 1000,100).Concat(generateList(1250, 4000,250) ) ) ) ).ToList() ;
        public static List<string> defaultDistFunY = generateList(0, 100,10).Concat(generateList(125, 250,25).Concat(generateList(300, 500,50).Concat(generateList(600, 1000,100).Concat(generateList(1250, 2000,250) ) ) ) ).ToList() ;
        
        public static List<string> XCoordinate = generateList(0, 8400, 50);
        public static List<string> YCoordinate = generateList(0, 2400, 25);
        //public static List<string> distFunXPlusMinus = (defaultDistFunX.GetRange(1,defaultDistFunX.Count-1).Select(v => "-"+v).Reverse()).Concat( defaultDistFunX  ).ToList() ;

        public static readonly Dictionary<KeyWords, List<string>> lookUpValueList = new Dictionary<KeyWords, List<string>>()
        {
            { KeyWords.defaultValue, generateList(0, 100) },            
            { KeyWords.distance, defaultDistFunX },
            { KeyWords.scaleX, AddInverseElements(generateList(0.1, 1,0.1)) },
            { KeyWords.depth, defaultDistFunY },
            { KeyWords.height, defaultDistFunY },
            { KeyWords.seed, generateList(0, 214,1,(v) => v*1e7) },
            { KeyWords.ContainsItem, PropertyCondition.ContainsItemList.pcContainsItemList },              
            { KeyWords.ContainsItemID, new List<string>{"8","5395"} },                        
            { KeyWords.ContainsModifier, PropertyCondition.ContainsItemList.pcContainsModifierList },  
            { KeyWords.MinStackSize, generateList(0, 100) },
            { KeyWords.MinStackSizeID, generateList(0, 100) },
            { KeyWords.Islocked, TrueFalseList },            
            { KeyWords.ChestType, ChestID2ChestType.ChestTypeList },
            { KeyWords.StatueType, StatueTypes.StatueTypeDict },
            { KeyWords.ObjectType, ObjectType.ObjectTypeList },
            { KeyWords.RandomNumber, generateList(1, 15,1, (v)=>1-1.0/v ).Concat(generateList(1, 15,1, (v)=>1.0/v ).Concat( new List<string>{"0.0025","0.9975","0.455654","0.470898","0.477678","0.544643","0.529297","0.522322" } ) ).ToList()  },
            { KeyWords.MoonType, PropertyCondition.WorldStyles.pcMoonType },
            { KeyWords.DungeonColor, PropertyCondition.WorldStyles.pcDungeonColor },
            { KeyWords.DungeonSide, PropertyCondition.WorldStyles.pcLeftRightSide },
            { KeyWords.DesertStyleGuess, PropertyCondition.WorldStyles.pcDesertStyles },
            { KeyWords.HallowBiomeSide, PropertyCondition.WorldStyles.pcLeftRightSide },
            { KeyWords.HallowAtJungleSnow, PropertyCondition.WorldStyles.pcJungelDungeonSide },
            { KeyWords.OreCopperOrTin, PropertyCondition.WorldStyles.pcOreCopperOrTin },
            { KeyWords.OreIronOrLead, PropertyCondition.WorldStyles.pcOreIronOrLead },
            { KeyWords.OreSilverOrTungsten, PropertyCondition.WorldStyles.pcOreSilverOrTungsten },
            { KeyWords.OreGoldOrPlatinum, PropertyCondition.WorldStyles.pcOreGoldOrPlatinum },            
            { KeyWords.pathlength, defaultDistFunX },
            { KeyWords.TilesOnDungeonSide, defaultDistFunX },
            { KeyWords.TilesOnTempleSide, defaultDistFunX },
            { KeyWords.StopSearchAfterTestingSeeds, generateList(1, 20).Concat(generateList(30, 100,10).Concat(generateList(150, 500,50).Concat(generateList(600, 1000,100).Concat(generateList(2000, 9000,1000).Concat(generateList(4, 9,1, (c) => Math.Pow(10,c)).Concat(new List<string>{"2147483648"})  ))))).ToList()  },
            { KeyWords.HasTilesNearby, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.HasTilesAbove, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.HasTilesBelow, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.HasTilesAboveBelow, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.HasTilesLeft, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.HasTilesRight, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.HasTilesLeftRight, PropertyCondition.HasTilesNearbyList.ContentList},
            { KeyWords.radius,  generateList(0, 10)  },
            { KeyWords.dist,  generateList(0, 10).Concat(generateList(0, 100, 5)).ToList()  },
            { KeyWords.HasBGWallNearby, PropertyCondition.HasBGWallNearbyList.ContentList},
            { KeyWords.HasBGWallDifferentNearby, PropertyCondition.HasBGWallNearbyList.ContentList},           
            { KeyWords.GlowingMossType, PropertyCondition.WorldStyles.pcGlowingMossType },       
            { KeyWords.JungleShrineType, PropertyCondition.WorldStyles.pcJungleShrineType },  
            { KeyWords.OceanCaveDungeon, PropertyCondition.WorldStyles.pcOceanCaveDungeon },  

            { KeyWords.SalamanderGiantShellyCrawdad_Salamander, PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Salamander },  
            
            
            { KeyWords.SalamanderGiantShellyCrawdad_Crawdad, PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Crawdad },  
            
            
            { KeyWords.SalamanderGiantShellyCrawdad_GiantShelly, PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_GiantShelly },  
            


            { KeyWords.SalamanderGiantShellyCrawdad_Extinct,  PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Extinct },  
            
            { KeyWords.SalamanderGiantShellyCrawdad_UniqueCount,  generateList(2, 5) },  

            { KeyWords.TreeStyle,  PropertyCondition.WorldStyles.pcTreeStyle  },  
            { KeyWords.TreeStyle1,  PropertyCondition.WorldStyles.pcTreeStyle  },  
            { KeyWords.TreeStyle2,  PropertyCondition.WorldStyles.pcTreeStyle  },  
            { KeyWords.TreeStyle3,  PropertyCondition.WorldStyles.pcTreeStyle  },  
            { KeyWords.TreeStyle4,  PropertyCondition.WorldStyles.pcTreeStyle  },  

            { KeyWords.BackgroundStyle,  PropertyCondition.WorldStyles.pcBackgroundStyle  },  
            { KeyWords.BackgroundStyleForest1,  PropertyCondition.WorldStyles.pcBackgroundStyleForest  },  
            { KeyWords.BackgroundStyleForest2,  PropertyCondition.WorldStyles.pcBackgroundStyleForest  },  
            { KeyWords.BackgroundStyleForest3,  PropertyCondition.WorldStyles.pcBackgroundStyleForest  },  
            { KeyWords.BackgroundStyleForest4,  PropertyCondition.WorldStyles.pcBackgroundStyleForest  },  

            { KeyWords.BackgroundStyleCavern1,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern  },  
            { KeyWords.BackgroundStyleCavern2,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern  },  
            { KeyWords.BackgroundStyleCavern3,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern  },  
            { KeyWords.BackgroundStyleCavern4,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern  },  
            


            { KeyWords.EastRigthOf, defaultDistFunX },       
            { KeyWords.WestLeftOf, defaultDistFunX },       
            { KeyWords.XCoordinate, XCoordinate },       
            { KeyWords.YCoordinate, YCoordinate },   
            { KeyWords.RNGiScaleMin, new List<string>{"0","50","100", "500"} },   
            { KeyWords.RNGiScaleMax,  new List<string>{"1","100","1200","1800","2400","4200","6400","8400"} },   
            { KeyWords.RNGiDelta, generateList(-10, 10)  },                           
            { KeyWords.RNGiFind, generateList(0, 100) },   
            { KeyWords.RNGiStart, generateList(0, 100) },   
            { KeyWords.RNGiStepSize, generateList(0, 10) },   
            { KeyWords.RNGiMaxIndex, new List<string>{"10","100","250"}.Concat(generateList(500,20000,500)).ToList() },  

            { KeyWords.ExtensionDesertPotentialPyramid, generateList(0, 10) }, 
            { KeyWords.ExtensionDesertlPyramid, generateList(0, 10) }, 
            { KeyWords.ExtensionDesertOasis, generateList(0, 10) },             

        };

        public static readonly Dictionary<KeyWords, string> lookUpDefaultValueList = new Dictionary<KeyWords, string>()
        {
            { KeyWords.defaultValue, "" },
            { KeyWords.distance, "0" },
            { KeyWords.scaleX, "1" },
            { KeyWords.ContainsItem, PropertyCondition.ContainsItemList.pcContainsItemList[0] },       
            { KeyWords.ContainsModifier, PropertyCondition.ContainsItemList.pcContainsModifierList[0] },       
            { KeyWords.MinStackSize, "1" },
            { KeyWords.MinStackSizeID, "1" },
            { KeyWords.Islocked, TrueFalseList[1] },
            { KeyWords.RandomNumber, "0" },
            { KeyWords.MoonType, PropertyCondition.WorldStyles.pcMoonType[0] },
            { KeyWords.DungeonColor, PropertyCondition.WorldStyles.pcDungeonColor[0] },
            { KeyWords.DungeonSide, PropertyCondition.WorldStyles.pcLeftRightSide[1] },      
            { KeyWords.StatueType, Helpers.EnumHelper<StatueType>.GetNameOFEnumVariable(StatueType.Other) },      
            { KeyWords.ObjectType, Helpers.EnumHelper<ObjectTypeEnum>.GetNameOFEnumVariable(ObjectTypeEnum.Other) },      
            { KeyWords.radius, "1" },            
            { KeyWords.dist, "1" }, 
            { KeyWords.GlowingMossType, PropertyCondition.WorldStyles.pcGlowingMossType[3] },
            { KeyWords.JungleShrineType, PropertyCondition.WorldStyles.pcJungleShrineType[4] },
            { KeyWords.OceanCaveDungeon, PropertyCondition.WorldStyles.pcOceanCaveDungeon[0] },
            { KeyWords.DesertStyleGuess, PropertyCondition.WorldStyles.pcDesertStyles[0] },

            { KeyWords.SalamanderGiantShellyCrawdad_Salamander, PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Salamander[0] },  
                       
            { KeyWords.SalamanderGiantShellyCrawdad_GiantShelly, PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_GiantShelly[0] },  
                        
            { KeyWords.SalamanderGiantShellyCrawdad_Crawdad, PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Crawdad[0] },  
            
            { KeyWords.SalamanderGiantShellyCrawdad_Extinct,  PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Extinct[0] },  
            
            { KeyWords.SalamanderGiantShellyCrawdad_UniqueCount, "5" },  

            { KeyWords.TreeStyle,  PropertyCondition.WorldStyles.pcTreeStyle[0]  },  
            { KeyWords.TreeStyle1,  PropertyCondition.WorldStyles.pcTreeStyle[0]  },  
            { KeyWords.TreeStyle2,  PropertyCondition.WorldStyles.pcTreeStyle[0]  },  
            { KeyWords.TreeStyle3,  PropertyCondition.WorldStyles.pcTreeStyle[0]  },  
            { KeyWords.TreeStyle4,  PropertyCondition.WorldStyles.pcTreeStyle[0]  },  

            { KeyWords.BackgroundStyle,  PropertyCondition.WorldStyles.pcBackgroundStyle[0]  },  
            { KeyWords.BackgroundStyleForest1,  PropertyCondition.WorldStyles.pcBackgroundStyleForest[0]  },  
            { KeyWords.BackgroundStyleForest2,  PropertyCondition.WorldStyles.pcBackgroundStyleForest[0]  },  
            { KeyWords.BackgroundStyleForest3,  PropertyCondition.WorldStyles.pcBackgroundStyleForest[0]  },  
            { KeyWords.BackgroundStyleForest4,  PropertyCondition.WorldStyles.pcBackgroundStyleForest[0]  },  

            { KeyWords.BackgroundStyleCavern1,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern[0]  },  
            { KeyWords.BackgroundStyleCavern2,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern[0]  },  
            { KeyWords.BackgroundStyleCavern3,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern[0]  },  
            { KeyWords.BackgroundStyleCavern4,  PropertyCondition.WorldStyles.pcBackgroundStyleCavern[0]  },  
            

            { KeyWords.RNGiScaleMin, "0" }, 
            { KeyWords.RNGiScaleMax, "1" }, 
            { KeyWords.RNGiDelta, "0" }, 
            { KeyWords.RNGiFind, "1" }, 
            { KeyWords.RNGiStart, "0" }, 
            { KeyWords.RNGiStepSize, "1" }, 
            { KeyWords.RNGiMaxIndex, "100" }, 


            { KeyWords.ExtensionDesertPotentialPyramid, "1" }, 
            { KeyWords.ExtensionDesertlPyramid, "1" }, 
            { KeyWords.ExtensionDesertOasis, "1" },     
            
        };


        public static List<string> FindValueValueList4ValueName(string name)
        {            
            KeyWords? res = Helpers.EnumHelper<KeyWords>.Try2FindValue4Name(name);            
            if (res == null)
                return lookUpValueList[KeyWords.defaultValue];            
            return lookUpValueList[(KeyWords)res];
        }

        public static string FindDefaultValue4ValueName(string name)
        {
            KeyWords? res = Helpers.EnumHelper<KeyWords>.Try2FindValue4Name(name);
            if (res == null)
                return lookUpDefaultValueList[KeyWords.defaultValue];

            return lookUpDefaultValueList.ContainsKey((KeyWords)res)?lookUpDefaultValueList[(KeyWords)res]:lookUpValueList[(KeyWords)res][0];
        }
    }
}