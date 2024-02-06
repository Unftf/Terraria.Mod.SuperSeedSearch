using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using SuperSeedSearch.ConstantEnum;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using System.Reflection;
using SuperSeedSearch.Storage;
using System.ComponentModel.DataAnnotations;   

namespace SuperSeedSearch.PropertyCondition
{
    public static class HasBGWallNearbyList
    { //todo maybe add some count
        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasWallNearby = (p, c) =>
        {   
            string which = c.targetValue.GetValueAsString();
            int radius = (int)c.ParameterValue1.valueDouble;            
            if(LookUpBGWalls.ContainsKey(which) && radius>=0)                                        
            {
                if(radius<11  &&  CountNearWallTypes(p.posX,p.posY, radius,1, LookUpBGWalls[which] )>0){
                    return c.targetValue.GetValueAsString();
                }else if(radius>10 && CountNearWallTypesOnlyInRadius(p.posX,p.posY, radius,1, LookUpBGWalls[which] )>0){
                    return c.targetValue.GetValueAsString();
                }
            }
            return "not " + c.targetValue.GetValueAsString();
        };
        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasWallDifferentThanNearby = (p, c) =>
        {   
            string which = c.targetValue.GetValueAsString();
            int radius = (int)c.ParameterValue1.valueDouble;            
            if(LookUpBGWalls.ContainsKey(which) && radius>=0)                                        
            {
                if(radius<11  &&  CountNearDifferentWallTypes(p.posX,p.posY, radius,1, LookUpBGWalls[which] )>0){
                    return c.targetValue.GetValueAsString();
                }else if(radius>10 && CountNearDifferentWallTypesOnlyInRadius(p.posX,p.posY, radius,1, LookUpBGWalls[which] )>0){
                    return c.targetValue.GetValueAsString();
                }
            }
            return "not " + c.targetValue.GetValueAsString();
        };
        //todo generalize
        public static int CountNearDifferentWallTypes(int X, int Y, int radius, int cap, params int[] walltypes ){
            if (walltypes.Length == 0) return 0;
			int fromX = X - radius;
			int toX = X + radius;
			int fromY = Y - radius;
			int toY = Y + radius;
            fromX = Utils.Clamp(fromX, 0, Main.maxTilesX - 1);
			toX = Utils.Clamp(toX, 0, Main.maxTilesX - 1);
			fromY = Utils.Clamp(fromY, 0, Main.maxTilesY - 1);
			toY = Utils.Clamp(toY, 0, Main.maxTilesY - 1);                        
            int step = 1;
            int maxShift = Math.Max(toX-fromX,toY-fromY );            
            if(maxShift>100) step = maxShift/100;
            
            int different = 0;
            for(int x = fromX; x <= toX; x+=step){
                for(int y = fromY; y <= toY; y+=step){
                    int w;
                    for(w = 0; w < walltypes.Length; w++){
                        if(walltypes[w] == (int)Main.tile[x, y].WallType) break;                    
                    }
                    if(w==walltypes.Length){
                        different++; if(different>=cap) return different;
                    }
                }
            }
            return different;
        }
        public static int CountNearDifferentWallTypesOnlyInRadius(int X, int Y, int radius, int cap, params int[] walltypes ){
            if (walltypes.Length == 0) return 0;
			int fromX = X - radius;
			int toX = X + radius;
			int fromY = Y - radius;
			int toY = Y + radius;
            fromX = Utils.Clamp(fromX, 0, Main.maxTilesX - 1);
			toX = Utils.Clamp(toX, 0, Main.maxTilesX - 1);
			fromY = Utils.Clamp(fromY, 0, Main.maxTilesY - 1);
			toY = Utils.Clamp(toY, 0, Main.maxTilesY - 1);
            
            Func<int,int,bool> check = (x,y) => { return !walltypes.Contains(Main.tile[x, y].WallType); };
            int step = 1;
            int maxShift = Math.Max(toX-fromX,toY-fromY );            
            if(maxShift>100) step = maxShift/100;

            int different = 0;
            for(int x = fromX; x <= toX; x+=step){
                if(check(x, fromY)){different++; if(different>=cap) return different;}
                if(check(x, toY  )){different++; if(different>=cap) return different;}
            }
            for(int y = fromY; y <= toY; y+=step){
                if(check(fromX, y)){different++; if(different>=cap) return different;}
                if(check(toX,   y)){different++; if(different>=cap) return different;}
            }

            return different;
        }

        public static int CountNearWallTypes(int X, int Y, int radius, int cap, params int[] walltypes ){
            if (walltypes.Length == 0) return 0;
			int fromX = X - radius;
			int toX = X + radius;
			int fromY = Y - radius;
			int toY = Y + radius;
            fromX = Utils.Clamp(fromX, 0, Main.maxTilesX - 1);
			toX = Utils.Clamp(toX, 0, Main.maxTilesX - 1);
			fromY = Utils.Clamp(fromY, 0, Main.maxTilesY - 1);
			toY = Utils.Clamp(toY, 0, Main.maxTilesY - 1);                        
            int step = 1;
            int maxShift = Math.Max(toX-fromX,toY-fromY );            
            if(maxShift>100) step = maxShift/100;

            int found = 0;
            for(int x = fromX; x <= toX; x+=step){
                for(int y = fromY; y <= toY; y+=step){
                    for(int w = 0; w < walltypes.Length; w++){
                        if(walltypes[w] == Main.tile[x, y].WallType){found++; if(found>=cap) return found;}                    
                    }
                }
            }
            return found;
        }


        public static int CountNearWallTypesOnlyInRadius(int X, int Y, int radius, int cap, params int[] walltypes ){
            if (walltypes.Length == 0) return 0;
			int fromX = X - radius;
			int toX = X + radius;
			int fromY = Y - radius;
			int toY = Y + radius;
            fromX = Utils.Clamp(fromX, 0, Main.maxTilesX - 1);
			toX = Utils.Clamp(toX, 0, Main.maxTilesX - 1);
			fromY = Utils.Clamp(fromY, 0, Main.maxTilesY - 1);
			toY = Utils.Clamp(toY, 0, Main.maxTilesY - 1);
            
            Func<int,int,bool> check = (x,y) => { return walltypes.Contains(Main.tile[x, y].WallType); };
            int step = 1;
            int maxShift = Math.Max(toX-fromX,toY-fromY );            
            if(maxShift>100) step = maxShift/100;

            int found = 0;
            for(int x = fromX; x <= toX; x+=step){
                if(check(x, fromY)){found++; if(found>=cap) return found;}
                if(check(x, toY  )){found++; if(found>=cap) return found;}
            }
            for(int y = fromY; y <= toY; y+=step){
                if(check(fromX, y)){found++; if(found>=cap) return found;}
                if(check(toX,   y)){found++; if(found>=cap) return found;}
            }

            return found;
        }

        public static Dictionary<string, int[]> LookUpBGWalls = new Dictionary<string, int[]>();

        public static List<String> ContentList = GenList();
        
        private static List<String> GenList(){
            List<String> content = new List<String>();
            FieldInfo[] fi =  typeof(BiomeWalls).GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);  
            foreach(var v in fi){
                int[] list = (int[])v.GetValue(null);
                var attribute = Attribute.GetCustomAttribute(v, typeof(DisplayAttribute)) as DisplayAttribute;
                string name = attribute!=null? attribute.Name: v.Name;
                content.Add(name);
                LookUpBGWalls.Add(name,list);
            }
            return content;
        }           

    }
}