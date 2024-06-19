// Obtener la fecha actual
var hoy = new Date().toISOString().split('T')[0];
// Establecer la fecha mínima del campo de entrada a la fecha actual
document.getElementById("Date").setAttribute("min", hoy);

function validarFormulario(event) {


    if (validarNombre() && validarTelefono() && validarCorreo() && validarFecha() && validarNumeroPersonas()) {
        
        mensajeExito();
        setTimeout(function () {
            document.getElementById('formReservation').submit();
        }, 3000);

        
        return true;
    } else {
        return false;
    }

    /*return validarCedula() && validarNombre() && validarCorreo() && validarContrasenna();*/

}

function validarNombre() {
    var nombre = document.getElementById('Nombre').value;

    // Verificar si el nombre está vacío
    if (nombre.trim() === "") {
        Swal.fire({
            icon: "error",
            text: "Por favor, ingrese su nombre.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    // Verificar si el nombre tiene más de 85 caracteres
    if (nombre.length > 85) {
        Swal.fire({
            icon: "error",
            text: "El nombre no puede tener más de 85 caracteres.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }
    return true;
}

function validarTelefono() {
    var telefono = document.getElementById('Telefono').value;

    // Verificar si el teléfono está vacío
    if (telefono.trim() === "") {
        Swal.fire({
            icon: "error",
            text: "Por favor, ingrese su número de teléfono.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    // Verificar si el teléfono contiene caracteres no numéricos
    if (!/^\d+$/.test(telefono)) {
        Swal.fire({
            icon: "error",
            text: "El número de teléfono solo puede contener números.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    // Verificar si el teléfono no tiene al menos 8 dígitos
    if (telefono.length < 8) {
        Swal.fire({
            icon: "error",
            text: "El número de teléfono debe tener al menos 8 dígitos.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    

    return true;
}

function mensajeExito() {
    Swal.fire({
        icon: "success",
        text: "Agregado con éxito",
        background: "#242424",
        width: "30%",
        showConfirmButton: false,
    }).then(function () {
        // Después de cerrar la alerta, enviar el formulario
        document.getElementById('formReservation').submit();
    });
}

function validarCorreo() {
    var correoElectronico = document.getElementById("Correo").value.trim(); 
    var correoRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!correoElectronico) {
        Swal.fire({
            icon: "error",
            text: "Por favor, ingrese un correo electrónico.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    if (!correoRegex.test(correoElectronico)) {
        Swal.fire({
            icon: "error",
            text: "Por favor, ingrese un correo electrónico válido.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    return true;
}

function validarFecha() {
    var fechaReserva = document.getElementById("Date").value.trim(); // Elimina espacios en blanco al inicio y al final

    if (!fechaReserva) {
        Swal.fire({
            icon: "error",
            text: "Por favor, ingrese una fecha de reserva.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }


    return true;
}

function validarNumeroPersonas() {
    var numeroPersonas = document.getElementById("NumeroPersonas").value.trim(); // Elimina espacios en blanco al inicio y al final

    if (!numeroPersonas) {
        Swal.fire({
            icon: "error",
            text: "Por favor, ingrese el número de personas.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    if (parseInt(numeroPersonas) > 80) {
        Swal.fire({
            icon: "error",
            text: "El número de personas no puede ser mayor a 80.",
            background: "#242424",
            width: "30%",
            confirmButtonColor: '#24a0ed',
        });
        return false;
    }

    return true;
}