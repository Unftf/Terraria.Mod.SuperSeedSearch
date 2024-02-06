using ReLogic.Utilities;
using System.ComponentModel.DataAnnotations;



namespace SuperSeedSearch.ConstantEnum
{
    public enum GreaterOrLessEqual
    {
        [Display(Name = "≤")]
        LessOrEqual = -1,
        [Display(Name = "=")]
        Equal = 0,
        [Display(Name = "≥")]
        GreaterOrEqual = 1,
        [Display(Name = "≠")]
        NotEqual = 2,

    }
   

    public static class GreaterOrLessEqualClass
    {
        public static bool CompareAgoleB(this GreaterOrLessEqual gole, int A, int B){
            switch(gole){
                case GreaterOrLessEqual.LessOrEqual:
                    return A<=B;
                case GreaterOrLessEqual.Equal:
                    return A==B;
                case GreaterOrLessEqual.GreaterOrEqual:
                    return A>=B;
                case GreaterOrLessEqual.NotEqual:
                    return A!=B;
            }
            return false;
        }

        public static string ToDString(this GreaterOrLessEqual val)
        {
            return (val.GetAttribute<DisplayAttribute>()).Name;
        }        
    }

}