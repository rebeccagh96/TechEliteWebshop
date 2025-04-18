document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tabs input[type='radio']");
    const contentSections = document.querySelectorAll(".content .account-tab");
    const labels = document.querySelectorAll(".tabs label");
    const hamburgerBtn = document.getElementById("hamburger-btn");
    const overlayMenu = document.getElementById("overlay-menu");

    const tabMapping = {
        "tab1": "first",
        "tab2": "second",
        "tab3": "third",
        "tab4": "fourth",
        "tab5": "fifth",
        "tab6": "sixth",
        "tab7": "seventh"
    };

    function showContent(selectedTab) {
        contentSections.forEach(section => section.style.display = "none");

        const selectedSection = document.querySelector(`.content .${tabMapping[selectedTab.id]}`);
        if (selectedSection) {
            selectedSection.style.display = "block";
        }
    }

    const adminLabels = [
        "Användare",
        "Orderhistorik",
        "Lagersaldo",
        "Inkorga",
        "Aviseringar",
        "Inställningar",
        "Övrigt"
    ];

    const userLabels = [
        "Profil",
        "Orderhistorik",
        "Hjälp",
        "Spårning",
        "Trådar",
        "Inlägg",
        "Hjälp"
    ];

    const isAdmin = document.getElementById("account-root").dataset.isAdmin === "true";

    const selectedLabels = isAdmin ? adminLabels : userLabels;

    labels.forEach((label, index) => {
        if (selectedLabels[index])
            label.textContent = selectedLabels[index];
    });

    tabs.forEach(tab => {
        tab.addEventListener("change", function () {
            showContent(this);
        });
    });

    const checkedTab = document.querySelector(".tabs input[type='radio']:checked");
    if (checkedTab) {
        showContent(checkedTab);
    }

    hamburgerBtn.addEventListener("click", function () {
        const isVisible = overlayMenu.style.display === "block";
        overlayMenu.style.display = isVisible ? "none" : "block";
    });

    overlayMenu.addEventListener("click", function (event) {
        if (event.target.tagName === "LABEL") {
            overlayMenu.style.display = "none";
        }
    });
});
