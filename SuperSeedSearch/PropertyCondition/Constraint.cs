using System;
using System.Collections.Generic;
using SuperSeedSearch.ConstantEnum;


namespace SuperSeedSearch.PropertyCondition
{



    public class Constraint
    {
        public class TargetValue : ValueStringAndOrDouble
        {

            internal TargetValue() : base()
            {

            }


            public TargetValue(TargetValue old) : base(old.GetValueAsString())
            {

            }


            public TargetValue(double targetValueDouble) : base(targetValueDouble) { }
            public TargetValue(string targetValueString) : base(targetValueString) { }



            public static implicit operator TargetValue(string val) => new TargetValue(val);
            public static implicit operator TargetValue(float val) => new TargetValue((double)val);
            public static implicit operator TargetValue(double val) => new TargetValue(val);

            public static implicit operator string(TargetValue val) => val.valueString;
            public static implicit operator double?(TargetValue val) => val.valueDouble;
        }

        public class CondParameterValue : ValueStringAndOrDouble
        {
            internal CondParameterValue() : base()
            {

                name = "";
            }
            public CondParameterValue(string name, string paramValue)
            {
                this.name = name;
                SetValue(paramValue);
            }
            public CondParameterValue(string name, double paramValue)
            {
                this.name = name;
                SetValue(paramValue);
            }
            public CondParameterValue(CondParameterValue old) : base(old.GetValueAsString())
            {
                this.name = old.name;
            }

            public string name = "";

            public bool IsParameterSet => (name.Length > 0);
            public static implicit operator CondParameterValue(int val) => new CondParameterValue(val);


        }


        public ConstantEnum.GreaterOrLessEqual gole { set; get; }

        public ConstantEnum.WorldGenPass delayToworldGenPass { get; set; } //always need to be equal or later than Condition worldGenPass value
        public string name { get; }

        public TargetValue targetValue;
        public CondParameterValue ParameterValue1; 
        public CondParameterValue ParameterValue2;
        public CondParameterValue ParameterValue3;
        Func<PropertyElement, Constraint, ValueStringAndOrDouble> condEvalFun;

        public List<string> valueList { get; } = null;

        public Func<Constraint,bool, string> customDisplayFun = null;
        public string SpecialDisplay(bool isFirst){
            if(customDisplayFun==null) return "";
            return customDisplayFun(this,isFirst);
        }

        Func< GreaterOrLessEqual, TargetValue,TargetValue, bool> extraEvalFun = null;
        public bool SpecialEval(GreaterOrLessEqual gole, TargetValue target, TargetValue valueIs){
            if(extraEvalFun==null) return true;
            return extraEvalFun(gole,target,valueIs);
        }

        public bool  Evaluate(PropertyElement gpe)
        {
            
            if (gpe.lastWorldGenPass < delayToworldGenPass) return true;
            
            ValueStringAndOrDouble res = condEvalFun(gpe, this);

            bool extra = SpecialEval(gole,targetValue.valueDouble,res.valueDouble);
            if(!extra) return false;

            if (gole == ConstantEnum.GreaterOrLessEqual.Equal || gole == ConstantEnum.GreaterOrLessEqual.NotEqual)
            {
                bool notEqual = (gole == ConstantEnum.GreaterOrLessEqual.NotEqual);
                if (!targetValue.IsStringEmpty) return res.valueString.Equals(targetValue.valueString)^notEqual;
                return (res.valueDouble == targetValue.valueDouble)^notEqual;
            }
            
            if (gole == ConstantEnum.GreaterOrLessEqual.GreaterOrEqual)
                return res.valueDouble >= targetValue.valueDouble;
            else if (gole == ConstantEnum.GreaterOrLessEqual.LessOrEqual)
                return res.valueDouble <= targetValue.valueDouble;
            
            return false;

        }
        public void SetValue(string newVal, int param = -1)
        {
            if (param < 0)
            {
                targetValue.SetValue(newVal);
            }
            else if (param == 1)
            {
                ParameterValue1.SetValue(newVal);
            }
            else if (param == 2)
            {
                ParameterValue2.SetValue(newVal);
            }
            else if (param == 3)
            {
                ParameterValue3.SetValue(newVal);
            }
        }

        public Constraint(string name,
                Func<PropertyElement, Constraint, ValueStringAndOrDouble> condEvalFun,
                List<string> valueList = null,
                ConstantEnum.WorldGenPass delayToworldGenPass = ConstantEnum.WorldGenPass.PostWorldGen,
                TargetValue targetValue = null,
                CondParameterValue ParameterValue1 = null,
                CondParameterValue ParameterValue2 = null,
                CondParameterValue ParameterValue3 = null,
                Func<Constraint,bool, string> customDisplayFun = null,
                Func< GreaterOrLessEqual, TargetValue,TargetValue, bool> extraEvalFun = null
               
                )
        {
            if (valueList == null) valueList = new List<string>();
            if (targetValue == null) targetValue = new TargetValue("");
            if (ParameterValue1 == null) ParameterValue1 = new CondParameterValue();
            if (ParameterValue2 == null) ParameterValue2 = new CondParameterValue();
            if (ParameterValue3 == null) ParameterValue3 = new CondParameterValue();
            if (condEvalFun == null) condEvalFun = (a, b) => b.targetValue;



            if (targetValue.IsValueEmpty && valueList.Count > 0)
                targetValue.SetValue(valueList[0]);

            if (!targetValue.IsStringEmpty) gole = ConstantEnum.GreaterOrLessEqual.Equal;
            else gole = ConstantEnum.GreaterOrLessEqual.GreaterOrEqual;



            this.targetValue = new TargetValue(targetValue);
            this.condEvalFun = new Func<PropertyElement, Constraint, ValueStringAndOrDouble>(condEvalFun);
            this.valueList = new List<string>(valueList);
            this.delayToworldGenPass = delayToworldGenPass;
            this.name = name;
            this.ParameterValue1 = new CondParameterValue(ParameterValue1);
            this.ParameterValue2 = new CondParameterValue(ParameterValue2);
            this.ParameterValue3 = new CondParameterValue(ParameterValue3);
            this.customDisplayFun = customDisplayFun;
            this.extraEvalFun = extraEvalFun;
            

        }

        public Constraint(Constraint old) :
        this(old == null ? "" : old.name,
            old == null || old.condEvalFun == null ? null : new Func<PropertyElement, Constraint, ValueStringAndOrDouble>(old.condEvalFun),
            old == null || old.valueList == null ? null : new List<string>(old.valueList),
            old == null ? ConstantEnum.WorldGenPass.PostWorldGen : old.delayToworldGenPass,
            old == null || old.targetValue == null ? null : new TargetValue(old.targetValue),
            old == null || old.ParameterValue1 == null ? null : new CondParameterValue(old.ParameterValue1),
            old == null || old.ParameterValue2 == null ? null : new CondParameterValue(old.ParameterValue2),
            old == null || old.ParameterValue3 == null ? null : new CondParameterValue(old.ParameterValue3),
            old == null || old.customDisplayFun == null ? null : old.customDisplayFun,
            old == null || old.extraEvalFun == null ? null : old.extraEvalFun
           
        )
        {
            if (old != null)
            {
                gole = old.gole;
            }
        }

        public override string ToString() => ToString(" ");

        public string ToString(string sep = " ")
        {
            bool isSp = sep.Equals(" ");
            string paramstr = (ParameterValue1.IsParameterSet?    ParameterValue1.name+(isSp?":":"")+sep+ParameterValue1.GetValueAsString():"") +
                              (ParameterValue2.IsParameterSet?sep+ParameterValue2.name+(isSp?":":"")+sep+ParameterValue2.GetValueAsString():"") +
                              (ParameterValue3.IsParameterSet?sep+ParameterValue3.name+(isSp?":":"")+sep+ParameterValue3.GetValueAsString():"");




            return $"{name}{(isSp?":":"")}{sep}{GreaterOrLessEqualClass.ToDString(gole)}{sep}{targetValue.GetValueAsString()}{(paramstr.Length==0?"":(isSp?" with ":sep)+paramstr)}";//####
        }

    }
}