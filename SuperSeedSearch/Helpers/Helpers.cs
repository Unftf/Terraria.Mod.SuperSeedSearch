using Terraria.UI;
using Terraria.WorldBuilding;
using SuperSeedSearch.ConstantEnum;
using System;
using Terraria;
using Terraria.ObjectData;

namespace SuperSeedSearch.Helpers
{

    public static class Helpers
    {


        public static void writeDebugFile(string content, bool newFile = false, string name = "")
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter( (name.Length==0?Terraria.Main.worldPathName.Substring(0, Terraria.Main.worldPathName.Length - 4):name) + "_debug.txt", !newFile))
            {
                file.WriteLine(content);
            }
        }


        public static void SetUIBorderToZero(this UIElement uiel){
                    uiel.SetPadding(0);
                    uiel.HAlign = 0f;
                    uiel.VAlign = 0f;                                        
                    uiel.MarginBottom = 0;
                    uiel.MarginTop = 0;
                    uiel.MarginLeft = 0;
                    uiel.MarginRight = 0;

        }

        public static string EncodeNameString(string name,  int currentSeed, string currentSeedAsText, int seedsDoneCount, WorldSize worldSize, Difficulty difficulty, EvilType evilType, bool ignoreZeroCount = false){
            const int maxNameSize = 27;                
            if(name.Equals( EnumHelper<WorldName>.GetNameOFEnumVariable(WorldName.seedOnly))) return currentSeed.ToString();            
            string seedFullFront = ((int)worldSize+1)+"."+((int)difficulty+1)+"."+((int)(evilType+1)/2+1).ToString()+".";
            int l = name.Length;
            bool startsWithHash = false;
            if(l>1){
                startsWithHash = name[0].Equals('#');
            }else{
                return name;
            }
            string[] parts = name.Split('#');
            string rname = startsWithHash?"":parts[0];
            for(int i=(startsWithHash?0:1);i<parts.Length;i++){
                if( parts[i].Length<1) continue;
                string ps = "";                
                int lz = 0; int lzt = -1;
                for(int j=0;j<2 && j<parts[i].Length;j++){
                    char fc= parts[i][j];
                    if( fc=='S') ps = seedFullFront+currentSeed.ToString().PadLeft(lzt<0?10:lz,'0')+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( fc=='T' || fc=='t') ps = currentSeedAsText+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( fc=='E' || fc=='e') ps = (evilType==EvilType.crimson?"crim":"corr")+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( fc=='D' || fc=='d') ps = (GenVars.dungeonSide <0?"l":"r")+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( fc=='Z' || fc=='z') ps = ( worldSize switch { WorldSize.small => "S", WorldSize.medium => "M", _ or WorldSize.large => "L" })+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( fc=='s') ps = currentSeed.ToString().PadLeft(lz, '0')+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( fc=='c') ps = (seedsDoneCount==0 && ignoreZeroCount? "": seedsDoneCount.ToString().PadLeft(lz, '0') )+ (parts[i].Length>j+1?parts[i].Substring(j+1):"");
                    else if( lzt<0 &&  (lzt = "0123456789".IndexOf(fc))>0 ) lz = lzt;
                    else ps += "#"+parts[i];
                    if(ps.Length>0)     break;
                }
                rname += ps;
            }
            return rname.Length>0?(rname.Length>maxNameSize?rname.Substring(0,maxNameSize):rname):name;
        }


        public static Tuple<int,int,int,int> GetObjectOrigin(int posx, int posy){

            TileObjectData tdata = TileObjectData.GetTileData(Main.tile[posx,posy].TileType,0);
            if(tdata==null) return null;
            int tileSizeX = tdata.Width;
            int tileSizeY = tdata.Height;            
            if(tileSizeX==1 && tileSizeY==1) return new Tuple<int, int,int,int>(posx, posy,0,0);

            int fsx = tdata.CoordinateFullWidth;//fullObjectSizeX
            int fsy = tdata.CoordinateFullHeight;
           

            int fsxpt = fsx/tileSizeX;
            int fsypt = fsy/tileSizeY;
            
            int fxp = Main.tile[posx,posy].TileFrameX%fsx;
            int fyp = Main.tile[posx,posy].TileFrameY%fsy;
            int frameStartX = Main.tile[posx,posy].TileFrameX-fxp;
            int frameStartY = Main.tile[posx,posy].TileFrameY-fyp;
            
            int xs = posx-fxp/fsxpt;
            int ys = posy-fyp/fsypt;

            return new Tuple<int, int,int,int>(xs, ys, frameStartX, frameStartY );

        }
        

    }

}