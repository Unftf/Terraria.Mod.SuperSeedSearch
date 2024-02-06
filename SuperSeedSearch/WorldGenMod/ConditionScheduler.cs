using System.Collections.Generic;
using SuperSeedSearch.UI;
using SuperSeedSearch.Storage;
using System;



namespace SuperSeedSearch.WorldGenMod
{
    public class ConditionScheduler
    {
        List<PropertyCondition.Condition>[] allTabCond;
        bool[] tabIsGlobal;
        bool[] tabIsPassive;
        ConstantEnum.WorldGenPass[] lastPassOfTab;
        public List<PropertyCondition.Condition> ListAllCond { get; private set; }
        public List<PropertyCondition.Condition> ListAllCondPassive { get; private set; }

        public bool[] WGPassHasAnyConditionConstraint = null; 

        bool[] allTabValid;
        public ConditionScheduler(){
            
        }

        public void Reset()
        {
            int tabNum = ModMenuMod.uiSuperSeed.condInterface.allTabs.Count;
            allTabValid = new bool[tabNum];
            allTabCond = new List<PropertyCondition.Condition>[tabNum];
            ListAllCond = new List<PropertyCondition.Condition>();
            ListAllCondPassive = new List<PropertyCondition.Condition>();
            tabIsGlobal = new bool[tabNum];
            tabIsPassive = new bool[tabNum];
            lastPassOfTab = new ConstantEnum.WorldGenPass[tabNum];

            for (int i = 0; i < tabNum; i++)
            {
                allTabCond[i] = new List<PropertyCondition.Condition>();
                
            }

            WorldInfo.SetValue(ConstantEnum.WGHacksEnum.WGHacksIsActive, ConstantEnum.OnOffenable.Off.ToString() );

            if (ModMenuMod.uiSuperSeed == null || ModMenuMod.uiSuperSeed.condInterface == null || ModMenuMod.uiSuperSeed.condInterface.allTabs == null) return;
            for (int i = 0; i < ModMenuMod.uiSuperSeed.condInterface.allTabs.Count; i++)
            {
                var item = ModMenuMod.uiSuperSeed.condInterface.allTabs[i];

                if (item.GetType().Equals(typeof(UIConditionInterface.SearchQueryTab)) && item.IsActiveTab != TabIsActive.disabled )
                {
                    if( item.IsActiveTab == TabIsActive.global){ tabIsGlobal[i] = true; }
                    else if( item.IsActiveTab == TabIsActive.passive){ tabIsPassive[i] = true; }

                    lastPassOfTab[i] = ConstantEnum.WorldGenPass.PreWorldGen;

                    List<PropertyCondition.Condition> tabConditions = (item as UIConditionInterface.SearchQueryTab).tabListContent.ExportAsConditonList();
                    if (tabConditions != null)
                    {
                        foreach (var item2 in tabConditions)
                        {

                            item2.ResetPreStartOfSearch();
                            if(item.IsActiveTab == TabIsActive.passive)
                                ListAllCondPassive.Add(item2);
                            else
                                ListAllCond.Add(item2);

                                                        
                            //todo combine with SetUpActivePasses
                            if(item2.constrElemChosenList == null || item2.constrElemChosenList.Count == 0){                                
                                if(item2.firstEntryPointMin>lastPassOfTab[i]) lastPassOfTab[i] = item2.firstEntryPointMin;
                            }
                            else
                            {
                                foreach(var constr in item2.constrElemChosenList){                                
                                    if(constr.delayToworldGenPass>lastPassOfTab[i]) lastPassOfTab[i] = constr.delayToworldGenPass;
                                }   
                            }



                        }
                        allTabCond[i] = tabConditions;

                    }
                }else if(item.GetType().Equals(typeof(UIConditionInterface.WGHacksTab)) && item.IsActiveTab == TabIsActive.active){
                    WorldInfo.SetValue(ConstantEnum.WGHacksEnum.WGHacksIsActive, ConstantEnum.OnOffenable.On.ToString() );
                }
            }

            SetUpActivePasses();
            lastPassUsedPassive = lastPassUsed;
            foreach(var pc in ListAllCondPassive){
                //if(pc.firstEntryPoint < lastPassUsed) pc.firstEntryPoint = lastPassUsed;
                if (pc.firstEntryPoint > lastPassUsedPassive) lastPassUsedPassive = pc.firstEntryPoint;
                ListAllCond.Add(pc);
            }
            SetUpActivePasses();
            (lastPassUsedPassive , lastPassUsed) = (lastPassUsed , lastPassUsedPassive);

           

            SetIDsForChestsItem();
            //SetIDsWalls();
            

        }
        //todo2: not nice here
        private void SetIDsForChestsItem()
        {
            foreach (var item in ListAllCond)
            {
                if (item.conditionType == ConstantEnum.ConditionType.Chest )
                    foreach (var constr in item.constrElemChosenList)
                    {
                        if (constr.name.Equals(ConstantEnum.ConstraintNames.ContainsItem))
                            constr.targetValue.SetValue(constr.targetValue.valueString, PropertyCondition.ContainsItemList.LookUpItem[constr.targetValue.valueString]);
                    }
            }
        }


        private int wgPassOffset = 0;
        public ConstantEnum.WorldGenPass lastPassUsed {get;private set;}= ConstantEnum.WorldGenPass.PreWorldGen;
        public ConstantEnum.WorldGenPass lastPassUsedPassive {get;private set;}= ConstantEnum.WorldGenPass.PreWorldGen;

        private void SetUpActivePasses(){
            lastPassUsed = ConstantEnum.WorldGenPass.PreWorldGen;

            wgPassOffset = -((int)ConstantEnum.WorldGenPass.PreWorldGen);
            int numPass = wgPassOffset+(int)ConstantEnum.WorldGenPass.PostWorldGen+1;
            WGPassHasAnyConditionConstraint = new bool[numPass];

            foreach(var cond in ListAllCond)            
                if(cond.constrElemChosenList == null || cond.constrElemChosenList.Count == 0){
                    WGPassHasAnyConditionConstraint[wgPassOffset+(int)cond.firstEntryPointMin] = true;
                    if(cond.firstEntryPointMin>lastPassUsed) lastPassUsed = cond.firstEntryPointMin;
                }
                else
                {
                 foreach(var constr in cond.constrElemChosenList){
                    WGPassHasAnyConditionConstraint[wgPassOffset+(int)constr.delayToworldGenPass] = true;
                    if(constr.delayToworldGenPass>lastPassUsed) lastPassUsed = constr.delayToworldGenPass;
                    }   
                }
                
        }
        public bool HasAnyThing4WGPass( ConstantEnum.WorldGenPass wgpass){

            return  WGPassHasAnyConditionConstraint!=null && WGPassHasAnyConditionConstraint[wgPassOffset+(int)wgpass];
        }



        public void ResetConditionsAfterPass(bool keepPersistent, ConstantEnum.WorldGenPass lastWGPass)
        {
            
            ListAllCond.ForEach((c) => c.ResetPreNextPass(keepPersistent, lastWGPass));
        }       

        public bool CheckOutIfAnyConditionTrueAfterFullWorldAnalysis(ConstantEnum.WorldGenPass lastPass)
        {
            for (int i = 0; i < allTabValid.Length; i++) allTabValid[i] = false;
            int anyNotEmpty = 0;
            
  
            for(int g=0; g<2;g++){
                bool isGlobal = g==0? true:false;
                anyNotEmpty = 0;
                for (int ti = 0; ti < allTabValid.Length; ti++)
                {   
                    allTabValid[ti] = false;

                    if( tabIsGlobal[ti] != isGlobal ) continue;     
                    
                    if( tabIsPassive[ti] && (lastPass < lastPassUsed || lastPassOfTab[ti] > lastPassUsed) && lastPass < ConstantEnum.WorldGenPass.FinalCleanup && lastPass<lastPassOfTab[ti]) continue;

                    bool isTrue = true;                    
                    foreach (var cond in allTabCond[ti])
                    {
                        isTrue &= cond.PostAnalysisCheckIfConditionIsTrue(lastPass);      

                        anyNotEmpty++;
                        if (!isTrue) break;
                    }
                    if(isGlobal && !isTrue) return false;
                    
                    allTabValid[ti] =  allTabCond[ti].Count!=0 ? isTrue : false;   
                }
                
            }

            bool result = false;
            foreach (var item in allTabValid)
            {
                result |= item;
            }

            return anyNotEmpty == 0 ? true : result;

        }

        public string GetConditionStateAsString(){
            string all = "";
            foreach(var cond in ListAllCond){
                List<string> condsl = cond.AsStringList();
                foreach(var line in condsl){
                    all = all+System.Environment.NewLine+line;
                }
                all = all+System.Environment.NewLine;
            }
            return all;
        }
    }
    


}