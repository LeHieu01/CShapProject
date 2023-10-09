var dataTable;

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title'},
            { data: 'isbn'},
            { data: 'author'},
            { data: 'price'},
            { data: 'category.name' },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="product-btn align-items-center" style="display: flex; justify-content: center; align-items: center;">
                        <a href="/Admin/Product/Upsert?Id=${data}" class="btn btn-outline-primary mx-2 align-items-center"><i class="bi bi-pen"></i> Edit </a>
                        <a onClick=Delete('/admin/product/delete/${data}')  class="btn btn-outline-danger mx-2 align-items-center"><i class="bi bi-trash"></i> Delete </a>
                    </div>`
                }
            }
        ]
    });
}

$(document).ready(function () {
    loadDataTable();
});

function Delete(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success mx-3',
            cancelButton: 'btn btn-danger mx-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    /*toastr.success(data.message);*/
                    dataTable.ajax.reload();
                }
            })
            swalWithBootstrapButtons.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success')
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your imaginary file is safe :)',
                'error'
            )
        }
    })
}

