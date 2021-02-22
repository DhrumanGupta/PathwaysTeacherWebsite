var stickyHeaderFooter;
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
    stickyHeaderFooter = document.getElementById("Navbar");

    sticky = stickyHeaderFooter.offsetTop;
    originalHeaderHeight = stickyHeaderFooter.clientHeight;
    originalHeight = window.innerHeight;
    heightOffest = 0;
}

function UpdateSticky() {
    if (window.pageYOffset - headerOffset > sticky - heightOffest) {
        stickyHeaderFooter.classList.add("sticky");
        stickyHeaderFooter.classList.remove("footer");
        headerOffset = originalHeaderHeight - stickyHeaderFooter.clientHeight;
    }
    else {
        stickyHeaderFooter.classList.remove("sticky");
        stickyHeaderFooter.classList.add("footer");
        headerOffset = originalHeaderHeight - stickyHeaderFooter.clientHeight;
    }
}