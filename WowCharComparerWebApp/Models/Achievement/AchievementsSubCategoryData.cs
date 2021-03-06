﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WowCharComparerWebApp.Models.Achievement
{
    public class AchievementsSubCategoryData
    {
        [Key]
        [JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string CategoryName { get; set; }

        [JsonProperty(PropertyName = "Achievements")]
        public ICollection<AchievementsData> AchievementsData { get; set; }

    }
}