function toggleSearch() {
    const searchInput = document.querySelector('.search-input');
    const searchIcon = document.querySelector('.search-icon');

   
    searchInput.classList.toggle('show');

   
    searchIcon.classList.toggle('active');
}