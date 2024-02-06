using System;
using Terraria.ID;
using SuperSeedSearch.ConstantEnum;


namespace SuperSeedSearch.PropertyCondition
{
    public class PropertyElement
    {
        //TODo somethin like hasmap for world custom world prpertie -> static world Props

        public ConstantEnum.PropertyType CondType = ConstantEnum.PropertyType.Unknown;
        public ConstantEnum.WorldGenPass lastWorldGenPass = ConstantEnum.WorldGenPass.PostWorldGen;
        public string name =  ConstantEnum.ConstantsStrings.IsUnsetString;
        // todo1: seperate values which are compared and those which are just read/set
        public short posX = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue, posY = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue, chestIndex = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue;
        public short IDTile = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue,
                frameX = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue, frameY = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue, IDWall = ConstantEnum.ConstantsStrings.IsUnsetPropertyValue;                 
                 
        public bool? isActive = null;
        public bool? isChestLocked = null;

        public Func<PropertyElement, bool> matchFun = null;


        public bool ComputeMatchFun(PropertyElement gpe)
        {
            if (matchFun == null) return true;
            else return matchFun(gpe);
        }



        public virtual bool HasSetProperitesOf(PropertyElement gpe)
        {
            PropertyElement thispe = this;
            return gpe.SetProperitesAlsoIn(thispe);
            /*
            if (gpe.CondType != CondType) return false;

            if ((gpe.isActive != null && isActive != null && gpe.isActive != isActive) || (gpe.isActive != null && isActive == null)) return false;

            if (gpe.IDTile != ConstantEnum.Constants.IsUnsetPropertyValue && gpe.IDTile != IDTile) return false;


            if (gpe.frameX != ConstantEnum.Constants.IsUnsetPropertyValue && gpe.frameX != frameX) return false;

            if (gpe.frameY != ConstantEnum.Constants.IsUnsetPropertyValue && gpe.frameY != frameY) return false;

            if (gpe.IDWall != ConstantEnum.Constants.IsUnsetPropertyValue && gpe.IDWall != IDWall) return false;

            if (gpe.posX != ConstantEnum.Constants.IsUnsetPropertyValue && gpe.posX != posX) return false;

            if (gpe.posY != ConstantEnum.Constants.IsUnsetPropertyValue && gpe.posY != posY) return false;

            if ((gpe.isChestLocked != null && isChestLocked != null && gpe.isChestLocked != isChestLocked) || (gpe.isChestLocked != null && isChestLocked == null)) return false;

            if (!gpe.name.Equals(ConstantEnum.Constants.IsUnsetString) && !gpe.name.Equals(name)) return false;

            bool matchFunb = gpe.ComputeMatchFun(this);

            return matchFunb;*/
        }
        public virtual bool SetProperitesAlsoIn(PropertyElement gpe)
        {

            if (CondType != gpe.CondType) return false;

            if ((isActive != null && gpe.isActive != null && isActive != gpe.isActive) || (isActive != null && gpe.isActive == null)) return false;

            if (IDTile != ConstantEnum.ConstantsStrings.IsUnsetPropertyValue && IDTile != gpe.IDTile) return false;


            if (frameX != ConstantEnum.ConstantsStrings.IsUnsetPropertyValue && frameX != gpe.frameX) return false;

            if (frameY != ConstantEnum.ConstantsStrings.IsUnsetPropertyValue && frameY != gpe.frameY) return false;

            if (IDWall != ConstantEnum.ConstantsStrings.IsUnsetPropertyValue && IDWall != gpe.IDWall) return false;

            if (posX != ConstantEnum.ConstantsStrings.IsUnsetPropertyValue && posX != gpe.posX) return false;

            if (posY != ConstantEnum.ConstantsStrings.IsUnsetPropertyValue && posY != gpe.posY) return false;

            if ((isChestLocked != null && gpe.isChestLocked != null && isChestLocked != gpe.isChestLocked) || (isChestLocked != null && gpe.isChestLocked == null)) return false;

            if (!name.Equals(ConstantEnum.ConstantsStrings.IsUnsetString) && !name.Equals(gpe.name)) return false;

            bool matchFunb = ComputeMatchFun(gpe);

            return matchFunb;


            //return gpe.HasSetProperitesOf(this);            
        }

        public override string ToString()
        {
            //string CondType = this.CondType==ConstantEnum.PropertyType.Unknown?"":Helpers.EnumHelper<PropertyType>.GetNameOFEnumVariable(this.CondType)+":";

            string name =  this.name.Equals(ConstantEnum.ConstantsStrings.IsUnsetString)?"":this.name;

            string posX = this.posX==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue?"":this.posX.ToString();
            string posY = this.posY==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue?"":this.posY.ToString();
            string pos  = posX.Length>0 || posY.Length>0? "at pos ("+posX+","+posY+")":"";
            //string chestIndex = this.chestIndex==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue?"":this.chestIndex.ToString();
            string IDTile = this.IDTile==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue || this.isActive==null || this.isActive==false?"":TileID.Search.GetName(this.IDTile);
            string frameX = this.frameX==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue?"":this.frameX.ToString();
            string frameY = this.frameY==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue?"":this.frameY.ToString();
            string frames  = frameX.Length>0 || frameY.Length>0? "frames ("+frameX+","+frameY+")":"";
            string hold = "";
            string IDWall = this.IDWall==ConstantEnum.ConstantsStrings.IsUnsetPropertyValue?"": (hold = WallID.Search.GetName(this.IDWall)).Equals("None")?"":"with wall "+ hold;
            //string isActive = this.isActive==null?"": this.isActive == true?"acitve":"not active";
            string isChestLocked = this.isChestLocked==null?"": this.isChestLocked == true?"locked":"";

            string all = $"{name} {IDTile} {pos} {frames} {IDWall} {isChestLocked}";            
            return all.Replace(" ","").Length==0?"":all;
        }

    }
}