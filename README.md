# NGeo
NGeo makes it easier for users of geographic data to invoke GeoNames service endpoints. You'll no longer have to 
write your own GeoNames or GeoPlanet clients. It is a complete rewrite using HttpClient and making all calls 
async. It uses parallel execution when parsing the returned results.

## How can I use it?
	var request = new FindNearbyToponymRequest() {
		UserName = "userName",
		Latitude = 47.3m,
		Longitude = 9m,
		Style = Style.FULL
	};
	var response = await GeoNameService.FindNearbyToponym(request);

## New in version 1.8 - Breaking Changes
The PlaceFinder client was removed in version 1.8.0.0, because that service is no longer available as a WCF-consumable API. It was then re-added in version 1.8.1.0 with an OAuth client implementation. Note that the Yahoo! PlaceFinder service is now part of Yahoo!'s [BOSS GEO Services](http://developer.yahoo.com/boss/geo/), and is no longer free. To use the NGeo PlaceFinder client, you must first sign up for BOSS GEO, obtain a Consumer Key and Consumer Secret, and give Yahoo! a valid credit card number for billing. No builds or revisions in version 1.8.x.y affect either the GeoNames or GeoPlanet clients.

Because of the new PlaceFinder authentication model, there is now an `IContainPlaceFinder` interface and implementation. The difference between this and the `IConsumePlaceFinder` interface is that the latter takes consumerKey and consumerSecret arguments directly within its methods, whereas the former does not. This is the same pattern used by the GeoNames and GeoPlanet clients to wrap authentication information when you are using an IoC container. For more information, read about the `IContainGeoNames` and `IContainGeoPlanet` interfaces below.

### How to fix breaking changes in versions 1.8+
Much of the source code has not changed. The major difference is that the various methods on the `IConsumePlaceFinder` interface now take two additional arguments for the consumer key and secret. You can fix compiler errors after upgrading by either passing these arguments, or by changing your code to use `IContainPlaceFinder` instead of `IConsumePlaceFinder`. The default implementation of this interface in the project is `PlaceFinderContainer`, whose constructor takes the consumer key and secret and wraps the default `IConsumerPlaceFinder` implementation (which is still `PlaceFinderClient`).

## New in version 1.5
Because each NGeo service comes with a corresponding IConsumeXyz interface, you have always been able to dependency inject client instances and control their lifetimes + disposal semantics. However the `IConsumeGeoNames` and `IConsumeGeoPlanet` interfaces contain several overloads that accept either a username or app id for authentication. This means your code had to maintain a separate reference to your geonames username or geoplanet appid, and pass it in as a method parameter. When using inversion of control and dependency injecting IConsumeGeoWhatever instances into your controller, this can be annoying because you have to maintain your auth string in a separate dependency (or worse, hard-code it).

Today there are two new interfaces: `IContainGeoNames` and `IContainGeoPlanet`. These are nearly identical to their respective IConsumeGeo predecessors, except that their methods do not contain username or app id parameters. Instead, they expect the implementation class will already know the auth credential. Each class has a default implementation, named `GeoNamesContainer` and `GeoPlanetContainer` respectively. Each of these classes has a constructor that accepts a string for your geonames user name or geoplanet app id. You can use them in your IoC container to resolve the auth dependency at runtime. Let's look at an example using SimpleInjector:

	//  each time the service is requested, construct a new instance with the auth credential
	simpleInjectorContainer.RegisterPerWebRequest<IContainGeoNames>(() =>
		new GeoNamesContainer(ConfigurationManager.AppSettings["GeoNamesUserName"]));

	simpleInjectorContainer.RegisterPerWebRequest<IContainGeoPlanet>(() =>
		new GeoPlanetContainer(ConfigurationManager.AppSettings["GeoPlanetAppId"]));

With that registration, you can do something like this:

	public class GeoController : Controller
	{
		private readonly IContainGeoNames _geoNames;
		private readonly IContainGeoPlanet _geoPlanet;

		public GeoController(IContainGeoNames geoNames, IContainGeoPlanet geoPlanet)
		{
			_geoNames = geoNames;
			_geoPlanet = geoPlanet;
		}

		public ActionResult DoSomeWork()
		{
			// no auth arg required
			var gnEarth = _geoNames.Get(6295630);
			var gpEarth = _geoPlanet.Place(1);
			return View();
		}
	}

The actual `GeoNamesContainer` and `GeoPlanetContainer` classes just wrap a respective Client instance along with the auth string passed to the constructor, and delegate all method invocations to the client. So you will always get the same results, the only difference is these new interfaces hide the auth credential from you.

## Why should I use it?
There are at least 2 other GeoNames clients that I'm aware of, and they are listed on the [GeoNames Client Libraries page](http://www.geonames.org/export/client-libraries.html). This project is very much like the [.NET WCF project written by Baretta2 / Myren](http://www.codeproject.com/Articles/30627/GeoNames-NET-WCF-Client). I started modifying the Myren.GeoNames.Client project source code because my organization bought premium GeoNames credits, and we needed to invoke a different service URL. Also, since Baretta2's project was published, GeoNames has started requiring a username parameter to be passed to each web service invocation.

But that wasn't enough. We also needed a client for the [Yahoo! GeoPlanet API](http://developer.yahoo.com/geo/geoplanet/data/), and to my knowledge, there is not one of these written in .NET. So we rolled our own.

But that still wasn't enough. I also wanted this code to be available as a [NuGet package](http://nuget.org/packages/NGeo), so that I could use it in multiple projects.

So if you want a NuGet package that can give your app a client for GeoNames and Yahoo! GeoPlanet services, this is the only one I'm aware of.

## What if I don't like it?
So far, I've only built out clients for the services I use in my projects. If you would like to contribute to this project instead of building your own, feel free to fork and send me a pull request.

## License
This software is subject to the terms of the [Microsoft Public License (Ms-PL)](http://www.opensource.org/licenses/MS-PL).
