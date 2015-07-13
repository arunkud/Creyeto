var mapLocator = mapLocator || {};

mapLocator.LoadMap = function () {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;
            var coords = new google.maps.LatLng(latitude, longitude);
            var mapOptions = {
                zoom: 15,
                center: coords,
                mapTypeControl: true,
                navigationControlOptions: {
                    style: google.maps.NavigationControlStyle.SMALL
                },
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(
            document.getElementById("mapContainer"), mapOptions
            );
            var marker = new google.maps.Marker({
                position: coords,
                map: map,
                title: "Your current location!"
            });

            map.addListener("click", function (e) {
                alert(e.latLng.A);
                document.getElementById("<%=hfLat.ClientID %>").value = e.latLng.A;
                document.getElementById("<%=hfLon.ClientID %>").value = e.latLng.F;
            });
        });
    } else {
        alert("Geolocation API is not supported in your browser.");
    }
}();

