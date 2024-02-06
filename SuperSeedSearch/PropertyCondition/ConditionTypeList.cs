using System.Collections.Generic;


namespace SuperSeedSearch.PropertyCondition
{
    public class ConditionTypeList
    {
        public ConstantEnum.ConditionType conditionType { get; private set; } = ConstantEnum.ConditionType.Unknown;
        public string header { get; private set; } = "";

        public List<Condition> suitableTypeConditions = null;
        public bool canBeCounted {get;private set;} = false ;

        public ConditionTypeList(ConstantEnum.ConditionType conditionType, List<Condition> suitableTypeConditions = null, bool canBeCounted = false)
        {
            this.header = ConstantEnum.ConditionTypeHeader.conditionTypeHeader[conditionType];
            this.conditionType = conditionType;
            this.suitableTypeConditions = suitableTypeConditions;
            this.canBeCounted = canBeCounted;
        }




    }
}