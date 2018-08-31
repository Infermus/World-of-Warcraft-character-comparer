﻿using System.Collections.Generic;
using WowCharComparerLib.Models.CharacterProfile.Items.Others;

namespace WowCharComparerLib.Models.CharacterProfile.Items.Gear
{
    public class DefaultItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int Quality { get; set; }

        public int ItemLevel { get; set; }

        public ToolTipParams ToolTipParams { get; set; }

        public Stats [] Stats { get; set; }

        public int Armor { get; set; }

        public string Context { get; set; }

        public int [] BonusLists { get; set; } //TODO in progress, throwing null

        public int ArtifactId { get; set; }

        public int DisplayInfoId { get; set; }

        public int ArtifactAppearanceId { get; set; }

        public ArtifactTriats ArtifactTriats { get; set; }

        public Relics [] Relics { get; set; }

        public Others.Appearance Appearance { get; set; }

        public AzeriteItem AzeriteItem { get; set; }

        public AzeriteEmpoweredItem AzeriteEmpoweredItem { get; set; }

    }
}