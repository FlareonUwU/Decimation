﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Decimation.Items.Accessories
{
    class GraniteLinedTunic : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FamiliarShirt);
            recipe.AddIngredient(ItemID.Granite, 16);
            recipe.AddIngredient(ItemID.BlackThread, 10);
            recipe.AddIngredient(ItemID.Chain, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FamiliarShirt);
            recipe.AddIngredient(ItemID.Granite, 16);
            recipe.AddIngredient(ItemID.GreenThread, 10);
            recipe.AddIngredient(ItemID.Chain, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FamiliarShirt);
            recipe.AddIngredient(ItemID.Granite, 16);
            recipe.AddIngredient(ItemID.PinkThread, 10);
            recipe.AddIngredient(ItemID.Chain, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
