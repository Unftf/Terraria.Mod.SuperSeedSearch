
namespace SuperSeedSearch.PropertyCondition
{
    public class ValueStringAndOrDouble
    {
        public ValueStringAndOrDouble(double targetValueDouble)
        {
            SetValue(targetValueDouble);
        }
        public ValueStringAndOrDouble(string targetValueString)
        {

            SetValue(targetValueString);
        }
        public ValueStringAndOrDouble(string targetValueString, double targetValueDouble)
        {

            SetValue(targetValueString, targetValueDouble);
        }
        public ValueStringAndOrDouble()
        {
            Reset();
        }
        public ValueStringAndOrDouble(ValueStringAndOrDouble old)
        {
            valueString = old.valueString;
            valueDouble = old.valueDouble;
        }

        public void SetValue(string valueString)
        {
            double outf = 0;
            if (Helpers.BasicFunctions.TryToParseAnyDoubleNumber(valueString, out outf))
                SetValue(outf);
            else
            {

                this.valueString = valueString;
                this.valueDouble = null;
            }
        }
        public void SetValue(ValueStringAndOrDouble value){
            valueString = value.valueString;
            valueDouble = value.valueDouble;            
        }

        public void SetValue(double valueDouble)
        {

            this.valueString = ConstantEnum.ConstantsStrings.IsUnsetString;
            this.valueDouble = valueDouble;
        }
       public void SetValue(string valueString, double valueDouble)
        {

            this.valueString = valueString;
            this.valueDouble = valueDouble;
        }


        public void Reset()
        {
            valueString = ConstantEnum.ConstantsStrings.IsUnsetString;
            valueDouble = null;

        }
        public string GetValueAsString()
        {
            return IsStringEmpty ? $"{(valueDouble == null ? "" : valueDouble)}" : valueString;
        }

        public bool IsStringEmpty => (valueString.Length == 0);
        public bool IsValueEmpty => (GetValueAsString().Length == 0);


        public string valueString { get; private set; } = ConstantEnum.ConstantsStrings.IsUnsetString;
        public double? valueDouble { get; private set; } = null;

        public static implicit operator ValueStringAndOrDouble(string val) => new ValueStringAndOrDouble(val);
        public static implicit operator ValueStringAndOrDouble(double val) => new ValueStringAndOrDouble(val);
        public static implicit operator ValueStringAndOrDouble(float val) => new ValueStringAndOrDouble((double)val);
        public static implicit operator ValueStringAndOrDouble(int val) => new ValueStringAndOrDouble((double)val);

        public static implicit operator string(ValueStringAndOrDouble val) => val.valueString;
        public static implicit operator double?(ValueStringAndOrDouble val) => val.valueDouble;



    }



}