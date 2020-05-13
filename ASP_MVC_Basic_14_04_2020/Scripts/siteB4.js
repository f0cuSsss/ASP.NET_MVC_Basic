document.addEventListener('DOMContentLoaded', () => {

    var clearForm = document.getElementById("clearForm");
    if (clearForm) {
        clearForm.addEventListener('click', clearFormClick);
    } else {
        console.error("clearForm button not found");
    }
});

function clearFormClick() {
    const form = document.querySelector('.add-book-form');
    if (!form) {
        console.error("add-book-form not found");
        return;
    }

    form.reset();

    //setTimeout(() => { window.location = window.location }, 200);

    $.ajax({
        url: "/Book/ResetAddBook",
        success: function () {
            window.location = window.location
        },
        error: function () {
            alert("Ошибка");
        }
    })
}