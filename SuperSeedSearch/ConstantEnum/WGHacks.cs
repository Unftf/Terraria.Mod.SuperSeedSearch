using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Terraria.ID;
using SuperSeedSearch.Storage;
using System.Linq;
using Terraria.ObjectData;
using Terraria;

namespace SuperSeedSearch.ConstantEnum
{

    public enum WGHacksEnum
    {
        WGHacksVisibleInTabBar,
        WGHacksIsActive,
        OverwriteWorldTileSizeX,
        OverwriteWorldTileSizeY,
        SSdrunkIsActive,
        SSworthyIsActive,
        SScelebrIsActive,
        SSDontDigUpIsActive,
        SSNoTrapsIsActive,
        SSconstantIsActive,	   
        SSbeesIsActive,
        SSZenithIsActive,
        SSStarsCaught,
        //FixDoubleTempleIncrease,
        EndWorldGenAfterWGPass,
        StoreWorldImageAfter,
        PartialWorldGenPreStep,
        HardmodeOreTier1,
        HardmodeOreTier2,
        HardmodeOreTier3,
        StartInHardmode
    }
    public class WGHack:MainSetting
    {         

    }

    static class OnOffenable{
        public const string None ="don't change";
        public const string On ="is enabled";
        public const string Off ="is disabled";
        public const string OffOn ="is disabled but enable it after WG";
        public const string OnOff ="is enabled but disable it after WG";
    }   
    public enum SSdrunkIsActive
    {        
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }
    public enum SSworthyIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }
    public enum SScelebrIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }  
    public enum SSconstantIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }    
    public enum SSbeesIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }
    public enum SSZenithIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }
    public enum SSDontDigUpIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }
    public enum SSNoTrapsIsActive
    {
        [Display(Name = OnOffenable.None)]
        none = 0,
        [Display(Name = OnOffenable.Off)]
        off,
        [Display(Name = OnOffenable.OffOn)]
        offOn,
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = OnOffenable.OnOff)]
        onOff
    }

    public enum PartialWorldGenPreStep
    {
        [Display(Name = "None")]
        none = 0,
        [Display(Name = "FloatingIslandChest/FI-noCabins/MountCave/Terrarain/Reset")]
        FloatingIslandChestMountCave,
        [Display(Name = "FloatingIslandChest only")]
        FloatingIslandChestOnly,
        [Display(Name = "2-Steps: FloatIslChestOnly 1st/ with FloatIslposition 2nd (+MountCave/Terrain/Reset)")]
        FloatingIslandChest1stPos2nd,   
    }

   
    
    public enum HardmodeOreTier1
    {
        [Display(Name = "# do not change")]
        doNotChange = 0,        
        Cobalt = TileID.Cobalt,
        Palladium = TileID.Palladium,
        Mythril = TileID.Mythril,
        Orichalcum = TileID.Orichalcum,
        Adamantite = TileID.Adamantite,
        Titanium = TileID.Titanium
    
    }
    public enum HardmodeOreTier2
    {
        [Display(Name = "# do not change")]
        doNotChange = 0,        
        Mythril = TileID.Mythril,
        Orichalcum = TileID.Orichalcum,
        Cobalt = TileID.Cobalt,
        Palladium = TileID.Palladium,
        Adamantite = TileID.Adamantite,
        Titanium = TileID.Titanium
    }
    public enum HardmodeOreTier3
    {
        [Display(Name = "# do not change")]
        doNotChange = 0,        
        Adamantite = TileID.Adamantite,
        Titanium = TileID.Titanium,
        Cobalt = TileID.Cobalt,
        Palladium = TileID.Palladium,
        Mythril = TileID.Mythril,
        Orichalcum = TileID.Orichalcum,
    }

    public enum StartInHardmode{
        [Display(Name = OnOffenable.Off)]
        off = 0,        
        [Display(Name = OnOffenable.On)]
        on,
        [Display(Name = "On but no new biomes")]
        onNoBiomes,
        [Display(Name = "On and all Mechs killed")]
        onMechsKilled,
        [Display(Name = "On and all Mechs killed + update + check for any quick bulb")]
        onMechsKilledAndUpdate,
    }

    public static class WGHackDict
    {
       public static List<string> GetBlockNames(){
            List<string> blocks = new List<string>();
            foreach(var tile in TileID.Search.Names){
                int tileid = TileID.Search.GetId(tile);
                //if(TileObjectData.GetTileData(tileid,0)==null && Main.tileSolid[tileid] ){
                if( !Main.tileFrameImportant[tileid]  ){
                        
                    blocks.Add(tile);
                }
            }
           
            

            blocks.Sort();
            return blocks;
       }


        public static Dictionary<WGHacksEnum, WGHack> wgHackSetting = new Dictionary<WGHacksEnum, WGHack>{
                    {WGHacksEnum.OverwriteWorldTileSizeX,  new WGHack{ name="Overwrite World size X", options = new List<string>{"# do not change", "4200","4800","5000", "5600","6400","7000","8400"},  doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.OverwriteWorldTileSizeX,str ) , disableCustomInput=true} },
                    {WGHacksEnum.OverwriteWorldTileSizeY,  new WGHack{ name="Overwrite World size Y", options = new List<string>{"# do not change", "1000","1200","1600","1800","2000","2400"}, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.OverwriteWorldTileSizeY,str ) , disableCustomInput=true} },
                    {WGHacksEnum.SSdrunkIsActive,  new WGHack{ name="Secret seed drunk world", options = Helpers.EnumHelper<SSdrunkIsActive>.GetAllEnumVariablesAsString(),  doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSdrunkIsActive,str ) , disableCustomInput=true} },
                    {WGHacksEnum.SSworthyIsActive,  new WGHack{ name="Secret seed for the worthy", options = Helpers.EnumHelper<SSworthyIsActive>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSworthyIsActive,str ), disableCustomInput=true } },                    
                    {WGHacksEnum.SScelebrIsActive,  new WGHack{ name="Secret seed celebration", options = Helpers.EnumHelper<SScelebrIsActive>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SScelebrIsActive,str ) , disableCustomInput=true} },
                    {WGHacksEnum.SSDontDigUpIsActive,  new WGHack{ name="Secret seed don't dig up", options = Helpers.EnumHelper<SSDontDigUpIsActive>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSDontDigUpIsActive,str ), disableCustomInput=true } },
                    {WGHacksEnum.SSNoTrapsIsActive,  new WGHack{ name="Secret seed no traps", options = Helpers.EnumHelper<SSNoTrapsIsActive>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSNoTrapsIsActive,str ) , disableCustomInput=true} },
                    {WGHacksEnum.SSconstantIsActive,  new WGHack{ name="Secret seed the constant", options = Helpers.EnumHelper<SSconstantIsActive>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSconstantIsActive,str ) , disableCustomInput=true} },
                    {WGHacksEnum.SSbeesIsActive,  new WGHack{ name="Secret seed not the bees", options = Helpers.EnumHelper<SSbeesIsActive>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSbeesIsActive,str ) , disableCustomInput=true} },
                    {WGHacksEnum.SSZenithIsActive,  new WGHack{ name="Secret seed get fixed Zenith", options = Helpers.EnumHelper<SSZenithIsActive>.GetAllEnumVariablesAsString(),  doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSZenithIsActive,str ) , disableCustomInput=true} },                    
                    {WGHacksEnum.SSStarsCaught,  new WGHack{ name="Stars caught (for Zenith)", options = ConditionElementValueList.generateList(0,200,10) ,  doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.SSStarsCaught,str ) , disableCustomInput=false} },                    
                    //{WGHacksEnum.FixDoubleTempleIncrease,  new WGHack{ name="No double Temple increase", options = Helpers.EnumHelper<FixDoubleTempleIncrease>.GetAllEnumVariablesAsString(), startingVal =  Helpers.EnumHelper<FixDoubleTempleIncrease>.GetNameOFEnumVariable(FixDoubleTempleIncrease.on) ,doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.FixDoubleTempleIncrease,str ) , disableCustomInput=true} },
                    {WGHacksEnum.EndWorldGenAfterWGPass,  new WGHack{ name="End world generation after", options = Helpers.EnumHelper<ConstantEnum.WorldGenPass>.GetAllEnumVariablesAsString(),startingVal = nameof(WorldGenPass.PostWorldGen), doNotSortContentList=true, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.EndWorldGenAfterWGPass,str ), disableCustomInput=true } },
                    {WGHacksEnum.PartialWorldGenPreStep,  new WGHack{ name="Partial WorldGen pre-step ", options = Helpers.EnumHelper<ConstantEnum.PartialWorldGenPreStep>.GetAllEnumVariablesAsString(), doNotSortContentList=true, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.PartialWorldGenPreStep,str ), disableCustomInput=true } },
                    //{WGHacksEnum.StoreWorldImageAfter,  new WGHack{ name="Store world image after", options = Helpers.EnumHelper<ConstantEnum.WorldGenPass>.GetAllEnumVariablesAsString(),startingVal = nameof(WorldGenPass.PostWorldGen), doNotSortContentList=true, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.StoreWorldImageAfter,str ) , disableCustomInput=true} },
                    {WGHacksEnum.HardmodeOreTier1,  new WGHack{ name="Set hardmode tier 1 to", options = Helpers.EnumHelper<HardmodeOreTier1>.GetAllEnumVariablesAsString().Concat(GetBlockNames()).ToList(), doNotSortContentList=true, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.HardmodeOreTier1,str ) , disableCustomInput=true} },
                    {WGHacksEnum.HardmodeOreTier2,  new WGHack{ name="Set hardmode tier 2 to", options = Helpers.EnumHelper<HardmodeOreTier2>.GetAllEnumVariablesAsString().Concat(GetBlockNames()).ToList(), doNotSortContentList=true, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.HardmodeOreTier2,str ) , disableCustomInput=true} },
                    {WGHacksEnum.HardmodeOreTier3,  new WGHack{ name="Set hardmode tier 3 to", options = Helpers.EnumHelper<HardmodeOreTier3>.GetAllEnumVariablesAsString().Concat(GetBlockNames()).ToList(), doNotSortContentList=true, doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.HardmodeOreTier3,str ), disableCustomInput=true } },                    
                    {WGHacksEnum.StartInHardmode,  new WGHack{ name="Start in hardmode", options = Helpers.EnumHelper<StartInHardmode>.GetAllEnumVariablesAsString(), doAfterInitAndTextChange = (str) => WorldInfo.SetValue(WGHacksEnum.StartInHardmode,str ), disableCustomInput=true } },                    
                    
        };



    }


}






