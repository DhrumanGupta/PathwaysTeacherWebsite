var events;

// Called from Body's OnLoad
function InitializeEvents(model) {
    events = JSON.parse(model);
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