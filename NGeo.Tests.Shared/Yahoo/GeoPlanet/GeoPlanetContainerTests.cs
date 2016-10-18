﻿using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace NGeo.Yahoo.GeoPlanet
{
    [TestClass]
    public class GeoPlanetContainerTests
    {
        private static readonly string AppId = ConfigurationManager.AppSettings["GeoPlanetAppId"];

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Place_ShouldReturn1Result_ForWoeId2380358()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var place = geoPlanetClient.Place(2380358);

                place.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Place_ShouldReturnNull_When404ExceptionIsThrown()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var place = geoPlanetClient.Place(int.MaxValue);

                place.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Places_ShouldReturnAtLeast1Result_ForQuery_Sfo()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.Places("SFO");

                places.ShouldNotBeNull();
                places.Items.Count.ShouldBeInRange(1, int.MaxValue);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Places_ShouldReturnMoreThan1Result_ForQuery_Ca()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.Places("CA");

                places.ShouldNotBeNull();
                places.Items.Count.ShouldBeInRange(2, int.MaxValue);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Places_ShouldReturnEmpty_WhenNoResultsAreFound()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.Places("asdfdfmdlfjkdlajsdlfasdfjdjfdlajlsdflasdjf");

                places.ShouldNotBeNull();
                places.Items.Count.ShouldEqual(0);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Places_ShouldReturnNull_WhenArgumentExceptionIsThrown()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.Places("");

                places.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Parent_ShouldReturn1Result_ForWoeId2380358()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var place = geoPlanetClient.Parent(2380358);

                place.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Parent_ShouldReturnNull_When404ExceptionIsThrown()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var place = geoPlanetClient.Parent(1);

                place.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Ancestors_ShouldReturnResults_ForWoeId2380358()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.Ancestors(2380358);

                places.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Ancestors_ShouldReturnNull_When404ExceptionIsThrown()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.Ancestors(23424802);

                places.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_BelongTos_ShouldReturnResults_ForWoeId2380358()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.BelongTos(23424833, RequestView.Long);

                places.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_BelongTos_ShouldReturnNull_When404ExceptionIsThrown()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var places = geoPlanetClient.BelongTos(1);

                places.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Types_ShouldReturnResults()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var placeTypes = geoPlanetClient.Types();

                placeTypes.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Type_ShouldReturn1Result_ForType35()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var placeType = geoPlanetClient.Type(35, RequestView.Long);

                placeType.ShouldNotBeNull();
                placeType.Code.ShouldEqual(35);
                placeType.Name.ShouldNotBeNull();
                placeType.Description.ShouldNotBeNull();
                placeType.Uri.ShouldNotBeNull();
                placeType.Language.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Type_ShouldReturnNull_WhenTypeDoesNotExist()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var placeType = geoPlanetClient.Type(int.MaxValue, RequestView.Long);

                placeType.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Continents_ShouldReturn7Results()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var continents = geoPlanetClient.Continents();

                continents.ShouldNotBeNull();
                continents.Items.ShouldNotBeNull();
                continents.Items.Count.ShouldEqual(7);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Countries_ShouldReturnResults()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var countries = geoPlanetClient.Countries();

                countries.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_States_ShouldReturn32Results_ForWoeId23424781()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var states = geoPlanetClient.States(23424781);

                states.ShouldNotBeNull();
                states.Items.ShouldNotBeNull();
                states.Items.Count.ShouldEqual(32);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_States_ShouldReturnNull_ForWoeId28289409()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var states = geoPlanetClient.States(28289409);

                states.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Level1Admins_ShouldReturn13Results_ForWoeId23424775()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var level1Admins = geoPlanetClient.Level1Admins(23424775);

                level1Admins.ShouldNotBeNull();
                level1Admins.Items.ShouldNotBeNull();
                level1Admins.Items.Count.ShouldEqual(13);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Counties_ShouldReturn88Results_ForWoeId2347594()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var counties = geoPlanetClient.Counties(2347594);

                counties.ShouldNotBeNull();
                counties.Items.ShouldNotBeNull();
                counties.Items.Count.ShouldEqual(88);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_States_ShouldReturnNull_ForWoeId2514815()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var counties = geoPlanetClient.Counties(2514815);

                counties.ShouldBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Level2Admins_ShouldReturn58Results_ForWoeId2347563()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var level2Admins = geoPlanetClient.Level2Admins(2347563);

                level2Admins.ShouldNotBeNull();
                level2Admins.Items.ShouldNotBeNull();
                level2Admins.Items.Count.ShouldEqual(58);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Concordance_WithIntId_ShouldReturnResults()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var response = geoPlanetClient.Concordance(ConcordanceNamespace.WoeId, 2380358);

                response.ShouldNotBeNull();
                response.ForNamespace.ShouldEqual(ConcordanceNamespace.WoeId);
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Concordance_WithStringId_ShouldReturnResults()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var response = geoPlanetClient.Concordance(ConcordanceNamespace.Iso, "DE");

                response.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void Yahoo_GeoPlanet_GeoPlanetContainer_Concordance_ShouldReturnNull_When404ExceptionIsThrown()
        {
            using (var geoPlanetClient = new GeoPlanetContainer(AppId))
            {
                var response = geoPlanetClient.Concordance(ConcordanceNamespace.GeoNames, 6295630);

                response.ShouldBeNull();
            }
        }

    }
}
