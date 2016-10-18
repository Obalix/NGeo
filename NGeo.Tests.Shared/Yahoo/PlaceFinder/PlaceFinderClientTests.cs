﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace NGeo.Yahoo.PlaceFinder
{
    [TestClass]
    public class PlaceFinderClientTests
    {
        private static readonly string ConsumerKey = ConfigurationManager.AppSettings["PlaceFinderConsumerKey"];
        private static readonly string ConsumerSecret = ConfigurationManager.AppSettings["PlaceFinderConsumerSecret"];

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldBePublic()
        {
            using (var client = new PlaceFinderClient())
            {
                client.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldEnsureStreetDetailFlagIsRemoved()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByName("Eiffel Tower")
                {
                    Flags = new HashSet<Flag>
                    {
                        Flag.StreetDetail,
                    }
                };

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                request.Flags.ShouldNotContain(Flag.StreetDetail);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldEnsurePhpFlagIsRemoved()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByName("Great Pyramids")
                {
                    Flags = new HashSet<Flag>
                    {
                        Flag.Php,
                    }
                };

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                request.Flags.ShouldNotContain(Flag.Php);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByCoordinates_ShouldReturn1Result_ForYahooExample()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByCoordinates(37.775391, 122.412209);

                var resultSet = client.Find(request, ConsumerKey, ConsumerSecret);
                resultSet.ShouldNotBeNull();
                resultSet.Results.ShouldNotBeNull();
                resultSet.Count.ShouldEqual(1);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByCoordinates_ShouldNotThrowException_WhenWoeIdIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                var resultSet = client.Find(new PlaceByCoordinates(41.53531, -160.793358), ConsumerKey, ConsumerSecret);
                resultSet.Count.ShouldEqual(1);
                resultSet.Results[0].WoeId.ShouldBeNull();
                resultSet.Results[0].WoeType.ShouldBeNull();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByCoordinates_ShouldThrowException_WhenArgIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                client.Find(null as PlaceByCoordinates, ConsumerKey, ConsumerSecret);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByFreeformText_ShouldReturn1Result_ForYahooExample()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("701 First Ave., Sunnyvale, CA 94089");

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByFreeformText_ShouldThrowException_WhenArgIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                client.Find(null as PlaceByFreeformText, ConsumerKey, ConsumerSecret);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByName_ShouldReturn1Result_ForYahooExample()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByName("Yosemite National Park");

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByName_ShouldThrowException_WhenArgIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                client.Find(null as PlaceByName, ConsumerKey, ConsumerSecret);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByWoeId_ShouldReturn1Result_ForYahooExample()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByWoeId(12797150);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByWoeId_ShouldThrowException_WhenArgIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                client.Find(null as PlaceByWoeId, ConsumerKey, ConsumerSecret);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByMultilineAddress_ShouldReturn1Result_ForYahooExample1()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByMultilineAddress("701 First Ave.");

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldBeInRange(1, int.MaxValue);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByMultilineAddress_ShouldThrowException_WhenArgIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                client.Find(null as PlaceByMultilineAddress, ConsumerKey, ConsumerSecret);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindBFullyParsedAddress_ShouldReturn1Result_ForYahooExample1()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFullyParsedAddress
                {
                    House = "701",
                    Street = "First Ave.",
                    Postal = "94089",
                    City = "Sunnyvale",
                    County = "Santa Clara",
                    StateOrProvince = "CA",
                    Country = "USA",
                };

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Yahoo_PlaceFinder_PlaceFinderClient_FindByFullyParsedAddress_ShouldThrowException_WhenArgIsNull()
        {
            using (var client = new PlaceFinderClient())
            {
                client.Find(null as PlaceByFullyParsedAddress, ConsumerKey, ConsumerSecret);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnCoordinateDataOnly_WhenCoordinateDataOnlyIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .ReturnCoordinateDataOnly();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].CityName.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnCoordinateDataOnly_WhenCoordinateDataOnlyIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .ReturnCoordinateDataOnly()
                    .ReturnCoordinateDataOnly(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].CityName.ShouldEqual(request.Location);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnWoeId_WhenExcludeWoeIdIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .ExcludeWoeId();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].WoeId.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnWoeId_WhenExcludeWoeIdIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .ExcludeWoeId()
                    .ExcludeWoeId(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].WoeId.HasValue.ShouldBeTrue();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnGlobalElements_WhenReturnGlobalElementsIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .ReturnGlobalElements();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].CityName.ShouldBeNull();
                results.Results[0].Level3Name.ShouldEqual(request.Location);
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnGlobalElements_WhenReturnGlobalElementsIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .ReturnGlobalElements()
                    .ReturnGlobalElements(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].CityName.ShouldEqual(request.Location);
                results.Results[0].Level3Name.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnAirportCode_WhenIncludeAirportCodeIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeAirportCode();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].AirportCode.ShouldEqual("ATL");
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnAirportCode_WhenIncludeAirportCodeIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeAirportCode()
                    .IncludeAirportCode(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].AirportCode.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnTelephoneAreaCode_WhenIncludeTelephoneAreaCodeIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeTelephoneAreaCode();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].AreaCode.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnTelephoneAreaCode_WhenIncludeTelephoneAreaCodeIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeTelephoneAreaCode()
                    .IncludeTelephoneAreaCode(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].AreaCode.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnTimeZone_WhenIncludeTimeZoneIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeTimeZone();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].TimeZone.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnTimeZone_WhenIncludeTimeZoneIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeTimeZone()
                    .IncludeTimeZone(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].TimeZone.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnBoundingBox_WhenIncludeBoundingBoxIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeBoundingBox();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].BoundingBox.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnBoundingBox_WhenIncludeBoundingBoxIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeBoundingBox()
                    .IncludeBoundingBox(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].BoundingBox.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnNeighborhoods_WhenIncludeNeighborhoodsIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeNeighborhoods();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                string.IsNullOrWhiteSpace(results.Results[0].Neighborhood).ShouldBeFalse();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnNeighborhoods_WhenIncludeNeighborhoodsIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Atlanta")
                    .IncludeNeighborhoods()
                    .IncludeNeighborhoods(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                string.IsNullOrWhiteSpace(results.Results[0].Neighborhood).ShouldBeTrue();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldReturnCrossStreets_WhenIncludeCrossStreetsIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("1600 Pennsylvania Avenue Washington, DC")
                    .IncludeCrossStreets();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                string.IsNullOrWhiteSpace(results.Results[0].CrossStreets).ShouldBeFalse();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotReturnCrossStreets_WhenIncludeCrossStreetsIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("1600 Pennsylvania Avenue Washington, DC")
                    .IncludeCrossStreets()
                    .IncludeCrossStreets(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].CrossStreets.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldLimitResultsToLocaleCountry_WhenLimitResultsToLocaleCountryIsInvoked_WithNoArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Paris")
                {
                    Locale = "en-US",
                    Count = 50,
                }
                .LimitResultsToLocaleCountry();

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldBeInRange(1, 50);
                results.Results.ToList().ForEach(result => result.CountryCode.ShouldEqual("US"));
            }
        }

        [TestMethod]
        public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotLimitResultsToLocaleCountry_WhenLimitResultsToLocaleCountryIsInvoked_WithFalseArg()
        {
            using (var client = new PlaceFinderClient())
            {
                var request = new PlaceByFreeformText("Paris")
                {
                    Locale = "en-US",
                }
                .LimitResultsToLocaleCountry()
                .LimitResultsToLocaleCountry(false);

                var results = client.Find(request, ConsumerKey, ConsumerSecret);
                results.ShouldNotBeNull();
                results.Results.ShouldNotBeNull();
                results.Count.ShouldEqual(1);
                results.Results[0].CountryCode.ShouldEqual("FR");
            }
        }

        //[TestMethod] // this test fails now, placefinder quick mode seems to not work
        //public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldOnlyFindExactMatches_WhenQuickModeIsInvoked_WithNoArg()
        //{
        //    using (var client = new PlaceFinderClient())
        //    {
        //        var request = new PlaceByFreeformText("1426 Some Road")
        //            .QuickMode();

        //        var results = client.Find(request, ConsumerKey, ConsumerSecret);
        //        results.ShouldNotBeNull();
        //        results.Results.ShouldNotBeNull();
        //        results.Count.ShouldEqual(7);
        //    }
        //}

        //[TestMethod] // this test fails now, placefinder quick mode seems to not work
        //public void Yahoo_PlaceFinder_PlaceFinderClient_ShouldNotOnlyFindExactMatches_WhenQuickModeIsInvoked_WithFalseArg()
        //{
        //    using (var client = new PlaceFinderClient())
        //    {
        //        var request = new PlaceByFreeformText("1426 Some Road")
        //            .QuickMode()
        //            .QuickMode(false);

        //        var results = client.Find(request, ConsumerKey, ConsumerSecret);
        //        results.ShouldNotBeNull();
        //        results.Results.ShouldNotBeNull();
        //        results.Count.ShouldEqual(2);
        //    }
        //}
    }
}
