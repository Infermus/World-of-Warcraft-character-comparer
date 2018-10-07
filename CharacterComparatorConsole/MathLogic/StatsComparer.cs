﻿using System;
using System.Collections.Generic;
using WowCharComparerWebApp.Models.CharacterProfile;

namespace CharacterComparatorConsole.MathLogic
{
    internal class StatsComparer
    {
        public static List<KeyValuePair<Stats, decimal>> ComparePrimaryCharacterStats(List<CharacterModel> parsedResultList)
        {
            List<KeyValuePair<Stats, decimal>> countedPrimaryStatsPercent = new List<KeyValuePair<Stats, decimal>>();

            try
            {
                if (parsedResultList.Count == 2)
                {
                    List<Tuple<Stats, int, int>> minMaxPrimaryStatsTuple = new List<Tuple<Stats, int, int>>
                    {
                        new Tuple<Stats, int, int>(Stats.Str,
                                                    Math.Max(parsedResultList[0].Stats.Str, parsedResultList[1].Stats.Str),
                                                    Math.Min(parsedResultList[0].Stats.Str, parsedResultList[1].Stats.Str)),
                        new Tuple<Stats, int, int>(Stats.Int,
                                                    Math.Max(parsedResultList[0].Stats.Int, parsedResultList[1].Stats.Int),
                                                    Math.Min(parsedResultList[0].Stats.Int, parsedResultList[1].Stats.Int)),
                        new Tuple<Stats, int, int>(Stats.Agi,
                                                    Math.Max(parsedResultList[0].Stats.Agi, parsedResultList[1].Stats.Agi),
                                                    Math.Min(parsedResultList[0].Stats.Agi, parsedResultList[1].Stats.Agi)),
                        new Tuple<Stats, int, int>(Stats.Sta,
                                                    Math.Max(parsedResultList[0].Stats.Sta, parsedResultList[1].Stats.Sta),
                                                    Math.Min(parsedResultList[0].Stats.Sta, parsedResultList[1].Stats.Sta)),
                    };

                    countedPrimaryStatsPercent = PrimaryStatsPercentCalculation(minMaxPrimaryStatsTuple);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);               
            }

            return countedPrimaryStatsPercent;
        }

        public static List<KeyValuePair<Stats, decimal>> PrimaryStatsPercentCalculation(List<Tuple<Stats, int, int>> primaryStats)
        {
            List<KeyValuePair<Stats, decimal>> countedPrimaryStatsPercent = new List<KeyValuePair<Stats, decimal>>();

            for (int index = 0; index <= 3; index++)
            {
                decimal value1 = primaryStats[index].Item2;
                decimal value2 = primaryStats[index].Item3;
                decimal countedPercent = decimal.Round(((value1 - value2) / value2) * 100, 2);
                countedPrimaryStatsPercent.Add(new KeyValuePair<Stats, decimal>(primaryStats[index].Item1, countedPercent));
            }
            return countedPrimaryStatsPercent;
        }       
    }
}
