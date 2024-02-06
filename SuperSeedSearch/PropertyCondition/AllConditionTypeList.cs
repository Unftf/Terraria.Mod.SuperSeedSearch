using System.Collections.Generic;
using System.Linq;



namespace SuperSeedSearch.PropertyCondition
{
    public class AllConditionTypeList
    {
        public List<ConstantEnum.ConditionType> conditionTypesIncluded { get; private set; } = null;//only after initialization
        //public string header { get; private set; } = "";

        public List<ConditionTypeList> typeConditionList = null;

        public AllConditionTypeList(List<ConditionTypeList> typeConditionList = null)
        {
            
            conditionTypesIncluded = new List<ConstantEnum.ConditionType>();

            if(typeConditionList!=null){
                foreach (var item in typeConditionList)
                {
                    conditionTypesIncluded.Add(item.conditionType);
                }
            }
            conditionTypesIncluded = conditionTypesIncluded.Distinct().ToList();
            conditionTypesIncluded.Sort();
            
            this.typeConditionList = typeConditionList;
             

        }   

        public bool HasElements() => typeConditionList == null || typeConditionList.Count == 0;

    }
}