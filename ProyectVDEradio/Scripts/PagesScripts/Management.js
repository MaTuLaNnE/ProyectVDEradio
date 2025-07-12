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

//document.getElementById("roleSelect").addEventListener("change", function () {
//    var valor = this.value;
//    var clienteCampos = document.getElementById("clienteCampos");
//    if (valor === "3") { // ID del rol Cliente
//        clienteCampos.style.display = "block";
//    } else {
//        clienteCampos.style.display = "none";
//    }
//});



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