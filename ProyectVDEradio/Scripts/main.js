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

// ========== FUNCIONES DE COTIZACIONES ==========

// Actualizar cotizaciones en el ticker
function actualizarCotizaciones() {
    const cotizaciones = [
        { nombre: "💱 USD/UYU", valor: (39.50 + (Math.random() - 0.5) * 2).toFixed(2) },
        { nombre: "💶 EUR/UYU", valor: (42.70 + (Math.random() - 0.5) * 3).toFixed(2) },
        { nombre: "🇧🇷 BRL/UYU", valor: (7.90 + (Math.random() - 0.5) * 0.5).toFixed(2) },
        { nombre: "🇦🇷 ARS/UYU", valor: (0.038 + (Math.random() - 0.5) * 0.005).toFixed(3) },
        { nombre: "📈 BITCOIN", valor: "$" + (67450 + (Math.random() - 0.5) * 5000).toFixed(0) },
        { nombre: "💎 ETHEREUM", valor: "$" + (3210 + (Math.random() - 0.5) * 500).toFixed(0) },
        { nombre: "🏦 MERVAL", valor: (1847230 + (Math.random() - 0.5) * 50000).toFixed(0) },
        { nombre: "📊 DOW JONES", valor: (38905 + (Math.random() - 0.5) * 1000).toFixed(0) }
    ];

    let tickerHTML = '';
    cotizaciones.forEach(item => {
        const trend = Math.random() > 0.5 ? '▲' : '▼';
        const className = trend === '▲' ? 'currency-up' : 'currency-down';
        tickerHTML += `<span class="ticker-item">${item.nombre}: <span class="${className}">${item.valor} ${trend}</span></span>`;
    });

    document.getElementById('tickerContent').innerHTML = tickerHTML;
}

// ========== EFECTOS VISUALES ==========

// Efecto de hover en las tarjetas de noticias
function inicializarEfectosNoticias() {
    const noticias = document.querySelectorAll('.noticia-item');
    noticias.forEach(noticia => {
        noticia.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-10px) scale(1.02)';
        });

        noticia.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
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

// ========== FUNCIONES ADICIONALES ==========

// Función para mostrar mensaje de bienvenida
function mostrarBienvenida() {
    console.log(`
    🎙️ ¡Bienvenido a Voz del Este! 🎙️
    📻 Tu radio favorita de Maldonado
    🌤️ Clima actualizado cada 30 segundos
    💱 Cotizaciones actualizadas cada 15 segundos
    `);
}

// Función para debugging
function estadoSistema() {
    console.log('=== ESTADO DEL SISTEMA ===');
    console.log('Clima navbar:', document.getElementById('navbarTemp').textContent);
    console.log('Clima principal:', document.getElementById('temperatura').textContent);
    console.log('Ticker funcionando:', document.getElementById('tickerContent').innerHTML.length > 0);
    console.log('========================');
}

// Mostrar bienvenida al cargar
mostrarBienvenida();

// Hacer funciones disponibles globalmente para debugging
window.estadoSistema = estadoSistema;
window.actualizarClima = actualizarClima;
window.actualizarCotizaciones = actualizarCotizaciones;
