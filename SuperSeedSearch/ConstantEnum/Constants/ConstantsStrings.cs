using System.ComponentModel.DataAnnotations;



namespace SuperSeedSearch.ConstantEnum
{

    static class ConstantsStrings
    {
        public const short IsUnsetPropertyValue = -1;
        public const string IsUnsetString = "";

        public const double IsDefaultConditionValue = 0;

        public const string ConditionTypeName = "Type";
        public const string ConditionCountName = "Count";

        public const string AddConditionElementButtonText = "Add new constraint";
        public const string AddConditionButtonText = "Add new condition";

        public const string True = "true";
        public const string False = "false";     

        public const string modpath = @"/SuperSeedSearch";   

        public const string NewClearCondtions = "#a new config / clear config";
        public const string DefaultConfig = "#default config (loads on start)";
        public const string DefaultConfigFileName = "default";

        public const string RandomStartingSeed = "#random";        
        public const string None = "None";        

    }

    public enum TrueFalse 
    {
        [Display(Name = ConstantsStrings.False)]
        False = 0,
        [Display(Name = ConstantsStrings.True)]
        True,
    }
    

}