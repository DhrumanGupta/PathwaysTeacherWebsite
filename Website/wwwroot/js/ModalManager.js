var modal;
var close;
var modalHeader;
var modalSubheader;
var modalContent;

function InitializeModal() {
    modal = document.getElementById("Modal");

    close = modal.getElementsByClassName("modal-close")[0];
    close.onclick = function () {
        CloseModal();
    }

    modalHeader = modal.getElementsByClassName("modal-header")[0];
    modalSubheader = modal.getElementsByClassName("modal-subheader")[0];
    modalContent = modal.getElementsByClassName("modal-content")[0];
}

// When the user clicks on the button, open the modal
function ShowModal(header, subheader, content) {
    modalHeader.innerHTML = header;
    modalSubheader.innerHTML = subheader;
    modalContent.innerHTML = content;

    modal.style.display = "block";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        CloseModal();
    }
}

function CloseModal() {
    modal.style.display = "none";
}