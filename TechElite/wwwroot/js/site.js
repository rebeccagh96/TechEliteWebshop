document.addEventListener("DOMContentLoaded", function () {
    const searchIcon = document.querySelector(".search-icon");
    const searchInput = document.querySelector(".search-input");
    const searchContainer = document.querySelector(".search");

    // Visa eller dölj sökfältet när sökikonen klickas
    searchIcon.addEventListener("click", function () {
        searchContainer.classList.toggle("show-search");
        searchInput.focus(); // Sätt fokus på inputfältet när det visas
    });
});