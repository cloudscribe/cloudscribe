window.cloudscribeUnotrusiveBingMap = {
	mapItems: [],
	buildMap: function (div) {
		var mapItem = { };
		mapItem.map = new Microsoft.Maps.Map(div, {});
		mapItem.fromAddressElement = document.getElementById(div.dataset.fromAddressElementId);
		mapItem.btnGetDirections = document.getElementById(div.dataset.getDirectionsButtonId);
		mapItem.btnGetDirections.onclick = function () {
			if (mapItem.fromAddressElement.value) {
				var requestOptions = {
					bounds: mapItem.map.getBounds(),
					where: mapItem.fromAddressElement.value,
					callback: mapItem.getDirectionsCallback
				};
				mapItem.searchManager.geocode(requestOptions);
			}
		};

		mapItem.getDirectionsCallback = function (answer, userData) {
			var fromResult = answer.results[0];
			var fromPoint = new Microsoft.Maps.Directions.Waypoint({ address: fromResult.name, location: fromResult.location });
			mapItem.directionsManager.addWaypoint(fromPoint);

			var destination = new Microsoft.Maps.Directions.Waypoint({ address: div.dataset.locationTitle, location: mapItem.geoResult.location });
			mapItem.directionsManager.addWaypoint(destination);

			mapItem.directionsManager.setRenderOptions({ itineraryContainer: document.getElementById(div.dataset.itineraryElementId) });
			//mapItem.directionsManager.showInputPanel(document.getElementById(div.directionsElementId));
			mapItem.directionsManager.calculateDirections();
		};
		
		mapItem.searchCallback = function (answer, userData) {
			mapItem.geoResult = answer.results[0];
			mapItem.map.setView({ bounds: mapItem.geoResult.bestView });
			mapItem.map.entities.push(new Microsoft.Maps.Pushpin(mapItem.geoResult.location));
			Microsoft.Maps.loadModule('Microsoft.Maps.Directions', function () {
				mapItem.directionsManager = new Microsoft.Maps.Directions.DirectionsManager(mapItem.map);
				mapItem.directionsManager.setRequestOptions({ routeMode: Microsoft.Maps.Directions.RouteMode.driving });     
			});
		};

		mapItem.initSearch = function () {
			var requestOptions = {
				bounds: mapItem.map.getBounds(),
				where: div.dataset.address,
				callback: mapItem.searchCallback
			};
			mapItem.searchManager.geocode(requestOptions);
		};
		
		Microsoft.Maps.loadModule('Microsoft.Maps.Search', function () {
			mapItem.searchManager = new Microsoft.Maps.Search.SearchManager(mapItem.map);
			mapItem.initSearch();
		});
		
		this.mapItems.push(mapItem);
	}
};

function loadMapScenario() {
	var mapElements = document.querySelectorAll('[data-bing-map]');
	for (var i = 0; i < mapElements.length; i++) {
		var item = mapElements[i];
		window.cloudscribeUnotrusiveBingMap.buildMap(item);
	}
}