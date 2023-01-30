using System;
using UnityEngine;
using Verse;

namespace ProjectileInversion;

public class Settings : ModSettings
{
    public static int speed = 200;

    public static bool showText = true;
    public static bool noRebound;
    public static bool addTrait = true;
    public static string currentVersion;
    public static int weaponDamage = 1;
    public static float baseChance = 0.5f;
    public static int requiredTraitLevel = 20;

    public static void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard(GameFont.Small) { ColumnWidth = rect.width };
        listing_Standard.Begin(rect);
        listing_Standard.Label("ProjectileInversionSettingSpeedLabel".Translate() + ":" + speed.ToString("0.0"),
            -1f, "ProjectileInversionSettingSpeedDesc".Translate());
        speed = (int)listing_Standard.Slider(speed, 1f, 999f);
        listing_Standard.CheckboxLabeled("ProjectileInversionSettingShowTextLabel".Translate(), ref showText,
            "ProjectileInversionSettingShowTextDesc".Translate());
        listing_Standard.CheckboxLabeled("ProjectileInversionSettingNoReboundLabel".Translate(), ref noRebound,
            "ProjectileInversionSettingNoReboundDesc".Translate());
        listing_Standard.CheckboxLabeled("ProjectileInversionSettingAutoTraitLabel".Translate(), ref addTrait,
            "ProjectileInversionSettingAutoTraitDesc".Translate());
        listing_Standard.Gap();
        listing_Standard.Label("ProjectileInversionSettingWeaponDamageLabel".Translate(weaponDamage));
        listing_Standard.IntAdjuster(ref weaponDamage, 1);
        listing_Standard.Gap();
        listing_Standard.IntAdjuster(ref requiredTraitLevel, 1);
        listing_Standard.Gap();
        listing_Standard.Label("ProjectileInversionSettingBaseChanceLabel".Translate(baseChance * 100));
        baseChance = (float)Math.Round(listing_Standard.Slider(baseChance, 0.01f, 1f), 2);
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("ProjectileInversionSettingCurrentModVersionLabel".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref speed, "PISpeed", 200);
        Scribe_Values.Look(ref showText, "PIShowText", true);
        Scribe_Values.Look(ref addTrait, "PIAddTrait", true);
        Scribe_Values.Look(ref noRebound, "PINoRebound");
        Scribe_Values.Look(ref weaponDamage, "PIWeaponDamage", 1);
        Scribe_Values.Look(ref requiredTraitLevel, "PIrequiredTraitLevel", 1);
        Scribe_Values.Look(ref baseChance, "PIBaseChance", 0.5f);
    }
}
