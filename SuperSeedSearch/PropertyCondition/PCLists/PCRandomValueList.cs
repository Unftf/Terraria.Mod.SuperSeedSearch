using System;
using System.Collections.Generic;
using SuperSeedSearch.ConstantEnum;

using Terraria.Utilities;
using Terraria;
using SuperSeedSearch.UI;





namespace SuperSeedSearch.PropertyCondition.PCRandomValue
{
    public static class PCRandomValueConditionList
    {

        public static List<Condition> pcRandomValueList = null;



        static PCRandomValueConditionList()
        {
            AddToListValues(ConstantEnum.PCCalculatedValueTypes.RNGN, ConstantEnum.WorldGenPass.PreWorldGen);
            AddToListSeries(ConstantEnum.PCCalculatedValueTypes.RNGNseries, ConstantEnum.WorldGenPass.PreWorldGen);

        }





        static void AddToListValues(string name, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement, bool> matchFun = null)
        {
            if (pcRandomValueList == null) pcRandomValueList = new List<Condition>();
            PropertyElement propel = new PropertyElement { CondType = ConstantEnum.PropertyType.RNGnumber, matchFun = matchFun };

            List<Constraint> condelli = new List<Constraint>();

            foreach (Constraint condel in BasicConstraintList.pcRNGNValueConstraintList)
            {
                Constraint condelNew = new Constraint(condel);

                if (FirstEntryPoint > condel.delayToworldGenPass)
                    condelNew.delayToworldGenPass = FirstEntryPoint;
                else
                    condelNew.delayToworldGenPass = condel.delayToworldGenPass;

                condelli.Add(condelNew);
            }

            Condition cond = new Condition(ConditionType.RNGnumber, name, propel,condelli){isPersistent=true};
            cond.firstEntryPoint = FirstEntryPoint; 

            pcRandomValueList.Add(cond);
        }

        //todo unify with above
        static void AddToListSeries(string name, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement, bool> matchFun = null)
        {
            if (pcRandomValueList == null) pcRandomValueList = new List<Condition>();
            PropertyElement propel = new PropertyElement { CondType = ConstantEnum.PropertyType.RNGnumber, matchFun = matchFun };

            List<Constraint> condelli = new List<Constraint>();

            foreach (Constraint condel in BasicConstraintList.pcRNGNValueSeriesConstraintList)
            {
                Constraint condelNew = new Constraint(condel);

                if (FirstEntryPoint > condel.delayToworldGenPass)
                    condelNew.delayToworldGenPass = FirstEntryPoint;
                else
                    condelNew.delayToworldGenPass = condel.delayToworldGenPass;                    

                condelli.Add(condelNew);
            }

            Condition cond = new Condition(ConditionType.RNGnumber, name, propel,condelli ){isPersistent=true, postConstraintCheckFun = RNGseriesPostConstraintCheckFun };
            cond.firstEntryPoint = FirstEntryPoint; 

            pcRandomValueList.Add(cond);
        }
        public static Func<Condition, PropertyElement, bool> RNGseriesPostConstraintCheckFun = (c,p) => {
            
            if( c== null || c.constrElemChosenList == null || c.constrElemChosenList.Count == 0) return true;
       
            List<Constraint> Parameter = new List<Constraint>();
            List<Constraint> Constraints = new List<Constraint>();

            int maxDelta = 0;
            foreach(var cons in c.constrElemChosenList){
                if(cons.name.Equals(ConstantEnum.ConstraintNames.RNGNiParameters )) Parameter.Add(cons);
                else if(cons.name.Equals(ConstantEnum.ConstraintNames.RNGNidelta )){
                     Constraints.Add(cons);
                     if(cons.ParameterValue3>maxDelta) maxDelta = (int)cons.ParameterValue3;
                }
                
            }
         

            if(Parameter.Count == 0 || Constraints.Count == 0) return true;            
            
       
            foreach(var para in Parameter){
          
                int toFind = (int)para.targetValue;
                int startIndex = (int)para.ParameterValue1;
                int stepsize = (int)para.ParameterValue2;
                int maxIndex = (int)para.ParameterValue3;
                
                int seed = ModMenuMod.wGPassChanger.currentSeed;
                UnifiedRandom rand = new UnifiedRandom( seed );                    
                int totalSize = maxIndex+1+maxDelta;

                double[] values = new double[totalSize];

                for(int i=0; i<totalSize; i++) values[i] =   rand.NextDouble();
                

                int found = 0;
                for(int i=startIndex; i<=maxIndex; i+=stepsize){
                   
                    bool alltrue = true;
                    foreach(var cons in Constraints){
                        
                        double scaleMin = (double)cons.ParameterValue1;
                        double scaleMax = (double)cons.ParameterValue2;
                        int deltai = (int)cons.ParameterValue3;                        

                        if(i+deltai<0){ alltrue = false; break;}

                        double scaledNum =  (values[i+deltai]*(scaleMax-scaleMin) + scaleMin);
                        double target = (double)cons.targetValue.valueDouble;

                       
                        //TODO extern fun
                        switch(cons.gole){
                            case ConstantEnum.GreaterOrLessEqual.GreaterOrEqual:
                                alltrue &= scaledNum>=target;
                                break;
                            case ConstantEnum.GreaterOrLessEqual.LessOrEqual:
                                alltrue &= scaledNum<=target;
                                break;
                            case ConstantEnum.GreaterOrLessEqual.Equal:
                                alltrue &= scaledNum==target;
                                break;
                            case ConstantEnum.GreaterOrLessEqual.NotEqual:
                                alltrue &= scaledNum!=target;
                                break;
                            default: 
                                alltrue = false;
                                break;                            
                        }
                        if(!alltrue) break;
                    }
                    if(alltrue)
                         found++;
                         
                }

                //TODO extern fun, no need to search all indices if condition already true
                switch(para.gole){
                    case ConstantEnum.GreaterOrLessEqual.GreaterOrEqual:
                        if( found >= toFind ) return true;
                        break;
                    case ConstantEnum.GreaterOrLessEqual.LessOrEqual:
                        if( found <= toFind ) return true;
                        break;
                    case ConstantEnum.GreaterOrLessEqual.Equal:
                        if( found == toFind ) return true;
                        break;
                    case ConstantEnum.GreaterOrLessEqual.NotEqual:
                        if( found != toFind ) return true;
                        break;
                    default:                     
                        break;                            
                }                  
                
            }


            return false;
        };



    }

}