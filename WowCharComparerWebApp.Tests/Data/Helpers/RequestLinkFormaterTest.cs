using System;
using System.Collections.Generic;
using WowCharComparerWebApp.Configuration;
using WowCharComparerWebApp.Data.Helpers;
using WowCharComparerWebApp.Data.Wrappers;
using WowCharComparerWebApp.Enums;
using WowCharComparerWebApp.Enums.RaiderIO;
using WowCharComparerWebApp.Models.Servers;
using Xunit;

namespace WowCharComparerWebApp.Tests.Data.Helpers
{
    // Convention
    // method name : UnitOfWork_StateUnderTest_ExpectedBehavior
    // 1. Arrange
    // 2. Act
    // 3. Assert

    public class RequestLinkFormaterTest
    {
        [Fact]
        public void GenerateAPIRequestLink_WhenGenerateLink_ShouldGenerateValidLinkForCharacter()
        {
            // Arrange
            string expectedLink = "https://eu.api.blizzard.com/wow/character/burning-legion/selectus?locale=en_GB";

            // Act
            RequestLocalization requestLocalization = new RequestLocalization()
            {
                CoreRegionUrlAddress = APIConf.BlizzadAPIAddressWrapper[Region.Europe],
                Realm = new Realm() { Slug = "burning-legion", Locale = "en_GB" }
            };

            Uri actualLink = RequestLinkFormater.GenerateAPIRequestLink(BlizzardAPIProfiles.Character, requestLocalization,
                                                                        new List<KeyValuePair<string, string>>(),
                                                                        requestLocalization.Realm.Slug, "Selectus");
            //Assert
            Assert.Equal(expectedLink, actualLink.AbsoluteUri);
        }

        [Fact]
        public void GenerateRaiderIOApiRequestLink_WhenGenerateLink_ShouldGenerateValidLinkForRadierIO()
        {
            //Arrange
            string expectedLink = "https://raider.io/api/v1/characters/profile?region=eu&realm=burning-legion&name=wykminiacz&fields=mythic_plus_best_runs%2Cmythic_plus_ranks";

            RequestLocalization requestLocalization = new RequestLocalization()
            {
                CoreRegionUrlAddress = APIConf.RaiderIOAdress,
                Realm = new Realm() { Slug = "burning-legion", Locale = "en_GB", Timezone = "Europe/Paris"}
            };

            List<RaiderIOCharacterFields> characterFields = new List<RaiderIOCharacterFields>()
            {
                RaiderIOCharacterFields.MythicPlusBestRuns,
                RaiderIOCharacterFields.MythicPlusRanks
            };

            string region = requestLocalization.Realm.Timezone == "Europe/Paris" ? "eu" : throw new Exception("Chosen realm is not European");


            string localFields = string.Empty;

            foreach (RaiderIOCharacterFields field in characterFields)
            {
                string wrappedField = EnumDictionaryWrapper.rioCharacterFieldWrapper[field];
                localFields = localFields.AddFieldToUrl(wrappedField);

                localFields = localFields.EndsWith("+") ? localFields.Remove(localFields.Length - 1, 1)
                                       : localFields;
            }

            localFields = localFields.EndsWith("%2C") ? localFields.Remove(localFields.Length - 3, 3) // remove join parameters symbol at ending field
                                                       : localFields;

            List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("fields", localFields)
            };

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(RaiderIOCharacterParams.Region.ToString(), region),
                new KeyValuePair<string, string>(RaiderIOCharacterParams.Realm.ToString(), requestLocalization.Realm.Slug),
                new KeyValuePair<string, string>(RaiderIOCharacterParams.Name.ToString(), "Wykminiacz")
            };
            
            //Act
            Uri actualLink = RequestLinkFormater.GenerateRaiderIOApiRequestLink(fields, parameters);

            //Assert
            Assert.Equal(expectedLink, actualLink.AbsoluteUri);
         }
    }
}