// FUNKTION FÖR SCROLL-EFFEKT PÅ LOGO I FOOTER
document.addEventListener("DOMContentLoaded", () => {
    const scrollToTopBtn = document.getElementById('scrollToTop');
    if (scrollToTopBtn) {
        scrollToTopBtn.addEventListener('click', function(event) { // vid klick scrollas användaren högst upp på sidan igen
            event.preventDefault(); // förhindrar standardlänk-beteendet
            window.scrollTo({
                top: 0,
                behavior: 'smooth' // skapar en scroll-effekt upp till sidan
            });
        });
    }
});

// FUNKTION FÖR POPUP-RUTA VID BOKNING
document.addEventListener("DOMContentLoaded", function () {
    var toastEl = document.getElementById("booking-toast"); // hämtar popup-elementet
    if (toastEl) {
        var toast = new bootstrap.Toast(toastEl); // skapar en bootstrap-toast-instans
        toastEl.style.display = "block"; // gör popupen synlig
        toast.show(); // startar toasten och visar popupen
    }
});
