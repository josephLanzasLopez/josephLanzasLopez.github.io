

function validarFormulario(event) {


    if (validarNombre() && validarTelefono() && validarFecha() && validarContrasenna() && validarConfirmarContrasenna()) {

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

