window.syndicationMap = {
    mapInstance: null,

    init: function (containerId, dotnetRef) {
        if (!L) return;

        // Prevent double initialization
        if (this.mapInstance) {
            this.mapInstance.remove();
            this.mapInstance = null;
        }

        // Also check if the container already has Leaflet structure (edge case)
        const container = document.getElementById(containerId);
        if (container && container._leaflet_id) {
            container._leaflet_id = null; // Clear usage
            container.innerHTML = ''; // Wipe content
        }

        const map = L.map(containerId, {
            minZoom: 2,
            maxBounds: [[-90, -180], [90, 180]], // Restrict panning to one world
            maxBoundsViscosity: 1.0 // Sticky bounds
        }).setView([20, 140], 2);
        this.mapInstance = map;

        L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>',
            subdomains: 'abcd',
            maxZoom: 19,
            noWrap: true, // Prevent horizontal repetition
            bounds: [[-90, -180], [90, 180]]
        }).addTo(map);

        const regions = [
            { name: "Australia", lat: -25.27, lng: 133.77 },
            { name: "China", lat: 35.86, lng: 104.19 },
            { name: "USA", lat: 37.09, lng: -95.71 },
            { name: "UK", lat: 55.37, lng: -3.43 }
        ];

        regions.forEach(r => {
            const icon = L.divIcon({
                className: 'custom-region-marker',
                html: `<div style="background-color: #4f46e5; width: 12px; height: 12px; border-radius: 50%; box-shadow: 0 0 0 4px rgba(79, 70, 229, 0.3);"></div>`,
                iconSize: [20, 20],
                iconAnchor: [10, 10]
            });

            L.marker([r.lat, r.lng], { icon: icon })
                .addTo(map)
                .on('click', () => {
                    dotnetRef.invokeMethodAsync('OnRegionClick', r.name);
                });
        });
    }
};
