using System;
using System.Collections.Generic;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Storage;

namespace SuperSeedSearch.PropertyCondition
{
    public class Condition
    {
        public class TargetEvalValue : Constraint.TargetValue
        {
            internal TargetEvalValue()
            {
                this.evalFun = null;
            }

            public TargetEvalValue(double targetValueDouble, Func<Condition, bool> evalFun = null) : base(targetValueDouble)
            {
                this.evalFun = evalFun;
            }
            public TargetEvalValue(string targetValueString, Func<Condition, bool> evalFun = null) : base(targetValueString)
            {
                this.evalFun = evalFun;                
            }

            public Func<Condition, bool> evalFun { get; } = null;     
                   
        }



        public int count { get; private set; } = 0;
        public double countDivisor { get; set; } = 1;

        public double countSubtractor { get; set; } = 0;

        public bool isPersistent { get; set; } = false;//count doesn't get deleted after new wg pass

        public PassMemory.SeedMemoryBase<bool> memory = null; 
        public Func<int> GetConditionMemorySize = null;

        public ConstantEnum.WorldGenPass firstEntryPoint = ConstantEnum.WorldGenPass.PostWorldGen;
        public ConstantEnum.WorldGenPass lastEntryPoint = ConstantEnum.WorldGenPass.PostWorldGen;
        public ConstantEnum.WorldGenPass firstEntryPointMin {get; private set;} = ConstantEnum.WorldGenPass.PostWorldGen;

        public ConstantEnum.GreaterOrLessEqual gole = ConstantEnum.GreaterOrLessEqual.GreaterOrEqual;

        public string name { get; private set; }

        public TargetEvalValue targetEvalValue { get; private set; } = default(TargetEvalValue);

        public List<Constraint> constrElemChosenList = null;
        public List<Constraint> possibleConstrElemts { get; private set; } = null;

        public List<PropertyElement> appliesToPropertyElements { get; private set; } = null;

        public Func<Condition, PropertyElement, bool> postConstraintCheckFun = (c,p)=>true;
        ConditionLogger conditionLogger;
        public ConditionType conditionType{get;private set;}

        private Condition()
        {

        }
        public Func<Condition, bool> defaultEvalFun {get;}= (c =>
        {
            double res = c.count / c.countDivisor - c.countSubtractor;
            if (c.gole == ConstantEnum.GreaterOrLessEqual.GreaterOrEqual) return res >= c.targetEvalValue.valueDouble;
            if (c.gole == ConstantEnum.GreaterOrLessEqual.LessOrEqual) return res <= c.targetEvalValue.valueDouble;
            return res > 0;

        });

        public void SetSingleEntryPoint(ConstantEnum.WorldGenPass  single){
            firstEntryPoint = single;
            lastEntryPoint = single;
            firstEntryPointMin =single;
        }



        public Condition(Condition c2copy){//no deep copy
            
            conditionType = c2copy.conditionType;
            
            name = c2copy.name;
            
            appliesToPropertyElements = c2copy.appliesToPropertyElements;
            
            possibleConstrElemts = c2copy.possibleConstrElemts;
            

            targetEvalValue = c2copy.targetEvalValue;
            
            gole = c2copy.gole;
            
            firstEntryPointMin = c2copy.firstEntryPointMin;
            firstEntryPoint = c2copy.firstEntryPoint;
            lastEntryPoint = c2copy.lastEntryPoint;
            
            conditionLogger = c2copy.conditionLogger;
            
            isPersistent=c2copy.isPersistent;
            countDivisor=c2copy.countDivisor;
            countSubtractor=c2copy.countSubtractor;
            
            postConstraintCheckFun=c2copy.postConstraintCheckFun;      

            memory = c2copy.memory;   
            GetConditionMemorySize = c2copy.GetConditionMemorySize;   

       

        }


        public Condition(ConditionType conditionType, string name,
            List<PropertyElement> appliesToPropertyElements = null,
            List<Constraint> possibleCondElemts = null,
            TargetEvalValue targetEvalValue = null,
            Func<int> GetConditionMemorySize = null
         )
        {
            this.conditionType = conditionType;

            if (targetEvalValue == null) this.targetEvalValue = new TargetEvalValue();
            else this.targetEvalValue = targetEvalValue;

            if (!this.targetEvalValue.IsStringEmpty) gole = ConstantEnum.GreaterOrLessEqual.Equal;
            else gole = ConstantEnum.GreaterOrLessEqual.GreaterOrEqual;
            this.name = name;
            firstEntryPointMin = ConstantEnum.WorldGenPass.PostWorldGen;
            
            if(possibleCondElemts != null)
            foreach (var item in possibleCondElemts)
            {
                if(item!=null)
                    firstEntryPointMin = item.delayToworldGenPass < firstEntryPointMin ? item.delayToworldGenPass : firstEntryPointMin;
            }
            firstEntryPoint = firstEntryPointMin;        
            lastEntryPoint = firstEntryPoint;
            


            this.appliesToPropertyElements = appliesToPropertyElements;
            this.possibleConstrElemts = possibleCondElemts;
            this.conditionLogger = new ConditionLogger();

            this.GetConditionMemorySize = GetConditionMemorySize!=null? GetConditionMemorySize : null;
            this.memory = this.GetConditionMemorySize==null? null: new PassMemory.SeedMemoryBase<bool>(this.GetConditionMemorySize);

            
        }


        public Condition(ConditionType conditionType, string name,
            PropertyElement appliesToPropertyElement,
            List<Constraint> possibleCondElemts = null,
            TargetEvalValue targetEvalValue = null
         ) : this(conditionType, name, new List<PropertyElement> { appliesToPropertyElement }, possibleCondElemts, targetEvalValue) { }

        public Condition(ConditionType conditionType, string name,
            PropertyElement appliesToPropertyElement,
            Constraint possibleCondElemt,
            TargetEvalValue targetEvalValue = null
         ) : this(conditionType, name, new List<PropertyElement> { appliesToPropertyElement }, new List<Constraint> { possibleCondElemt }, targetEvalValue) { }



        public bool CheckIfPropertyElementIsValid(PropertyElement gpe)
        {
            if (appliesToPropertyElements == null) return false;
            foreach (PropertyElement pe in appliesToPropertyElements)
            {                
                if (pe.SetProperitesAlsoIn(gpe)) return true;
            }
            return false;
        }


        public bool EvaluateConstraintList(PropertyElement gpe, ConstantEnum.WorldGenPass lastPass)
        {
            
            if (lastPass < firstEntryPoint) return true;
            if ( isPersistent && lastEntryPoint< lastPass) return true;

            int condElmTrue = 0;
            if (constrElemChosenList != null)
                foreach (Constraint ce in constrElemChosenList)
                {

                    if (ce.delayToworldGenPass != ConstantEnum.WorldGenPass.Unknown && ce.delayToworldGenPass <= lastPass)
                    {
                        condElmTrue++;
                        bool val = ce.Evaluate(gpe);                     
                        if (!val) return false;                    
                    }                
                }
            if(!postConstraintCheckFun(this,gpe)) return false;
            if(targetEvalValue < 15) conditionLogger.Log(gpe);
            count++;
            return true;
        }



        public bool PostAnalysisCheckIfConditionIsTrue(ConstantEnum.WorldGenPass lastPass)
        {
            //if (lastPass == firstEntryPoint && lastPass>= ConstantEnum.WorldGenPass.Reset) Console.WriteLine("---------------> Checkinging out pass " + lastPass + " count is " + count);            

            if (lastPass < firstEntryPoint || constrElemChosenList == null) return true;                        
            if (targetEvalValue.evalFun != null) return targetEvalValue.evalFun(this);                        
            return defaultEvalFun(this);
        }

        public void ResetPreNextPass(bool keepPersitent, ConstantEnum.WorldGenPass lastWGPass)
        {            
            if (keepPersitent == false || !isPersistent || lastEntryPoint>=lastWGPass){ count = 0;conditionLogger.Clear(); };

        }
        public void ResetPreStartOfSearch()
        {
            count = 0;
            firstEntryPoint = ConstantEnum.WorldGenPass.PostWorldGen;
            lastEntryPoint = firstEntryPointMin;
            if (constrElemChosenList != null)
            {
                foreach (Constraint ce in constrElemChosenList)
                {
                    if (ce.delayToworldGenPass != ConstantEnum.WorldGenPass.Unknown && ce.delayToworldGenPass < firstEntryPoint)
                    {
                        firstEntryPoint = ce.delayToworldGenPass;
                    }
                    if (ce.delayToworldGenPass != ConstantEnum.WorldGenPass.Unknown && ce.delayToworldGenPass > lastEntryPoint)
                    {
                        lastEntryPoint = ce.delayToworldGenPass;
                    }
                }
            }
            
            if(constrElemChosenList == null ||  constrElemChosenList.Count == 0 || firstEntryPoint<firstEntryPointMin)
                firstEntryPoint = firstEntryPointMin;
        }


        public void SetCondElmChosenList(List<Constraint> cel)
        {
            constrElemChosenList = new List<Constraint>();
            foreach (var item in cel)
            {
                 constrElemChosenList.Add(new Constraint(item) )   ;
            }
        }
        public bool AddConstraintByName(string name, GreaterOrLessEqual? gole = null, ValueStringAndOrDouble targetVal = null, ValueStringAndOrDouble param1 = null, ValueStringAndOrDouble param2 = null, ValueStringAndOrDouble param3 = null){
            if(constrElemChosenList == null) constrElemChosenList = new List<Constraint>();
            foreach(Constraint constr in possibleConstrElemts){
                if(constr.name.Equals(name ) ){
                    var nc = new Constraint(constr);
                    if(targetVal!=null && targetVal.GetValueAsString().Length>0) nc.targetValue.SetValue(targetVal);
                    if(param1!=null && param1.GetValueAsString().Length>0) nc.ParameterValue1.SetValue(param1);//todo this doesnt check if name is equal
                    if(param2!=null && param2.GetValueAsString().Length>0) nc.ParameterValue2.SetValue(param2);
                    if(param3!=null && param3.GetValueAsString().Length>0) nc.ParameterValue3.SetValue(param3);
                    if(gole!=null) nc.gole = (GreaterOrLessEqual)gole;                    
                    constrElemChosenList.Add(nc );
                    return true;
                }
            }


            return false;
        }


        public List<string> AsStringList()
        {
            List<string> list = new List<string>();
            string scount = targetEvalValue.valueDouble == null && targetEvalValue.IsStringEmpty?"": (targetEvalValue.evalFun==null?count+(countDivisor!=1?"/"+countDivisor:"")+(countSubtractor!=0?"-"+countSubtractor:""):"f("+count+")");
            list.Add($"# {name}: {scount} {GreaterOrLessEqualClass.ToDString(gole) } {targetEvalValue.GetValueAsString()}");
            foreach(var ctr in constrElemChosenList){
                    list.Add(ctr.ToString());
            }

            if(conditionLogger.loglist.Count>0 && count/countDivisor-countSubtractor<100){
                list.Add("condition true for: ");
                foreach(var propistrue in conditionLogger.loglist){
                        list.Add(propistrue);
                }
            }
            
            return list;
        }


    }
}