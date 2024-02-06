using System.Linq;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using System.Reflection;



namespace SuperSeedSearch.WorldGenMod
{
    public static class DataInsertion
    {
        //todo1:better import gatherDataAt
        public static class VanillaVarNames{
            //public static string JungleHut = "jungleHut"; gloabl now
        }
     
        public static void InsertVar<T>(GenPass targetPass, string varname, T setToValue)
        {   //does not work for jungle/temple/terrain
            FieldInfo generationMethod = typeof(PassLegacy).GetField("_method", BindingFlags.Instance | BindingFlags.NonPublic);
            WorldGenLegacyMethod method = (WorldGenLegacyMethod)generationMethod.GetValue(targetPass);            
            var passField = method.Method.DeclaringType?.GetFields
            (
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.Static
            )
            .Single(x => x.Name == varname);
            passField.SetValue(method.Target,(T) setToValue );
        }


    }
}