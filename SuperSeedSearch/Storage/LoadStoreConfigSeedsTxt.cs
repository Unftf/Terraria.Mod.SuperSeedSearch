using Terraria;
using System;
using System.IO;
using System.Collections.Generic;
using SuperSeedSearch.UI;
using SuperSeedSearch.PropertyCondition;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Helpers;



using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;



namespace SuperSeedSearch.Storage
{ 

    public static class LoadStoreConfigSeedsTxt
    {  
        const string SplitChar = "|";
        const string sep = " " + SplitChar + " ";
        const string condPref = "	";
        const string constdPref = condPref+condPref;


        public static void SaveConfig(string name){
            if (name.Equals(ConstantsStrings.NewClearCondtions) ){   
                var sound = SoundID.MenuTick;
                sound.Pitch = 0.01f*Main.rand.Next(95, 100);
                sound.Volume = 0.75f;
                SoundEngine.PlaySound(sound);
                return;
            }
            if(name.Equals( ConstantsStrings.DefaultConfig)) name = ConstantsStrings.DefaultConfigFileName;
                        
            if (!System.IO.Directory.Exists(Main.SavePath + ConstantsStrings.modpath))
                System.IO.Directory.CreateDirectory(Main.SavePath + ConstantsStrings.modpath);

            string filePath = Main.SavePath + ConstantsStrings.modpath + @"/config" + name + ".txt";


            if( ModMenuMod.uiSuperSeed ==null || ModMenuMod.uiSuperSeed.condInterface == null ||  ModMenuMod.uiSuperSeed.condInterface.allTabs == null) return;

            string save = "";
            List<UIConditionInterface.UIAllTab> tabs = ModMenuMod.uiSuperSeed.condInterface.allTabs;
            foreach(var tab in tabs){
                if(tab.IsMainTab() ){
                    save += ConstantEnum.SearchQueryHeader.MainSettings +sep + Helpers.EnumHelper<TabIsActive>.GetNameOFEnumVariable(tab.IsActiveTab)  + Environment.NewLine;
                    UIConditionInterface.MainProps mtab = (UIConditionInterface.MainProps)tab;
                    foreach (ConstantEnum.MainSettingEnum item in Enum.GetValues(typeof(ConstantEnum.MainSettingEnum)) ){
                        if(mtab.GetValue(item).Length != 0)
                            save += condPref + Helpers.EnumHelper<ConstantEnum.MainSettingEnum>.GetNameOFEnumVariable(item) + sep + mtab.GetValue(item) + Environment.NewLine;
                    }
                    save += Environment.NewLine;
                }
                else if(tab is UIConditionInterface.WGHacksTab){
                    save += ConstantEnum.SearchQueryHeader.WGHacks +sep + Helpers.EnumHelper<TabIsActive>.GetNameOFEnumVariable(tab.IsActiveTab) + Environment.NewLine;
                    UIConditionInterface.WGHacksTab htab = (UIConditionInterface.WGHacksTab)tab;
                    foreach (ConstantEnum.WGHacksEnum item in Enum.GetValues(typeof(ConstantEnum.WGHacksEnum)) ){                        
                        if(htab.GetValue(item).Length != 0)
                            save += condPref + Helpers.EnumHelper<ConstantEnum.WGHacksEnum>.GetNameOFEnumVariable(item) + sep + htab.GetValue(item) + Environment.NewLine;
                    }
                    save += Environment.NewLine;
                }
                else if(tab is UIConditionInterface.SearchQueryTab){
                    UIConditionInterface.SearchQueryTab stab = (UIConditionInterface.SearchQueryTab) tab;

                    save += stab.header + sep + Helpers.EnumHelper<TabIsActive>.GetNameOFEnumVariable(tab.IsActiveTab) + Environment.NewLine;
                    List<PropertyCondition.Condition> clist =  stab.tabListContent.ExportAsConditonList();
                    foreach(PropertyCondition.Condition cond in clist){                        
                        save += condPref + Helpers.EnumHelper<ConstantEnum.ConditionType>.GetNameOFEnumVariable(cond.conditionType) + sep + cond.name + sep + GreaterOrLessEqualClass.ToDString(cond.gole) +sep+ cond.targetEvalValue.GetValueAsString() + Environment.NewLine;
                        List<Constraint> constList = cond.constrElemChosenList;
                        foreach(Constraint constr in constList){
                            save += constdPref + constr.ToString(sep)+ Environment.NewLine;
                        }
                    }

                    save += Environment.NewLine;
                }else{
                    save += "tab could not be parsed" + Environment.NewLine;
                }
            }

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filePath, false))
            {   
                file.WriteLine(save);
            }
            var sound2 = SoundID.Camera;            
            sound2.Volume = 0.5f;
            SoundEngine.PlaySound(sound2);
        }
  
        static bool isLoading = false;
        
        public static void LoadConfig(string name, bool silent=false ){     
            
            if(name.Equals(ConstantsStrings.DefaultConfig))  name = ConstantsStrings.DefaultConfigFileName ;


            if (name.IndexOf("nodig4pl", StringComparison.OrdinalIgnoreCase )>-1){
                Pathlength.Pathlength.nodig = true;     
                var sound1 = SoundID.Run;
                sound1.Pitch = 0.01f*Main.rand.Next(95, 100);                
                
                SoundEngine.PlaySound(sound1);           
                if (name.IndexOf("nodig4plDraw", StringComparison.OrdinalIgnoreCase )>-1){
                    Pathlength.Pathlength.nodigDrawPath = true;   
                }
            }
            else
            {
                if(Pathlength.Pathlength.nodig){
                    var sound1 = SoundID.Dig;
                    sound1.Pitch = 0.01f*Main.rand.Next(95, 100);
                    SoundEngine.PlaySound(sound1);  
                }    
                if (name.IndexOf("dig4plDraw", StringComparison.OrdinalIgnoreCase )>-1){
                    Pathlength.Pathlength.nodigDrawPath = true;   
                }else Pathlength.Pathlength.nodigDrawPath = false;   

                Pathlength.Pathlength.nodig = false;                
            }


            if(isLoading) return;                
            if (!System.IO.Directory.Exists(Main.SavePath + ConstantsStrings.modpath))
                return;
            if(ModMenuMod.uiSuperSeed == null || ModMenuMod.uiSuperSeed.condInterface==null || ModMenuMod.uiSuperSeed.condInterface.mainPropsPanel== null || ModMenuMod.uiSuperSeed.tabBar==null || ModMenuMod.uiSuperSeed.tabBar.tabList==null)
                return;
           
            string filePath = Main.SavePath + ConstantsStrings.modpath + @"/config" + name + ".txt";

            if (!System.IO.File.Exists(filePath) && !name.Equals(ConstantsStrings.NewClearCondtions) ){        
                var sound1 = SoundID.ChesterOpen;
                sound1.Pitch = 0.01f*Main.rand.Next(95, 100);
                if(!silent)SoundEngine.PlaySound(sound1);                
                return;
            }

            ModMenuMod.uiSuperSeed.condInterface.Reset();
            ModMenuMod.uiSuperSeed.tabBar.Reset();
            if( name.Equals(ConstantsStrings.NewClearCondtions)){
                var sound = SoundID.Grass;
                sound.Pitch = 0.5f*Main.rand.Next(95, 100);
                if(!silent)SoundEngine.PlaySound(sound );
                return;
            }

            isLoading = true;

            
            UITabBar.UITabbarTab.TabType tabType = UITabBar.UITabbarTab.TabType.TAB_NEW;
            int tabID = 0;
            bool isConstraint = false;
            bool isCondition = false;
            PropertyCondition.Condition curCond = null;

            UIConditionInterface.WGHacksTab htab = null;
            UIConditionInterface.SearchQueryTab stab = null;       
            //todo generalize simplify InsertCondition    

            
            

            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            string line;
            while ((line = file.ReadLine()) != null)
            {   
                if(line.Length<2)continue;

                string[] headerIsActive = line.Split(SplitChar[0],2);  

                if(headerIsActive[0].Trim().Equals(ConstantEnum.SearchQueryHeader.MainSettings)){                    
                    tabType = UITabBar.UITabbarTab.TabType.TAB_MAIN;continue;
                }else if(headerIsActive[0].Trim().Equals(ConstantEnum.SearchQueryHeader.WGHacks)){
                    if(curCond!=null) { 
                            SaveCurCond(stab, ref curCond);                     
                    }                     
                    tabID++;
                    if(ModMenuMod.uiSuperSeed.tabBar.tabList.Count<=tabID+1){                        
                        ModMenuMod.uiSuperSeed.tabBar.AddTabOnNewTabClick(UITabBar.UITabbarTab.TabType.TAB_WGHAX);
                    }
                    tabType = UITabBar.UITabbarTab.TabType.TAB_WGHAX;
                    ModMenuMod.uiSuperSeed.condInterface.ChangeTab(tabID);

                    htab = (UIConditionInterface.WGHacksTab)ModMenuMod.uiSuperSeed.condInterface.currentTab;
                    if(headerIsActive.Length == 2) htab.hol.inputActiveTab.UpdateText(headerIsActive[1].Trim());
                    continue;
                }else if(headerIsActive[0].Trim().Contains(ConstantEnum.SearchQueryHeader.SearchQuery)){
                    if(curCond!=null) { 
                            SaveCurCond(stab, ref curCond);                        
                    }                     
                    tabID++;
                    if(ModMenuMod.uiSuperSeed.tabBar.tabList.Count<=tabID+1){                        
                        ModMenuMod.uiSuperSeed.tabBar.AddTabOnNewTabClick(UITabBar.UITabbarTab.TabType.TAB_CONDITION);
                    }
                    tabType = UITabBar.UITabbarTab.TabType.TAB_CONDITION;
                    ModMenuMod.uiSuperSeed.condInterface.ChangeTab(tabID);
                    stab = (UIConditionInterface.SearchQueryTab)ModMenuMod.uiSuperSeed.condInterface.currentTab;
                    if(headerIsActive.Length == 2) stab.tabListContent.inputActiveTab.UpdateText(headerIsActive[1].Trim());
                    continue;                    
                }
                
                if(line.Substring(0,2)==constdPref){
                    isConstraint = true;
                    isCondition = false;
                }
                else if(line.Substring(0,1)==condPref){
                    isCondition = true;
                    isConstraint = false;
                }else continue;

                if(isCondition){                    
                    if(tabType == UITabBar.UITabbarTab.TabType.TAB_MAIN){
                        string[] nameval = line.Split(SplitChar[0],2);                        
                        if(EnumHelper<MainSettingEnum>.GetValueFromName(nameval[0].Trim()) != MainSettingEnum.SearchConfigName ){                            
                            ModMenuMod.uiSuperSeed.condInterface.mainPropsPanel.ChangeValue( EnumHelper<MainSettingEnum>.GetValueFromName(nameval[0].Trim()), nameval[1].Trim() );
                        }
                    }else if(tabType == UITabBar.UITabbarTab.TabType.TAB_WGHAX){                        
                        string[] nameval = line.Split(SplitChar[0],2);                        
                        htab.ChangeValue( EnumHelper<WGHacksEnum>.GetValueFromName(nameval[0].Trim()), nameval[1].Trim());
                    }else if(tabType == UITabBar.UITabbarTab.TabType.TAB_CONDITION){        
                        if(curCond!=null) { 
                            SaveCurCond(stab, ref curCond);                        
                        } 
                        string[] typeNameGoleValue = line.Split(SplitChar[0],4);  
                        ConditionType condType = EnumHelper<ConditionType>.GetValueFromName(typeNameGoleValue[0].Trim());
                        string condName = typeNameGoleValue[1].Trim();
                        GreaterOrLessEqual gole = EnumHelper<GreaterOrLessEqual>.GetValueFromName(typeNameGoleValue[2].Trim());
                        int targetValue = Int32.Parse(typeNameGoleValue[3].Trim());

                        PropertyCondition.Condition cond = AllTypeList.FindCondition(condType, condName);
                        if(cond!=null){
                            curCond = new PropertyCondition.Condition(cond);
                            if(curCond.constrElemChosenList==null) curCond.constrElemChosenList = new List<Constraint>();
                            curCond.gole = gole;
                            curCond.targetEvalValue.SetValue(targetValue);
                        }
                    }else{
                        Console.WriteLine("error: could not read config file");
                    }                    
                }else if(isConstraint){                    
                    if(curCond == null ) continue;
                    
                    string[] nameGoleTargetParam1TP2TP3T = line.Split(SplitChar[0],9);                                      
                    GreaterOrLessEqual gole = EnumHelper<GreaterOrLessEqual>.GetValueFromName(nameGoleTargetParam1TP2TP3T[1].Trim());
                    ValueStringAndOrDouble param1 = nameGoleTargetParam1TP2TP3T.Length >4?nameGoleTargetParam1TP2TP3T[4].Trim():"";
                    ValueStringAndOrDouble param2 = nameGoleTargetParam1TP2TP3T.Length >6?nameGoleTargetParam1TP2TP3T[6].Trim():"";
                    ValueStringAndOrDouble param3 = nameGoleTargetParam1TP2TP3T.Length >8?nameGoleTargetParam1TP2TP3T[8].Trim():"";
                    
                    curCond.AddConstraintByName(nameGoleTargetParam1TP2TP3T[0].Trim(), gole, nameGoleTargetParam1TP2TP3T[2].Trim(),param1,param2,param3);                   


                }else{
                    Console.WriteLine("error: could not read line " + line);
                }
            }       
            if(curCond!=null) { 
                SaveCurCond(stab, ref curCond);
            }      
            ModMenuMod.uiSuperSeed.condInterface.ChangeTab(0);
            file.Close();
            isLoading = false;            
            var sound2 = SoundID.ResearchComplete;
            sound2.Pitch = 0.5f*Main.rand.Next(95, 100);
            if(!silent)SoundEngine.PlaySound(sound2);
            //SoundEngine.PlaySound(SoundID.ResearchComplete with {Pitch=0.5f*Main.rand.Next(95, 100)});
        }
        public static void SaveCurCond(UIConditionInterface.SearchQueryTab stab, ref PropertyCondition.Condition curCond){
            
            stab.tabListContent.InsertCondition(curCond );                
            curCond=null; 
            
    }
    }

    public class LoadSeedsTxt{

        public static string[] LoadSeedFile(){
            string[] seeds = new string[0];
            string name = Main.WorldPath + @"/seeds.txt";
            if(!System.IO.File.Exists(name)) return seeds;

            using (StreamReader sr = new StreamReader(name)){
                string line = sr.ReadToEnd();
                seeds= line.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); 
            }
            return seeds;
        }

    }



}