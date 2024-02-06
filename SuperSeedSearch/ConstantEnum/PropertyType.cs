namespace SuperSeedSearch.ConstantEnum
{
    public enum ConditionType 
        {
            Unknown = 0,
            Tile,
            Chest,
            RNGnumber,     
            WorldStyleTypeProp,
            Object,
            PointOfInterest,
            Biome
               
        }

    //PropertyConditionType an contain property of different type. Those are the different types for analyzing them. e.g. Objects are analyzed in Tile analyzation. PoI & Style are unique.
    public enum PropertyType 
        {
            Unknown = 0,
            Tile,
            Chest,
            RNGnumber,
            Unique
               
        }

}