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
    alert(id);
    ShowModal();
}

var modal;
var close;

function InitializeEventModal() {
    modal = document.getElementById("Modal");
    close = document.getElementsByClassName("modal-close")[0];
}

// When the user clicks on the button, open the modal
function ShowModal() {
    alert('id');
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
close.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}