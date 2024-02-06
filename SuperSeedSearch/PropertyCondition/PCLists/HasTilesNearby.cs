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
    public static class HasTilesNearbyList
    {
        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesNearby = (p, c) =>
        {   
            string which = c.targetValue.GetValueAsString();
            int radius = (int)c.ParameterValue1.valueDouble;            
            if(LookUpTiles.ContainsKey(which) && radius>=0)                                        
            {
                if(radius<11  &&  WorldGen.CountNearBlocksTypes(p.posX,p.posY, radius,1, LookUpTiles[which] )>0){
                    return c.targetValue.GetValueAsString();
                }else if(radius>10 && CountNearBlocksTypesOnlyInRadius(p.posX,p.posY, (uint)radius,1, LookUpTiles[which] )>0){
                    return c.targetValue.GetValueAsString();
                }
            }
            return "not " + c.targetValue.GetValueAsString();
        };

        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesAbove = (p, c) =>
        {   
            int dist = (int)c.ParameterValue1.valueDouble;                        
            return HasTilesInDist(p,c,0,0,dist,0);
        };
        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesAboveBelow = (p, c) =>
        {   
            int dist = (int)c.ParameterValue1.valueDouble;                        
            return HasTilesInDist(p,c,0,0,dist,dist);
        };
                public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesBelow = (p, c) =>
        {   
            int dist = (int)c.ParameterValue1.valueDouble;                        
            return HasTilesInDist(p,c,0,0,0,dist);
        };


        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesLeft = (p, c) =>
        {               
            int dist = (int)c.ParameterValue1.valueDouble;                        
            return HasTilesInDist(p,c,dist,0,0,0);
        };
        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesRight = (p, c) =>
        {               
            int dist = (int)c.ParameterValue1.valueDouble;                        
            return HasTilesInDist(p,c,0,dist,0,0);
        };

        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasTilesLeftRight = (p, c) =>
        {               
            int dist = (int)c.ParameterValue1.valueDouble;                        
            return HasTilesInDist(p,c,dist,dist,0,0);
        };


        private static Func<PropertyElement, Constraint, int, int, int, int, ValueStringAndOrDouble> HasTilesInDist = (p, c, xm, xp, ym, yp) =>
        {   
            string which = c.targetValue.GetValueAsString();                      
            if(LookUpTiles.ContainsKey(which) && xm>=0 && xp>=0 && ym>=0 && yp>=0)                                        
            {
                if(CountNearBlocksTypesOnlyInDist(p.posX,p.posY,(uint)xm,(uint)xp, (uint)ym, (uint)yp ,1, LookUpTiles[which] )>0){
                    return which;
                }
            }
            return "not " + which;
        };






        //todo generalize with background wall
        public static int CountNearBlocksTypesOnlyInRadius(int X, int Y, uint radius, int cap, params int[] tiletypes ){
            return CountNearBlocksTypesOnlyInDist(X,Y,radius,radius,radius,radius,cap, tiletypes );
        }        
        public static int CountNearBlocksTypesOnlyInDist(int X, int Y, uint Xm, uint Xp, uint Ym, uint Yp, int cap, params int[] tiletypes ){
            if (tiletypes.Length == 0) return 0;
			int fromX = X - (int)Xm;
			int toX = X + (int)Xp;
			int fromY = Y - (int)Ym;
			int toY = Y + (int)Yp;
            fromX = Utils.Clamp(fromX, 0, Main.maxTilesX - 1);
			toX = Utils.Clamp(toX, 0, Main.maxTilesX - 1);
			fromY = Utils.Clamp(fromY, 0, Main.maxTilesY - 1);
			toY = Utils.Clamp(toY, 0, Main.maxTilesY - 1);
            
            Func<int,int,bool> check = (x,y) => { if (!Main.tile[x, y].HasTile) return false; return tiletypes.Contains(Main.tile[x, y].TileType); };
            int step = 1;
            int maxShift = Math.Max(toX-fromX,toY-fromY );            
            if(maxShift>100) step = maxShift/100;

            int found = 0;            
            for(int x = fromX; x <= toX; x+=step){
                if(check(x, fromY)){found++; if(found>=cap) return found;}                                
            }
            for(int y = fromY+1; y < toY; y+=step){
                if(check(fromX, y)){found++; if(found>=cap) return found;}
            }
            if(fromX!=toX)
            for(int y = fromY+1; y < toY; y+=step){
                if(check(toX, y)){found++; if(found>=cap) return found;}
            }
            if(fromY!=toY)
            for(int x = fromX; x <= toX; x+=step){
                if(check(x, toY)){found++; if(found>=cap) return found;}                                
            }

            return found;
        }




        public static Dictionary<string, int[]> LookUpTiles = new Dictionary<string, int[]>();

        public static List<String> ContentList = GenList();
        
        private static List<String> GenList(){
            List<String> content = new List<String>();
            FieldInfo[] fi =  typeof(BiomeTiles).GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);  
            foreach(var v in fi){
                int[] list = (int[])v.GetValue(null);
                var attribute = Attribute.GetCustomAttribute(v, typeof(DisplayAttribute)) as DisplayAttribute;
                string name = attribute!=null? attribute.Name: v.Name;
                content.Add(name);
                LookUpTiles.Add(name,list);
            }
            return content;
        }           

    }
}