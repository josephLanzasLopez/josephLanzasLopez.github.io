function validarFormulario(event) {


    return validarCorreo();

}

function validarCorreo() {
    var correoElectronico = document.getElementById("CorreoElectronico").value;
    var correoRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

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