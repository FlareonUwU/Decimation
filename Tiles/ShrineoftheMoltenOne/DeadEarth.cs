﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Decimation.Tiles.ShrineoftheMoltenOne
{
    class DeadEarth : ModTile
    {

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = DustID.Sandnado;
            drop = ModContent.ItemType<Items.Placeable.ShrineoftheMoltenOne.DeadEarth>();
            AddMapEntry(new Color(33, 28, 25));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return DecimationWorld.downedArachnus;
        }
    }
}
