using System;
using System.Collections.Generic;
using Terraria.ID;


namespace SuperSeedSearch.PropertyCondition
{
    public class ConditionLogger{
        const int maXSize = 16;
        public List<string> loglist {get;private set;}
        public ConditionLogger(){
            loglist = loglist = new List<string>{};
        }
        public void Log(PropertyElement p){
            if(loglist.Count==maXSize) loglist.Add("...");
            if(loglist.Count>maXSize) return;            
            string newL = p.ToString();
            if(newL.Length>0)
                loglist.Add(newL);
        }
        public void Clear(){
            loglist.Clear();
        }
    }


}