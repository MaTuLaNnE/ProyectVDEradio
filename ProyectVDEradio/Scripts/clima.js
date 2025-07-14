async function cargarClimaNavbar() {
    try {
        const response = await fetch('/Weather/GetWeather');
        const data = await response.json();
        document.getElementById('navbarTemp').textContent = `${Math.round(data.temp)}°C`;
        document.getElementById('navbarEstado').textContent = data.estado.charAt(0).toUpperCase() + data.estado.slice(1);

        // Usar el icono
        const iconoUrl = `https://openweathermap.org/img/wn/${data.icono}@2x.png`;
        document.getElementById('navbarClimaIcon').innerHTML = `<img src="${iconoUrl}" alt="Icono del clima" style="width: 40px; height: 40px;">`;

    } catch (e) {
        console.error("Error al obtener el clima", e);
        document.getElementById('navbarEstado').textContent = "No disponible";
        document.getElementById('navbarClimaIcon').innerHTML = '🌫️';
    }
}

document.addEventListener("DOMContentLoaded", cargarClimaNavbar);

setInterval(cargarClimaNavbar, 600000); // actualiza cada 10 minutos
