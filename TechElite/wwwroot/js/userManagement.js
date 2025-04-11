document.addEventListener("DOMContentLoaded", function () {
    const manageButtons = document.querySelectorAll(".manage-user-btn");
    const editForm = document.getElementById("edit-user-form");
    const editPlaceholder = document.getElementById("edit-placeholder");
    const cancelButton = document.getElementById("cancel-edit");
    const deleteButton = document.getElementById("delete-user");

    // Hantera "Hantera"-knappen
    manageButtons.forEach(button => {
        button.addEventListener("click", function () {
            const userId = this.getAttribute("data-user-id");
            const userName = this.getAttribute("data-user-username");
            const email = this.getAttribute("data-user-email");
            const firstName = this.getAttribute("data-user-firstname");
            const lastName = this.getAttribute("data-user-lastname");

            document.getElementById("edit-user-id").value = userId;
            document.getElementById("edit-user-username").value = userName;
            document.getElementById("edit-user-email").value = email;
            document.getElementById("edit-user-firstname").value = firstName;
            document.getElementById("edit-user-lastname").value = lastName;

            // Visa redigeringsformuläret och dölj placeholdern
            editForm.style.display = "block";
            editPlaceholder.style.display = "none";
        });
    });

    // Hantera "Avbryt"-knappen
    cancelButton.addEventListener("click", function () {
        editForm.style.display = "none";
        editPlaceholder.style.display = "block";
    });

    // Hantera formulärinlämning för redigering
    editForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(editForm);

        fetch("/Account/Edit", {
            method: "POST",
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => { throw new Error(text); });
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    location.reload();
                } else {
                    alert("Misslyckades med att uppdatera användaren.");
                }
            })
            .catch(error => {
                console.error("Fel:", error);
                alert("Ett fel inträffade: " + error.message);
            });
    });

    // Hantera "Radera"-knappen
    deleteButton.addEventListener("click", function () {
        const userId = document.getElementById("edit-user-id").value;

        if (confirm("Är du säker på att du vill radera användaren?")) {
            fetch("/Account/Delete", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ id: userId })
            })
                .then(response => {
                    if (!response.ok) {
                        return response.text().then(text => { throw new Error(text); });
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        alert(data.message);
                        location.reload();
                    } else {
                        alert("Misslyckades med att radera användaren.");
                    }
                })
                .catch(error => {
                    console.error("Fel:", error);
                    alert("Ett fel inträffade: " + error.message);
                });
        }
    });
});