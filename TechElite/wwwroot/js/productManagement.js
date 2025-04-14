document.addEventListener("DOMContentLoaded", function () {
    const productForm = document.getElementById("edit-product-form");
    const productPlaceholder = document.getElementById("product-edit-placeholder");
    const manageButtons = document.querySelectorAll(".manage-product-btn");
    const addProductBtn = document.querySelector(".add-product-btn");
    const cancelButton = document.getElementById("cancel-product-edit");
    const deleteButton = document.getElementById("delete-product");
    const imageInput = document.getElementById("edit-product-image");
    const currentImagePreview = document.getElementById("current-image-preview");
    const newImagePreview = document.getElementById("new-image-preview");

    // Placeholder för när ingen bild valts
    const placeholderSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 50 50">
        <rect width="50" height="50" fill="#f2f2f2" />
        <text x="25" y="25" font-family="Arial" font-size="6" text-anchor="middle" dominant-baseline="middle" fill="#999">Ingen bild</text>
    </svg>;`;

    // Funktion för att populeara formuläret
    function populateProductForm(button) {
        const productId = button.getAttribute("data-product-id") || "";
        document.getElementById("edit-product-id").value = productId;
        document.getElementById("edit-product-name").value = button.getAttribute("data-product-name") || "";
        document.getElementById("edit-product-description").value = button.getAttribute("data-product-description") || "";
        document.getElementById("edit-product-price").value = button.getAttribute("data-product-price") || "";
        document.getElementById("edit-product-quantity").value = button.getAttribute("data-product-quantity") || "";
        document.getElementById("edit-product-department").value = button.getAttribute("data-product-department-id") || "";

        currentImagePreview.innerHTML = "";
        newImagePreview.innerHTML = "";

        // Om en existerande produkt redigeras hämta bild
        if (productId) {
            fetch(`/Product/GetImage/${productId}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success && data.svgContent) {
                        // Visa SVGn direkt i förhandsvisningen
                        currentImagePreview.innerHTML = `
                            <div style="width: 50px; height: 50px; overflow: hidden; border: 1px solid #ccc; display: flex; align-items: center; justify-content: center;">
                                ${data.svgContent}
                            </div>
                            <p style="margin-top: 5px; font-size: 8px;">Nuvarande SVG</p>
                        `;
                    } else {
                        // Visa placeholder om ingen bild finns
                        newImagePreview.innerHTML = `
                            <div style="width: 50px; height: 50px; overflow: hidden; border: 1px solid #4CAF50; display: flex; align-items: center; justify-content: center;">
                                ${svgContent}
                            </div>
                            <p style="margin-top: 5px; font-size: 8px; color: #4CAF50;">Ny SVG (förhandsvisning)</p>
                        `;

                    }
                })
                .catch(error => {
                    console.error("Fel vid hämtning av SVG:", error);
                    currentImagePreview.innerHTML = `
                        <div style="max-width: 50px; max-height: 50px; border: 1px dashed #ccc; padding: 5px;">
                            ${placeholderSvg}
                        </div>
                        <p style="margin-top: 5px; font-size: 8px; color: #cc0000;">Kunde inte hämta bilden</p>
                    `;
                });
        }

        productForm.style.display = "block";
        if (productPlaceholder) {
            productPlaceholder.style.display = "none";
        }
    }

    // Hantera bildinmatning
    if (imageInput) {
        imageInput.addEventListener("change", function () {
            newImagePreview.innerHTML = "";

            if (this.files && this.files[0]) {
                // Kontrollera att filen är en SVG
                const file = this.files[0];
                if (file.type !== 'image/svg+xml') {
                    newImagePreview.innerHTML = `
                        <p style="color: #cc0000;">Endast SVG-filer stöds</p>
                    `;
                    this.value = '';
                    return;
                }

                const reader = new FileReader();

                reader.onload = function (e) {
                    const svgContent = e.target.result;

                    newImagePreview.innerHTML = `
                        <div style="width: 50px; height: 50px; overflow: hidden; border: 1px solid #ccc; display: flex; align-items: center; justify-content: center;">
                            ${svgContent}
                        </div>
                        <p style="margin-top: 5px; font-size: 8px; color: #4CAF50;">Ny SVG (förhandsvisning)</p>
     
                        
                   `;
                };

                reader.readAsText(file);
            }
        });
    }

    manageButtons.forEach(button => {
        button.addEventListener("click", function () {
            populateProductForm(this);
        });
    });
    
    if (addProductBtn) {
        addProductBtn.addEventListener("click", function () {

            document.getElementById("edit-product-id").value = "";
            document.getElementById("edit-product-name").value = "";
            document.getElementById("edit-product-description").value = "";
            document.getElementById("edit-product-price").value = "";
            document.getElementById("edit-product-quantity").value = "";


            const departmentSelect = document.getElementById("edit-product-department");
            if (departmentSelect.options.length > 0) {
                departmentSelect.selectedIndex = 0;
            }

            
            currentImagePreview.innerHTML = `
                <div style="max-width: 50px; max-height: 50px; border: 1px dashed #ccc; padding: 5px;">
                    ${placeholderSvg}
                </div>
                <p style="margin-top: 5px; font-size: 8px;">Ingen bild vald ännu</p>
            `;
            newImagePreview.innerHTML = "";

            productForm.style.display = "block";
            if (productPlaceholder) {
                productPlaceholder.style.display = "none";
            }
        });
    }


    if (cancelButton) {
        cancelButton.addEventListener("click", function () {
            productForm.style.display = "none";
            if (productPlaceholder) {
                productPlaceholder.style.display = "block";
            }
        });
    }

    productForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const formData = new FormData(productForm);


        console.log("Form data being sent:");
        for (let [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }

        fetch("/Product/Save", {
            method: "POST",
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        console.error("Server error response:", text);
                        throw new Error(text);
                    });
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    location.reload();
                } else {
                    alert("Misslyckades med att uppdatera produkten: " + (data.message || ""));
                }
            })
            .catch(error => {
                console.error("Fel:", error);
                alert("Ett fel inträffade: " + error.message);
            });
    });


    if (deleteButton) {
        deleteButton.addEventListener("click", function () {
            const productId = document.getElementById("edit-product-id").value;

            if (!productId) {
                alert("Kan inte radera en produkt som inte är sparad ännu.");
                return;
            }

            if (confirm("Är du säker på att du vill radera produkten?")) {
                fetch("/Product/Delete", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ ProductId: productId })
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
                            alert("Misslyckades med att radera produkten: " + (data.message || ""));
                        }
                    })
                    .catch(error => {
                        console.error("Fel:", error);
                        alert("Ett fel inträffade: " + error.message);
                    });
            }
        });
    }
});