window.globeViz = {
    globe: null,

    init: function (containerId, dotnetRef) {
        // Wait for Globe to be loaded
        if (typeof Globe === 'undefined') {
            setTimeout(() => this.init(containerId, dotnetRef), 100);
            return;
        }

        const container = document.getElementById(containerId);
        if (!container) return;

        // Data Points with Stats
        const origin = { name: "Brisbane (HQ)", lat: -27.4698, lng: 153.0251, size: 1.5, color: '#ffffff' };

        const keyMarkets = [
            { name: "Australia (National)", lat: -25.2744, lng: 133.7751, size: 0.5, listings: 3, markets: "RealEstate.com.au, Domain" },
            { name: "Canberra", lat: -35.2809, lng: 149.1300, size: 0.3, listings: 2, markets: "Allhomes" },
            { name: "Perth", lat: -31.9505, lng: 115.8605, size: 0.3, listings: 2, markets: "REIWA, Domain" },
            { name: "Melbourne", lat: -37.8136, lng: 144.9631, size: 0.3, listings: 3, markets: "RealEstateView, Homely" },
            { name: "China", lat: 35.8617, lng: 104.1954, size: 0.8, listings: 1, markets: "Juwai, WeChat" },
            { name: "USA", lat: 37.0902, lng: -95.7129, size: 0.8, listings: 2, markets: "Zillow, Realtor.com" },
            { name: "UK", lat: 51.5074, lng: -0.1278, size: 0.8, listings: 4, markets: "Rightmove, Zoopla" },
            { name: "Europe", lat: 48.8566, lng: 2.3522, size: 0.6, listings: 1, markets: "ImmoScout24" },
            { name: "Singapore", lat: 1.3521, lng: 103.8198, size: 0.6, listings: 1, markets: "PropertyGuru" },
            { name: "UAE", lat: 23.4241, lng: 53.8478, size: 0.6, listings: 1, markets: "Bayut, Dubizzle" }
        ];

        // Format points for Globe.gl
        const points = keyMarkets.map(p => ({
            ...p,
            color: '#10b981', // Emerald Green for active markets
            maxR: p.size * 4,
            propagationSpeed: 0.5 + Math.random() * 0.5,
            repeatPeriod: 800 + Math.random() * 1000
        }));

        // Arcs from Brisbane to all markets
        const arcs = keyMarkets.map(p => ({
            startLat: origin.lat,
            startLng: origin.lng,
            endLat: p.lat,
            endLng: p.lng,
            color: ['rgba(255, 255, 255, 0.5)', 'rgba(16, 185, 129, 0.8)']
        }));

        this.globe = Globe()
            (container)
            .globeImageUrl('//unpkg.com/three-globe/example/img/earth-blue-marble.jpg')
            .bumpImageUrl('//unpkg.com/three-globe/example/img/earth-topology.png')
            .backgroundImageUrl('//unpkg.com/three-globe/example/img/night-sky.png')
            .width(container.clientWidth || 500)
            .height(container.clientHeight || 500)
            // Custom HTML Markers
            .htmlElementsData([origin, ...points])
            .htmlLat('lat')
            .htmlLng('lng')
            .htmlElement(d => {
                const el = document.createElement('div');
                el.className = 'globe-marker';
                el.style.color = d.lat === origin.lat ? '#fbbf24' : 'white'; // Gold for HQ
                el.innerHTML = `
                    <div class="marker-dot"></div>
                    <div class="marker-label">${d.name}</div>
                `;

                // Add click handler to marker since it sits on top
                el.onclick = () => {
                    if (dotnetRef) dotnetRef.invokeMethodAsync('OnRegionClick', d.name);
                    this.globe.pointOfView({ lat: d.lat, lng: d.lng, altitude: 1.5 }, 1500);
                };

                return el;
            })

            // Refine Arcs
            .arcsData(arcs)
            .arcColor('color')
            .arcDashLength(0.4)
            .arcDashGap(0.2)
            .arcDashAnimateTime(1500) // Faster flow
            .arcStroke(0.8) // Thicker lines

            // Atmosphere Tweaks
            .atmosphereColor('#60a5fa') // Lighter blue for better glow
            .atmosphereAltitude(0.2)
            .pointLabel(d => {
                if (!d.listings && !d.markets) return null; // Don't show for simple points
                return `
                    <div style="background: rgba(15, 23, 42, 0.9); color: white; border: 1px solid rgba(16, 185, 129, 0.5); border-radius: 8px; padding: 12px; font-family: sans-serif; backdrop-filter: blur(4px);">
                        <div style="font-weight: bold; margin-bottom: 4px; color: #10b981; font-size: 1.1em;">${d.name}</div>
                        <div style="font-size: 0.9em; margin-bottom: 4px;">Active Listings: <strong style="color: white;">${d.listings || 1}</strong></div>
                        ${d.markets ? `<div style="font-size: 0.8em; color: #94a3b8;">${d.markets}</div>` : ''}
                    </div>
                `;
            })
            .onPointClick(point => {
                if (dotnetRef) dotnetRef.invokeMethodAsync('OnRegionClick', point.name);
                this.globe.pointOfView({ lat: point.lat, lng: point.lng, altitude: 1.5 }, 1500);
            })
            .onPointHover(point => {
                container.style.cursor = point ? 'pointer' : 'default';
                this.globe.controls().autoRotate = !point;
            });

        // Auto-rotate
        this.globe.controls().autoRotate = true;
        this.globe.controls().autoRotateSpeed = 0.6;

        // Set initial view safely
        this.globe.pointOfView({ lat: -25, lng: 135, altitude: 2.0 });

        // Force resize handler immediately
        if (this.globe) {
            this.globe.width(container.offsetWidth);
            this.globe.height(container.offsetHeight);
        }

        // Handle window resize
        if (!window.globeResizeListener) {
            window.globeResizeListener = true;
            window.addEventListener('resize', () => {
                if (this.globe) {
                    this.globe.width(container.offsetWidth);
                    this.globe.height(container.offsetHeight);
                }
            });
        }
    }
};
