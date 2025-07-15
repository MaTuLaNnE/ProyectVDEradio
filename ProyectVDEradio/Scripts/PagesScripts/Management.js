function toggleCard(header) {
    const card = header.parentElement;
    const isActive = card.classList.contains('active');


    document.querySelectorAll('.gestion-card.active').forEach(activeCard => {
        activeCard.classList.remove('active');
    });

    // Si la tarjeta no estaba activa, abrirla
    if (!isActive) {
        card.classList.add('active');
    }
}



// Efecto de hover mejorado para los botones
document.querySelectorAll('.crud-btn').forEach(btn => {
    btn.addEventListener('mouseenter', function () {
        this.style.transform = 'translateY(-3px) scale(1.02)';
    });

    btn.addEventListener('mouseleave', function () {
        this.style.transform = 'translateY(0) scale(1)';
    });
});





// Pa crear el usuario


// Script para mostrar/ocultar campos de cliente
document.getElementById("roleSelect").addEventListener("change", function () {
    var valor = this.value;
    var clienteCampos = document.getElementById("clienteCampos");

    if (valor === "3") { // ID del rol Cliente
        clienteCampos.classList.add("show");
    } else {
        clienteCampos.classList.remove("show");
    }
});

// Efectos de interacción mejorados
document.querySelectorAll('.form-control').forEach(input => {
    input.addEventListener('focus', function () {
        this.parentElement.style.transform = 'translateY(-2px)';
    });

    input.addEventListener('blur', function () {
        this.parentElement.style.transform = 'translateY(0)';
    });
});

//pa ver los usuarios

document.getElementById('searchInput').addEventListener('input', function () {
    const searchTerm = this.value.toLowerCase();
    const tableRows = document.querySelectorAll('.modern-table tbody tr');

    tableRows.forEach(row => {
        const userName = row.cells[0].textContent.toLowerCase();
        const email = row.cells[1].textContent.toLowerCase();
        const role = row.cells[3].textContent.toLowerCase();

        if (userName.includes(searchTerm) || email.includes(searchTerm) || role.includes(searchTerm)) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
});

// Animaciones escalonadas para las filas
document.querySelectorAll('.modern-table tbody tr').forEach((row, index) => {
    row.style.animationDelay = `${index * 0.1}s`;
});

// Confirmación para eliminar
document.querySelectorAll('.btn-danger').forEach(btn => {
    btn.addEventListener('click', function (e) {
        if (!confirm('¿Estás seguro de que deseas eliminar este usuario?')) {
            e.preventDefault();
        }
    });
});

// Efectos de hover mejorados
document.querySelectorAll('.action-btn').forEach(btn => {
    btn.addEventListener('mouseenter', function () {
        this.style.transform = 'translateY(-2px) scale(1.05)';
    });

    btn.addEventListener('mouseleave', function () {
        this.style.transform = 'translateY(0) scale(1)';
    });
});