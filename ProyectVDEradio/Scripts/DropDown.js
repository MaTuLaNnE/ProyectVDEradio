// Función para manejar el dropdown
function inicializarDropdown() {
    const dropdown = document.querySelector('.dropdown');
    const dropdownToggle = dropdown.querySelector('.dropdown-toggle');

    // Toggle dropdown al hacer click
    dropdownToggle.addEventListener('click', function (e) {
        e.preventDefault();
        dropdown.classList.toggle('show');
    });

    // Cerrar dropdown al hacer click fuera
    document.addEventListener('click', function (e) {
        if (!dropdown.contains(e.target)) {
            dropdown.classList.remove('show');
        }
    });

    // Cerrar dropdown al presionar Escape
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            dropdown.classList.remove('show');
        }
    });
}

// Función existente de efectos de noticias
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

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function () {
    inicializarDropdown();
    inicializarEfectosNoticias();
});