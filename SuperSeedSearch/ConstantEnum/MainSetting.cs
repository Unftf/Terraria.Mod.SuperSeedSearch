using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using SuperSeedSearch.Storage;
using System.IO;
using Terraria;
using SuperSeedSearch.ConstantEnum;

namespace SuperSeedSearch.ConstantEnum
{

    public enum MainSettingEnum
    {
        SearchConfigName,
        WorldName,
        WorldSize,
        EvilType,
        Difficulty,
        StartSeed,
        StopSearchAfter,
        StopSearchIfFound,
        NextSeedIsOldPlus,
        StoreWorldMapAsPicture,
        StoreStatsInfo,
        StoreWorldFile,
        SecretSeed,
        SecretSeedModifier,
        SeedReplaceText
    }
    public class MainSetting
    {
        public string name = "";
        public List<string> options = null;

        public PropertyCondition.ValueStringAndOrDouble startingVal = null;
        public bool disableCustomInput = false;
        public bool doNotSortContentList = false;

        public Action<string> doAfterInitAndTextChange = null;

    }
    public enum WorldName : int
    {
        [Display(Name = "Fancy world")]
        fancyWorld = 0,
        [Display(Name = "#S to insert the world seed")]
        exampleSeed ,
        [Display(Name = "#s for a short version of the seed")]
        exampleSeedShort,
        [Display(Name = "#c for a counter")]
        exampleCounter,
        [Display(Name = "#1c or #1s for a leading zero")]
        exampleLeadingZero,
        [Display(Name = "#2s for a two leading zero")]
        exampleLeadingZero2,
        [Display(Name = "#3s for 3 and so on (up to 9)")]        
        exampleLeadingZero4,
        [Display(Name = "#T for the seed in text form")]
        exampleSeedText,   
        [Display(Name = "#z for the world size")]
        exampleWorldSize,  
        [Display(Name = "#e for the world evil type")]
        exampleEvil,  
        [Display(Name = "#d for dungeon side")]
        dungeonSide,  
        [Display(Name = "#s_#e#z")]
        exampleSeedEvilSize,  
        [Display(Name = "#s_#z#e")]
        exampleSeedSizeEvil,  
        [Display(Name = "#s_#z#d#e")]
        exampleSeedSizeDungeonSideEvil,  
        [Display(Name = "#s_#e")]
        exampleSeedEvil,  
        [Display(Name = "#z_#s_#d_#e")]
        exampleSizeSeedDungeonEvil,  
        [Display(Name = "#T")]
        seedText,   
        [Display(Name = "#c")]
        count,
        [Display(Name = "#s")]
        seed,
        [Display(Name = "#S")]
        seedFull,
        [Display(Name = "#Seed")]
        seedOnly,
    }
    public enum WorldSize :int
    {
        [Display(Name = "Small")]
        small = 0,
        [Display(Name = "Medium")]
        medium = 1,
        [Display(Name = "Large")]
        large = 2,
    }
    public enum EvilType:int
    {
        [Display(Name = "Random")]
        random = -1,
        [Display(Name = "Corruption")]
        corruption = 0,
        [Display(Name = "Crimson")]
        crimson = 1,
        [Display(Name = "Do both")]
        both = 2,
        [Display(Name = "Random inverse")]
        randomInverse = 3,
    }
    public enum Difficulty:int
    {
        [Display(Name = "Journey")]
        journey = 3,
        [Display(Name = "Classic")]
        classic = 0,
        [Display(Name = "Expert")]
        expert = 1,
        [Display(Name = "Master")]
        master = 2,        
    }
    public enum StoreAsPicture :int
    {
        [Display(Name = "Picture, add default item icons + queried items")]
        PictureDefault = 0,
        [Display(Name = "Picture, add default item icons")]
        PictureDefaultNoQueried = 1,
        [Display(Name = "Picture, add only rare item icons + queried items")]
        PictureRare = 2,
        [Display(Name = "Picture, add only queried item icons")]
        PictureQueriedOnly = 3,
        [Display(Name = "Picture, without icons")]
        PictureNoIcons = 4,
        [Display(Name = "Off")]
        Off = 5,
        //[Display(Name = "Picture only")]
        //PictureOnly = 2,
    }
   
    public enum StoreStatsInfo :int
    {
        [Display(Name = "On")]
        On = 0,
        [Display(Name = "Off")]
        Off = 1,
        [Display(Name = "Extended stats")]
        Extended = 2,
        [Display(Name = "Too many stats")]
        TooMany = 3,
    }
    public enum StoreWorldFile :int
    {
        [Display(Name = "On")]
        On = 0,
        [Display(Name = "Off")]
        Off = 1,
    }

    public enum SecretSeed:int
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Drunk world 5162020")]
        Drunk,
        [Display(Name = "for the worthy")]
        Worthy,
        [Display(Name = "celebrationmk10 5162021")]
        Celebration,
        [Display(Name = "the constant")]
        Constant,
        [Display(Name = "not the bees!")]
        Bees,
        [Display(Name = "don't dig up")]
        DontDigUp,
        [Display(Name = "no traps")]
        NoTraps,
        [Display(Name = "get fixed Zenith")]
        Zenith,
    }
    public enum SecretSeedModifier:int{
        [Display(Name = "#First that fits")]
        FirtFit = 0,
        [Display(Name = "5162020")]
        drunk,
        [Display(Name = "for the worthy")]
        ftw1,
        [Display(Name = "fortheworthy")]
        ftw2,
        [Display(Name = "celebrationmk10")]
        cele1,
        [Display(Name = "5162011")]
        cele2,
        [Display(Name = "5162021")]
        cele3,
        [Display(Name = "constant")]
        constant1,
        [Display(Name = "the constant")]
        constant2,
        [Display(Name = "theconstant")]
        constant3,
        [Display(Name = "eye4aneye")]
        constant4,
        [Display(Name = "eyeforaneye")]
        constant5,
        [Display(Name = "not the bees!")]
        bees1,
        [Display(Name = "not the bees")]
        bees2,
        [Display(Name = "notthebees")]
        bees3,
        [Display(Name = "don't dig up")]
        dig1,
        [Display(Name = "dont dig up")]
        dig2,
        [Display(Name = "dontdigup")]
        dig3,
        [Display(Name = "no traps")]
        traps1,
        [Display(Name = "notraps")]
        traps2,
        [Display(Name = "get fixed boi")]
        fixed1,
        [Display(Name = "getfixedboi")]
        fixed2,
        [Display(Name = "#Seed")]
        seed,
        [Display(Name = "#Random")]
        random,
    }

    public enum SeedReplaceText:int
    {
        [Display(Name = "#Do not replace")]
        Donot = 0,
        [Display(Name = "Super seed ")]
        SuperSeed,
        [Display(Name = "#c to include a counter")]
        exampleCounter,
        [Display(Name = "#s to start counting from the number seed")]
        exampleSeed,
        [Display(Name = "#1c or #1s for a leading zero")]
        exampleLeadingZero,
        [Display(Name = "#2s for a two leading zero")]
        exampleLeadingZero2,
        [Display(Name = "#3s for 3 and so on (up to 9)")]        
        exampleLeadingZero4,
        [Display(Name = "#interprete seed as WorldID and convert it back to a seed")]      
        WorldID2Seed,
        [Display(Name = "#seeds.txt in world folder")]      
        SeedFile
    }

    public static class MainSettingDict
    {

        public static Dictionary<MainSettingEnum, MainSetting> mainSetting = new Dictionary<MainSettingEnum, MainSetting>{
                    {MainSettingEnum.SearchConfigName,  new MainSetting{ name="Search config name", options = new List<string>(), doAfterInitAndTextChange = (str) => SetConent(str), startingVal=ConstantsStrings.NewClearCondtions, disableCustomInput=false} },
                    {MainSettingEnum.WorldName,  new MainSetting{ name="World name", options = Helpers.EnumHelper<WorldName>.GetAllEnumVariablesAsString(), startingVal="#Seed", doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.WorldName,str ), disableCustomInput=false, doNotSortContentList =true } },
                    {MainSettingEnum.WorldSize,  new MainSetting{ name="Size", options = Helpers.EnumHelper<WorldSize>.GetAllEnumVariablesAsString() , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.WorldSize,str ), disableCustomInput=true, doNotSortContentList=true } },
                    {MainSettingEnum.EvilType,  new MainSetting{ name="Evil type", options = Helpers.EnumHelper<EvilType>.GetAllEnumVariablesAsString() , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.EvilType,str ), disableCustomInput=true, doNotSortContentList=true } },
                    {MainSettingEnum.Difficulty,  new MainSetting{ name="Difficulty", options =  Helpers.EnumHelper<Difficulty>.GetAllEnumVariablesAsString() , startingVal=Helpers.EnumHelper<Difficulty>.GetNameOFEnumVariable(Difficulty.classic), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.Difficulty,str ), disableCustomInput=true, doNotSortContentList=true } },
                    {MainSettingEnum.StartSeed,  new MainSetting{ name="Starting Seed", options = ((new List<string>{ConstantsStrings.RandomStartingSeed}).Concat((ConstantEnum.ConditionElementValueList.generateList(0,10,1,(a)=>new Random().Next(0,Int32.MaxValue ))).Concat(ConditionElementValueList.lookUpValueList[ConditionElementValueList.KeyWords.seed]).ToList())).ToList()  , doNotSortContentList=true,  startingVal=ConstantsStrings.RandomStartingSeed , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.StartSeed,str ) } },
                    {MainSettingEnum.StopSearchIfFound,  new MainSetting{ name="Stop search after finding", options = ConstantEnum.ConditionElementValueList.generateList(1, 100) ,startingVal=10 , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.StopSearchIfFound,str ) } },
                    {MainSettingEnum.StopSearchAfter,  new MainSetting{ name="Stop search after testing", options = ConditionElementValueList.lookUpValueList[ConditionElementValueList.KeyWords.StopSearchAfterTestingSeeds],startingVal=1000000 , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.StopSearchAfter,str ) } },
                    {MainSettingEnum.NextSeedIsOldPlus,  new MainSetting{ name="Next seed is old seed +", options = ConditionElementValueList.lookUpValueList[ConditionElementValueList.KeyWords.defaultValue],startingVal=1 , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.NextSeedIsOldPlus,str ) } },
                    {MainSettingEnum.StoreWorldMapAsPicture,  new MainSetting{ name="Store world map as picture", options = Helpers.EnumHelper<StoreAsPicture>.GetAllEnumVariablesAsString() ,startingVal=Helpers.EnumHelper<StoreAsPicture>.GetNameOFEnumVariable(StoreAsPicture.Off)  , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.StoreWorldMapAsPicture,str ), disableCustomInput=true, doNotSortContentList=true  } },
                    {MainSettingEnum.StoreStatsInfo,  new MainSetting{ name="Store world stats & info", options = Helpers.EnumHelper<StoreStatsInfo>.GetAllEnumVariablesAsString() ,startingVal=Helpers.EnumHelper<StoreStatsInfo>.GetNameOFEnumVariable(StoreStatsInfo.Off)  , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.StoreStatsInfo,str ), disableCustomInput=true, doNotSortContentList=true  } },
                    {MainSettingEnum.StoreWorldFile,  new MainSetting{ name="Store world file", options = Helpers.EnumHelper<StoreWorldFile>.GetAllEnumVariablesAsString() ,startingVal=Helpers.EnumHelper<StoreWorldFile>.GetNameOFEnumVariable(StoreWorldFile.On)  , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.StoreWorldFile,str ), disableCustomInput=true, doNotSortContentList=true  } },
                    {MainSettingEnum.SecretSeed,  new MainSetting{ name="Secret easter egg seed", options = Helpers.EnumHelper<SecretSeed>.GetAllEnumVariablesAsString() ,startingVal=Helpers.EnumHelper<SecretSeed>.GetNameOFEnumVariable(SecretSeed.None)  , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.SecretSeed,str ), disableCustomInput=true, doNotSortContentList=true  } },
                    {MainSettingEnum.SecretSeedModifier,  new MainSetting{ name="Secret seed RNG modifier", options = Helpers.EnumHelper<SecretSeedModifier>.GetAllEnumVariablesAsString() ,startingVal=Helpers.EnumHelper<SecretSeedModifier>.GetNameOFEnumVariable(SecretSeedModifier.FirtFit)  , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.SecretSeedModifier,str ), disableCustomInput=false, doNotSortContentList=true  } },
                    {MainSettingEnum.SeedReplaceText,  new MainSetting{ name="Replace start seed with text", options = Helpers.EnumHelper<SeedReplaceText>.GetAllEnumVariablesAsString() ,startingVal=Helpers.EnumHelper<SeedReplaceText>.GetNameOFEnumVariable(SeedReplaceText.Donot)  , doAfterInitAndTextChange = (str) => WorldInfo.SetValue(MainSettingEnum.SeedReplaceText,str ), doNotSortContentList=true  } },

        };

        public static void SetConent(string newName){

            if (!System.IO.Directory.Exists(Main.SavePath + ConstantsStrings.modpath))
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(Main.SavePath + ConstantsStrings.modpath);

            FileInfo[] Files = dirInfo.GetFiles("*.txt"); //todo constant
            
            mainSetting[MainSettingEnum.SearchConfigName].options.Clear();
            foreach(FileInfo file in Files )
            {
                String name = file.Name.Substring(("config").Length, file.Name.Length-("config").Length-(".txt").Length);
                if(!name.Equals(ConstantsStrings.DefaultConfigFileName))
                    mainSetting[MainSettingEnum.SearchConfigName].options.Add(name);
            }            
            if(!mainSetting[MainSettingEnum.SearchConfigName].options.Contains(ConstantsStrings.NewClearCondtions))
                mainSetting[MainSettingEnum.SearchConfigName].options.Insert(0,ConstantsStrings.NewClearCondtions);
            if(!mainSetting[MainSettingEnum.SearchConfigName].options.Contains(ConstantsStrings.DefaultConfig))
                mainSetting[MainSettingEnum.SearchConfigName].options.Insert(0,ConstantsStrings.DefaultConfig);

                
            
        }

    }


}






