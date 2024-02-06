using Terraria;
using System;
using System.Collections.Generic;



namespace SuperSeedSearch.Storage
{ 

    public class PassMemory
    {  
     
        internal interface SeedMemoryInter{
            public abstract void Delete(); 
            public abstract void Reset();
            public abstract void Disable();
            
        }

        public class SeedMemoryBase<T> :SeedMemoryInter {
            public bool Inuse = false;
            public T[] data {get; private set;}= null;

            //public SeedMemoryBase(int size,T baseVal){
            public SeedMemoryBase(Func<int> GetSize){
                this.GetSize = GetSize;          
                      
            }
            Func<int> GetSize = ()=>0;
            public int size => GetSize();
            //public T baseVal;            
                        
            public T Get(int i) {if(data==null || !Inuse)return default(T); return data[i];}
            public void Set(int i, T v) {if(data==null){Init();}else if(!Inuse){ SetBackToDefault();} data[i] = v;}            
            public void Init(){                                
                data = new T[size];
                Inuse = true;
                wasUsedDuringSearch.Add(this);                     
                activeInCurPass.Add(this);                
            }
            public void Delete(){                   
                data = null;             
                Inuse = false;                                   
            }
            public void SetBackToDefault(){                   
                //Array.Fill(data, baseVal);
                Array.Clear(data);
                Inuse = true;       
                activeInCurPass.Add(this);                
            }
            public void Reset(){
                Array.Clear(data);                
                Inuse = false;                
            }
            public void Disable(){                
                Inuse = false;                
            }
        }
        internal static List<SeedMemoryInter> activeInCurPass = new List<SeedMemoryInter>();
        internal static List<SeedMemoryInter> wasUsedDuringSearch = new List<SeedMemoryInter>();

        public static void Clear()
        {
            foreach(var i in wasUsedDuringSearch) i.Delete();
            wasUsedDuringSearch.Clear();                   
        }
        public static void Reset()
        {
            foreach(var i in activeInCurPass) i.Reset();
            activeInCurPass.Clear();                    
        }
        public static void Disable()
        {            
            foreach(var i in activeInCurPass) i.Disable();
            activeInCurPass.Clear();     
                
        }


    }

}