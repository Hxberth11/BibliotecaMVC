// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function(){
    $(document).on('submit', 'form.swal-delete-form', function(e){
        e.preventDefault();
        var form = $(this);
        Swal.fire({
            title: "¿Estás seguro?",
            text: "No podrás revertir este cambio!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, eliminarlo!"
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit();
            }
        }); 
    });

    $(document).on('submit', 'form.swal-save-form', function (e) {
        e.preventDefault();
        var form = $(this);
        Swal.fire({
            title: "¿Quiere guardar los cambios?",
            text: "No podrás revertir este cambio!",
            icon: "question",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, guardarlos!"
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit();
            }
        });
    });




}); 