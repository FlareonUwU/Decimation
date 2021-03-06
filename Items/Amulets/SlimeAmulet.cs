using System;
using System.Collections.Generic;
using Decimation.Buffs;
using Decimation.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Decimation.Items.Amulets
{
    public class SlimeAmulet : Amulet
    {
        public override void SetAmuletDefaults()
        {
            item.width = 24;
            item.height = 24;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.anyIronBar = true;
            recipe.AddIngredient(ItemID.RoyalGel);
            recipe.AddIngredient(ItemID.Gel, 10);
            recipe.AddIngredient(ItemID.Chain, 2);
            recipe.AddIngredient(null, "SlimeBracelet");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override AmuletClasses GetAmuletClass()
        {
            return AmuletClasses.SUMMONER;
        }

        public override List<TooltipLine> GetTooltipLines()
        {
            return new List<TooltipLine>()
            {
                new TooltipLine(mod, "Effect", "Makes slimes friendly")
                {
                   overrideColor = Color.ForestGreen
                },
                new TooltipLine(mod, "Effect", "+3% minion damages")
                {
                    overrideColor = Color.ForestGreen
                },
                 new TooltipLine(mod, "Effect", "+3% minion knockback")
                {
                    overrideColor = Color.ForestGreen
                },
                 new TooltipLine(mod, "Effect", "+4% chances to inflict \"Slimed!\" debuff to ennemies on strikes")
                {
                    overrideColor = Color.ForestGreen
                }
            };
        }

        public override void UpdateAmulet(Player player)
        {
            player.minionDamage *= 0.03f;
            player.minionKB *= 0.03f;
            player.npcTypeNoAggro[1] = true;
            player.npcTypeNoAggro[16] = true;
            player.npcTypeNoAggro[59] = true;
            player.npcTypeNoAggro[71] = true;
            player.npcTypeNoAggro[81] = true;
            player.npcTypeNoAggro[121] = true;
            player.npcTypeNoAggro[122] = true;
            player.npcTypeNoAggro[138] = true;
            player.npcTypeNoAggro[147] = true;
            player.npcTypeNoAggro[183] = true;
            player.npcTypeNoAggro[184] = true;
            player.npcTypeNoAggro[187] = true;
            player.npcTypeNoAggro[204] = true;
            player.npcTypeNoAggro[225] = true;
            player.npcTypeNoAggro[244] = true;
            player.npcTypeNoAggro[302] = true;
            player.npcTypeNoAggro[304] = true;
            player.npcTypeNoAggro[333] = true;
            player.npcTypeNoAggro[334] = true;
            player.npcTypeNoAggro[335] = true;
            player.npcTypeNoAggro[336] = true;
            player.npcTypeNoAggro[535] = true;
            player.npcTypeNoAggro[537] = true;

            DecimationPlayer modPlayer = player.GetModPlayer<DecimationPlayer>();
            modPlayer.amuletsBuff = mod.BuffType<Singed>();
            modPlayer.amuletsBuffChances = 4;
            modPlayer.amuletsBuffTime = 300;
        }
    }
}