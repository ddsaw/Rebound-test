﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ProjectileInversion
{
    // Token: 0x02000002 RID: 2
    public static class API
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static bool canInverse(Pawn pawn, Projectile p)
        {
            var result = checkPawnInverseChance(pawn) && checkProjectileInversion(p);
            return result;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
        public static void damageWeapon(Pawn pawn)
        {
            if (!hasWeapon(pawn))
            {
                return;
            }

            if (!canDamageWeapon(pawn))
            {
                return;
            }

            var primary = pawn.equipment.Primary;
            var hitPoints = primary.HitPoints;
            primary.HitPoints = hitPoints - 1;
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020CC File Offset: 0x000002CC
        private static bool hasWeapon(Pawn pawn)
        {
            var flag = pawn.equipment.Primary == null || !pawn.equipment.Primary.def.IsMeleeWeapon ||
                       pawn.equipment.Primary.IsBrokenDown();
            return !flag;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002120 File Offset: 0x00000320
        private static bool randomCheck(float chance)
        {
            var random = new Random();
            var num = random.Next(0, 100);
            return num <= chance * 100f;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002150 File Offset: 0x00000350
        public static bool hasTrait(Pawn pawn)
        {
            return pawn.story.traits.HasTrait(TraitDef.Named("ProjectileInversion_Trait"));
        }

        // Token: 0x06000006 RID: 6 RVA: 0x0000217C File Offset: 0x0000037C
        private static bool checkPawnInverseChance(Pawn pawn)
        {
            if (pawn.IsColonist && !pawn.Drafted)
            {
                return false;
            }

            if (!hasTrait(pawn))
            {
                return false;
            }

            if (!hasWeapon(pawn))
            {
                return false;
            }

            if (pawn.equipment.Primary.def.useHitPoints && pawn.equipment.Primary.HitPoints <= 1)
            {
                pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out _, pawn.Position);
            }

            var num = (float) pawn.skills.GetSkill(SkillDefOf.Melee).levelInt;
            var num2 = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);
            if (num2 > 1f)
            {
                num2 = 1f;
            }

            var chance = 0.5f * num2 * (num / 10f);
            return randomCheck(chance);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002290 File Offset: 0x00000490
        private static bool canDamageWeapon(Pawn pawn)
        {
            bool result;
            if (!hasWeapon(pawn))
            {
                result = false;
            }
            else
            {
                var num = (float) pawn.skills.GetSkill(SkillDefOf.Melee).levelInt;
                var chance = 0.4f / (num / 5f);
                result = randomCheck(chance);
            }

            return result;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000022E0 File Offset: 0x000004E0
        private static bool isVanilaProjectile(Projectile p)
        {
            return new List<Type>
            {
                typeof(Bullet),
                typeof(Projectile_Explosive),
                typeof(Projectile_DoomsdayRocket)
            }.Contains(p.GetType());
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002344 File Offset: 0x00000544
        private static bool isProjectileTooFast(Projectile p)
        {
            return p.def.projectile.speed >= Settings.speed;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x0000237C File Offset: 0x0000057C
        private static bool checkProjectileInversion(Projectile p)
        {
            return !isProjectileTooFast(p);
        }
    }
}