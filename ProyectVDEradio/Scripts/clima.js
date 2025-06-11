function actualizarClima() {
    const climas = [
        { temp: [22, 23, 24, 25], estado: "Soleado", emoji: "☀️" },
        { temp: [20, 21, 22], estado: "Parcialmente nublado", emoji: "🌤️" },
        { temp: [18, 19, 20], estado: "Nublado", emoji: "☁️" },
        { temp: [16, 17, 18], estado: "Lluvia ligera", emoji: "🌦️" },
        { temp: [24, 25, 26], estado: "Despejado", emoji: "🌞" },
        { temp: [19, 20, 21], estado: "Brisa marina", emoji: "🌊" }
    ];

    const humedades = [55, 60, 65, 70, 75];
    const vientos = [10, 12, 15, 18, 20];

    const climaActual = climas[Math.floor(Math.random() * climas.length)];
    const tempActual = climaActual.temp[Math.floor(Math.random() * climaActual.temp.length)];

    // Actualizar sección principal
    document.getElementById('temperatura').textContent = tempActual + '°C';
    document.getElementById('estadoClima').textContent = climaActual.estado;
    document.getElementById('humedad').textContent =
        humedades[Math.floor(Math.random() * humedades.length)] + '%';
    document.getElementById('viento').textContent =
        vientos[Math.floor(Math.random() * vientos.length)] + ' km/h';

    // Actualizar navbar
    document.getElementById('navbarTemp').textContent = tempActual + '°C';
    document.getElementById('navbarEstado').textContent = climaActual.estado;
    document.getElementById('navbarClimaIcon').textContent = climaActual.emoji;
}

// ========== INICIALIZACIÓN ==========

// Inicializar todo cuando se carga la página
document.addEventListener('DOMContentLoaded', function () {
    console.log('🎙️ Voz del Este - Sistema iniciado');

    // Inicializar datos
    actualizarClima();
    actualizarCotizaciones();
    inicializarEfectosNoticias();

    // Configurar intervalos de actualización
    setInterval(actualizarClima, 30000);        // Cada 30 segundos
    setInterval(actualizarCotizaciones, 15000); // Cada 15 segundos

    console.log('✅ Todas las funciones inicializadas correctamente');
});
function estadoSistema() {
    console.log('=== ESTADO DEL SISTEMA ===');
    console.log('Clima navbar:', document.getElementById('navbarTemp').textContent);
    console.log('Clima principal:', document.getElementById('temperatura').textContent);
    console.log('Ticker funcionando:', document.getElementById('tickerContent').innerHTML.length > 0);
    console.log('========================');
}