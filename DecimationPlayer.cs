﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Decimation
{
    public class DecimationPlayer : ModPlayer
    {
        public bool closeToEnchantedAnvil = false;
        public bool isJestersQueverEquiped = false;

        public override void ResetEffects()
        {
            closeToEnchantedAnvil = false;
            isJestersQueverEquiped = false;
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (isJestersQueverEquiped && type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileID.JestersArrow;
            return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}
