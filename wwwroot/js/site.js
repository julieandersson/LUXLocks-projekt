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
