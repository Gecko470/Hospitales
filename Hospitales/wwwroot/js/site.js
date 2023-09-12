// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function mostrarModal(msg) {
    return Swal.fire({
        title: msg,
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: 'Aceptar',
        denyButtonText: `Cancelar`,
    });
}

function exito(msg) {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: msg,
        showConfirmButton: false,
        timer: 1500
    })
}

function error() {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Algo ha ido mal!'
    })
}

function Imprimir(titulo) {

    let table = "<h1>" + titulo + "</h1>"
    table += document.getElementById("table").outerHTML;
    let trCheck = document.getElementsByClassName("trCheck");
    Array.from(trCheck).forEach(item => {
        table = table.replace(item.outerHTML, "");
    });
    let pagina = window.document.body;
    var ventana = window.open();
    ventana.document.write(table);
    ventana.print();
    ventana.close();
    window.document.body = pagina;
}

function Excel() {
    document.getElementById("tipo").value = "excel"
    document.getElementById("frmExportar").submit();
}

function Word() {
    document.getElementById("tipo").value = "word"
    document.getElementById("frmExportar").submit();
}

function PDF() {
    document.getElementById("tipo").value = "pdf"
    document.getElementById("frmExportar").submit();
}

function DescargarPDF(controller, titulo) {
    let frm = new FormData();
    let checks = document.getElementsByName("nombrePropiedades");
    for (var i = 0; i < checks.length; i++) {
        if (checks[i].checked == true) {
            frm.append("nombrePropiedades[]", checks[i].value)
        }
    }

    $.ajax({
        type: "POST",
        url: "/" + controller + "/DescargarPDF",
        data: frm,
        contentType: false,
        processData: false,
        success: function (data) {
            let a = document.createElement("a");
            a.href = data;
            a.download = titulo;
            a.click();
        }

    });
}

//CARGAMOS DATATABLES

/*window.onload = () => {
    $(document).ready(function () {
        $('#table').DataTable();
    });
}*/

function Tabla(url, campos, controller, modal = false, editar, eliminar) {
    let tbody = document.getElementById("tbody");
    let contenido = "";

    $.get(url, (resp) => {
        resp.forEach(item => {
            contenido += "<tr>";

            campos.forEach((campo) => {
                contenido += "<td>" + item[campo] + "</td>"
            });

            if (modal == false) {
                if (editar == "1" && eliminar == "1") {
                    contenido += `<td>
                          <a href = '/${controller}/Edit/${item[campos[0]]}'><i class='fa fa-pencil-square-o text-primary' aria-hidden='true'></i></a>
                          <span onclick='Eliminar(${item[campos[0]]})'><i class='fa fa-trash-o text-danger' aria-hidden='true'></i></span>
                          </td>`;
                }
                else if (editar == "1" && eliminar == "0") {
                    contenido += `<td>
                          <a href = '/${controller}/Edit/${item[campos[0]]}'><i class='fa fa-pencil-square-o text-primary' aria-hidden='true'></i></a>
                          </td>`;
                }
                else if (editar == "0" && eliminar == "1") {
                    contenido += `<td>
                          <span onclick='Eliminar(${item[campos[0]]})'><i class='fa fa-trash-o text-danger' aria-hidden='true'></i></span>
                          </td>`;
                }
                else if (editar == "0" && eliminar == "0") {
                     contenido += `<td></td>`;
                }
                
            }
            else {
                 if (editar == "1" && eliminar == "1") {
                    contenido += `<td>
                          <span onclick='Edit(${item[campos[0]]})' data-bs-toggle="modal" data-bs-target="#exampleModal"><i class='fa fa-pencil-square-o text-primary' aria-hidden='true'></i></span>
                          <span onclick='Eliminar(${item[campos[0]]})'><i class='fa fa-trash-o text-danger' aria-hidden='true'></i></span>
                          </td>`;
                }
                else if (editar == "1" && eliminar == "0") {
                   contenido += `<td>
                          <span onclick='Edit(${item[campos[0]]})' data-bs-toggle="modal" data-bs-target="#exampleModal"><i class='fa fa-pencil-square-o text-primary' aria-hidden='true'></i></span>
                          </td>`;
                }
                else if (editar == "0" && eliminar == "1") {
                   contenido += `<td>
                          <span onclick='Eliminar(${item[campos[0]]})'><i class='fa fa-trash-o text-danger' aria-hidden='true'></i></span>
                          </td>`;
                }
                else if (editar == "0" && eliminar == "0") {
                     contenido += `<td> </td>`;
                }
            }
           
            contenido += "</tr>"
        });

        $('#table').DataTable().clear();
        $('#table').DataTable().destroy();

        tbody.innerHTML = contenido;
        $('#table').DataTable();
        console.table(resp);
    });
}
