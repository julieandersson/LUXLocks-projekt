"use strict";

document.addEventListener("DOMContentLoaded", function () {
    let dateInput = document.getElementById("appointmentDate");
    let errorMessage = document.getElementById("dateError"); // hämtar felmeddelande-elementet

    // sätter standardvärde i kalendern till morgondagens datum klockan 10:00 (lokal tid)
    let now = new Date();
    now.setDate(now.getDate() + 1); // + en dag (morgondagen)
    now.setHours(10, 0, 0, 0); // sätter tid till kl 10:00

    // formaterar till YYYY-MM-DDTHH:MM i lokal tid
    dateInput.value = formatDateTimeLocal(now);

    // validerar endast när användaren lämnar fältet
    dateInput.addEventListener("blur", function () {
        let selectedDateTime = new Date(dateInput.value);
        let hour = selectedDateTime.getHours();
        let minute = selectedDateTime.getMinutes();

        if (hour < 10 || hour >= 18 || minute !== 0) {
            errorMessage.textContent = "Du kan endast boka mellan 10:00 - 17:00 och välja hela timmar (t.ex. 10:00, 11:00, 12:00 osv).";
            errorMessage.style.display = "block"; // visar felmeddelandet om användaren anger en tid utanför tidsintervallet
            
            // behåller datumet men sätter tiden till 00:00 så användaren får manuellt välja om tid
            selectedDateTime.setHours(0, 0, 0, 0);

            // uppdaterar fältet med den korrigerade tiden i lokal tid
            dateInput.value = formatDateTimeLocal(selectedDateTime);
        } else {
            errorMessage.style.display = "none"; // döljer felmeddelandet om tiden är giltig
        }
    });

    // funktion för att formatera datumet till YYYY-MM-DDTHH:MM i lokal tid
    function formatDateTimeLocal(date) {
        let year = date.getFullYear();
        let month = String(date.getMonth() + 1).padStart(2, '0'); // lägger till nolla vid behov
        let day = String(date.getDate()).padStart(2, '0');
        let hours = String(date.getHours()).padStart(2, '0');
        let minutes = String(date.getMinutes()).padStart(2, '0');

        return `${year}-${month}-${day}T${hours}:${minutes}`;
    }
});