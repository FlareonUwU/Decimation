﻿using Decimation.Buffs;
using Decimation.Buffs.Buffs;
using Decimation.Buffs.Debuffs;
using Decimation.Items.Amulets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Decimation
{
    public class DecimationPlayer : ModPlayer
    {
        public bool closeToEnchantedAnvil = false;
        public bool jestersQuiverEquiped = false;
        public bool deadeyesQuiverEquipped = false;
        public bool endlessPouchofLifeEquipped = false;
        public bool graniteLinedTunicEquipped = false;

        public bool isInCombat = false;
        public uint combatTime = 0;

        // Amulet slot
        public Item amuletSlotItem;

        // Slimy Feet buff
        public bool wasJumping = false;
        public float lastJumpBoost = 0;

        // Scarab Endurance buff
        public byte scarabEnduranceBuffTimeCounter = 0;
        public byte scarabCounter = 0;
        public int[] scarabs = new int[3];
        public int oldStatDefense = 0;
        public byte lastHitCounter = 0;
        public bool wasHurt = false;

        // Scarab shield
        public int solarCounter = 0;

        // amulets
        public int amuletsSinged = 0;
        public int amuletBuffTime = 0;
        public uint enchantedHeartDropTime = 0;

        public override void Initialize()
        {
            amuletSlotItem = new Item();
            amuletSlotItem.SetDefaults(0, true);
        }

        public override void ResetEffects()
        {
            closeToEnchantedAnvil = false;
            jestersQuiverEquiped = false;
            deadeyesQuiverEquipped = false;
            endlessPouchofLifeEquipped = false;
            graniteLinedTunicEquipped = false;

            if (combatTime > 360)
            {
                combatTime = 0;
                enchantedHeartDropTime = 0;
                isInCombat = false;
            }

            amuletsSinged = 0;
            amuletBuffTime = 0;

            if (!player.HasBuff(mod.BuffType<SlimyFeet>())) lastJumpBoost = 0;
            if (!player.HasBuff(mod.BuffType<ScarabEndurance>()))
            {
                scarabEnduranceBuffTimeCounter = 0;
                scarabCounter = 0;
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound() {
                {"amuletSlotItem", ItemIO.Save(amuletSlotItem) }
            };
        }

        public override void Load(TagCompound tag)
        {
            amuletSlotItem = ItemIO.Load(tag.GetCompound("amuletSlotItem"));

            // Load to slot
            Decimation.amuletSlotState.LoadItem(amuletSlotItem);
        }

        // FIND AN ALTERNATIVE! THIS METHOD DOESN'T GET CALLED WITH ALL THE WEAPONS
        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile toCheck = Main.projectile[type];

            // Jester's Quiver
            if (jestersQuiverEquiped && toCheck.arrow)
                type = ProjectileID.JestersArrow;

            // Endless Pouch of Life
            if (endlessPouchofLifeEquipped && References.bullets.Contains(type))
                type = ProjectileID.ChlorophyteBullet;

            // Deadeye's Quiver
            if (deadeyesQuiverEquipped && (toCheck.arrow || References.bullets.Contains(type)))
            {
                if (toCheck.arrow)
                    type = ProjectileID.IchorArrow;
                else
                    type = ProjectileID.ChlorophyteBullet;

                speedX *= 1.15f;
                speedY *= 1.15f;
            }

            // Frost Amulet
            if (amuletSlotItem.type == mod.ItemType<FrostAmulet>() && toCheck.arrow)
            {
                speedX *= 1.03f;
                speedY *= 1.03f;
            }

            return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            if (deadeyesQuiverEquipped && (ammo.ammo == AmmoID.Arrow || ammo.ammo == AmmoID.Bullet) && Main.rand.Next(20) > 3)
                return false;
            if (endlessPouchofLifeEquipped && ammo.ammo == AmmoID.Bullet)
                return false;
            if (amuletSlotItem.type == mod.ItemType<FrostAmulet>() && (ammo.ammo == AmmoID.Arrow) && Main.rand.Next(50) == 0)
                return false;

            return base.ConsumeAmmo(weapon, ammo);
        }

        public override void UpdateVanityAccessories()
        {
            Decimation.amuletSlotState.UpdateAmulet();

            base.UpdateVanityAccessories();
        }

        public override void PostUpdate()
        {
            oldStatDefense = player.statDefense;

            if (lastHitCounter >= 60)
            {
                lastHitCounter = 0;
                wasHurt = false;
            }

            if (wasHurt)
                lastHitCounter++;

            if (isInCombat)
            {
                combatTime++;
                enchantedHeartDropTime++;
            }

            base.PostUpdate();
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (target.HasBuff(mod.BuffType<ScarabEndurance>())) player.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            isInCombat = true;
            combatTime = 0;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            isInCombat = true;
            combatTime = 0;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (player.HasBuff(mod.BuffType<ScarabEndurance>()) && scarabCounter > 0 && lastHitCounter == 0 && !wasHurt)
            {
                Main.projectile[scarabs[scarabCounter - 1]].Kill();
                scarabCounter--;
                wasHurt = true;
            }

            if (amuletsSinged != 0 && amuletBuffTime != 0)
                if (Main.rand.Next(amuletsSinged, 100) < amuletsSinged)
                    npc.AddBuff(mod.BuffType<Slimed>(), amuletBuffTime);

            if (graniteLinedTunicEquipped)
            {
                player.statLife += (int)(damage * 0.04f);

                if (Main.rand.Next(3, 100) < 3)
                    npc.AddBuff(BuffID.Confused, 600);
            }

            foreach (Player otherPlayer in Main.player)
            {
                if (otherPlayer.whoAmI != player.whoAmI)
                    if (otherPlayer.GetModPlayer<DecimationPlayer>().amuletSlotItem.type == mod.ItemType<GraniteAmulet>() && otherPlayer.team == player.team)
                    {
                        player.statLife += (int)(damage * 0.03f);
                        break;
                    }
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (player.HasBuff(mod.BuffType<ScarabEndurance>()) && scarabCounter > 0 && lastHitCounter == 0 && !wasHurt)
            {
                Main.projectile[scarabs[scarabCounter - 1]].Kill();
                scarabCounter--;
                wasHurt = true;
            }

            if (graniteLinedTunicEquipped)
            {
                player.statLife += (int)(damage * 0.04f);
            }

            foreach (Player otherPlayer in Main.player)
            {
                if (otherPlayer.whoAmI != player.whoAmI)
                    if (otherPlayer.GetModPlayer<DecimationPlayer>().amuletSlotItem.type == mod.ItemType<GraniteAmulet>() && otherPlayer.team == player.team)
                    {
                        player.statLife += (int)(damage * 0.03f);
                        break;
                    }
            }
        }
    }
}
