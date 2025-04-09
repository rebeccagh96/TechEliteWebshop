document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tabs input[type='radio']");
    const contentSections = document.querySelectorAll(".content .prof-tab");

    function showContent(selectedTab) {
        contentSections.forEach(section => section.style.display = "none");

        const tabMapping = {
            "tab1": "first",
            "tab2": "konto",
            "tab3": "aktivitet",
            "tab4": "inställningar",
            "tab5": "orderhistorik",
            "tab6": "recensioner",
            "tab7": "recensioner"
        };

        const selectedSection = document.querySelector(`.content .${tabMapping[selectedTab.id]}`);
        if (selectedSection) {
            selectedSection.style.display = "block";
        }
    }

    tabs.forEach(tab => {
        tab.addEventListener("change", function () {
            showContent(this);
        });
    });

    const checkedTab = document.querySelector(".tabs input[type='radio']:checked");
    if (checkedTab) {
        showContent(checkedTab);
    }
});

