"use strict";

document.addEventListener("DOMContentLoaded", function () {
    var tableContainer = document.getElementById("table-container");
    var scrollMessage = document.getElementById("scroll-message");

    function checkScroll() {
        if (tableContainer.scrollWidth > tableContainer.clientWidth) {
            scrollMessage.classList.remove("d-none");
        } else {
            scrollMessage.classList.add("d-none");
        }
    }

    checkScroll(); // kontrollerar vid sidans laddning
    window.addEventListener("resize", checkScroll); // kontrollerar vid ändring av skärmstorlek
});