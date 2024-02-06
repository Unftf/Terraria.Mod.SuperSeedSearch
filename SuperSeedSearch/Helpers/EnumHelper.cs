using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;

//from  //https://stackoverflow.com/questions/268084/creating-a-constant-dictionary-in-c-sharp
//int i = (int)Enum.Parse(typeof(T), "elementAsString");
//var z = (int)EnumHelper<T>.GetValueFromName("elementAsString or displayname");

namespace SuperSeedSearch.Helpers
{
    public static class EnumHelper<T> where T : struct, System.Enum
    {
        public static T GetValueFromName(string name){
            return GetValueFromName(name, false, out _ );
        }

        public static T GetValueFromName(string name, bool returnFirstIfInvalid, out string ValidName)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            ValidName = "";
            string firstStr = "";

            object first = null;
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (returnFirstIfInvalid && first == null && field !=null){ first = (T)field.GetValue(null); firstStr =attribute.Name;}

                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (T)field.GetValue(null);
                }
            }

            if (returnFirstIfInvalid && first != null) {ValidName=firstStr ;return (T)first;}

            throw new ArgumentOutOfRangeException("name");
        }



        public static T? Try2FindValue4Name(string name)
        {
            name = name.ToLower();
            List<Tuple<string, T>> matches = new List<Tuple<string, T>>();

            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (name.Contains(attribute.Name.ToLower()))
                    {
                        matches.Add(new Tuple<string, T>(attribute.Name, (T)field.GetValue(null)));
                    }
                }
                if (name.Contains(field.Name.ToLower()))
                {
                    matches.Add(new Tuple<string, T>(field.Name, (T)field.GetValue(null)));
                }
            }

            T? longest = null;
            int maxlen = 0;

            foreach (var item in matches)
            {
                if (item.Item1.Length > maxlen)
                    longest = item.Item2;
            }


            return longest;
        }



        public static List<string> GetAllEnumVariablesAsString()
        {
            List<string> members = new List<string>();
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {                    
                    members.Add(attribute.Name);
                }
                else if (field.FieldType.Equals(type)) {//also contains int32 value____
                    string[] lastOne = field.ToString().Split('.');
                    lastOne = lastOne[lastOne.Length-1].Split(' ');
                    members.Add(lastOne[lastOne.Length-1]);
                }

            }
            return members;
        }

        public static string GetNameOFEnumVariable(T val) //nameof, ToString("g");->name, ToString("d");->"decimal
        {
            string asString = val.ToString();
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            var field = type.GetField(asString);
            var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DisplayAttribute)) as DisplayAttribute;
            if (attribute != null) return attribute.Name;
            return asString;
        }


    }

    

}