function showNotifications() {
    var notis = document.getElementById("notification-article");
    var badge = document.getElementById("notification-badge");

    if (notis.style.display == "flex") notis.style.display = "none";
    else {
        notis.style.display = "flex";
        if (badge) {
            badge.style.display = "none";
        }
    }
}

function SearchForum() {
    const searchInput = document.querySelector('.search-input-forum');
    const searchIcon = document.querySelector('.search-icon');

    if (searchInput.style.display == "inline-block") searchInput.style.display = "none";
    else searchInput.style.display = "inline-block";
}