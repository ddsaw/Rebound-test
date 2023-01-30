using HarmonyLib;
using RimWorld;
using Verse;

namespace ProjectileInversion;

[HarmonyPatch(typeof(SkillRecord), "Learn", typeof(float), typeof(bool))]
public static class SkillRecord_Learn
{
    public static void Postfix(SkillRecord __instance)
    {
        if (!Settings.addTrait)
        {
            return;
        }

        var traverse = Traverse.Create(__instance);
        var value = traverse.Field("pawn").GetValue<Pawn>();
        if (API.hasTrait(value))
        {
            return;
        }

        if (!__instance.def.Equals(SkillDefOf.Melee))
        {
            return;
        }

        if (__instance.GetLevel() != Settings.requiredTraitLevel)
        {
            return;
        }

        value.story.traits.GainTrait(new Trait(TraitDef.Named("ProjectileInversion_Trait")));
        var isColonist = value.IsColonist;
        if (isColonist)
        {
            Messages.Message(
                "YourPawnGainProjectileInversionTraitMsg".Translate(value.Label,
                    TraitDef.Named("ProjectileInversion_Trait").degreeDatas.RandomElement().label),
                value, MessageTypeDefOf.PositiveEvent);
        }
    }
}
