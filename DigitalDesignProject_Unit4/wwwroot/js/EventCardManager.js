var events;

// Called from Body's OnLoad
function InitializeEvents(model) {
    events = JSON.parse(model);
    InitializeEventModal();
}

function OnHoverEnter(element) {
    var id = element.id;

    for (i = 0; i < events.length; i++) {
        if (events[i].Id == id) {
            var event = events[i];
            element.style.backgroundImage = 'url(' + event.imgHover + ')';
            break;
        }
    }
}

function OnHoverLeave(element) {
    var id = element.id;
    for (i = 0; i < events.length; i++) {
        if (events[i].Id == id) {
            var event = events[i];
            element.style.backgroundImage = 'url(' + event.imgDefault + ')';
            break;
        }
    }
}

function OnClick(element) {
    var id = element.id;
    for (i = 0; i < events.length; i++) {
        if (events[i].Id == id) {
            var event = events[i];
            ShowModal(event);
            break;
        }
    }
}

var modal;
var close;
var modalHeader;
var modalSubheader;
var modalContent;

function InitializeEventModal() {
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
function ShowModal(event) {
    modalHeader.innerHTML = event.Name;
    modalSubheader.innerHTML = event.StartTime + " - " + event.EndTime + " (Approx.)";
    modalContent.innerHTML = event.Description;

    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
function CloseModal() {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        CloseModal();
    }
}