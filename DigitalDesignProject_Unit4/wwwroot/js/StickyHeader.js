var header;
var sticky;
var originalHeight;
var heightOffest;

var originalHeaderHeight;
var headerOffset;

window.onresize = function () {
    heightOffest = originalHeight - window.innerHeight;
}

window.onscroll = function () {
    UpdateSticky();
}

// Called from Body's OnLoad
function InitializeSticky() {
    header = document.getElementById("Navbar");

    if (header == undefined) {
        alert("Test");
        location.reload();
    }

    sticky = header.offsetTop;
    originalHeaderHeight = header.clientHeight;
    originalHeight = window.innerHeight;
    heightOffest = 0;
}

function UpdateSticky() {
    if (window.pageYOffset - headerOffset > sticky - heightOffest) {
        header.classList.add("sticky");
        header.classList.remove("footer");
        headerOffset = originalHeaderHeight - header.clientHeight;
    }
    else {
        header.classList.remove("sticky");
        header.classList.add("footer");
        headerOffset = originalHeaderHeight - header.clientHeight;
    }
}