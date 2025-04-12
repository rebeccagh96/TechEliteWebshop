document.addEventListener("DOMContentLoaded", function () {
    const orderDetails = document.getElementById("edit-order-details");
    const orderPlaceholder = document.getElementById("edit-order-placeholder");
    const manageButtons = document.querySelectorAll(".manage-order-btn");
    const cancelButton = document.getElementById("cancel-order-edit");
    const deleteButton = document.getElementById("delete-order");

    function populateOrderDetails(button) {
        const orderId = button.getAttribute("data-order-id");
        const customerId = button.getAttribute("data-customer-id");
        const orderDate = button.getAttribute("data-order-date");

        const infoDiv = orderDetails.querySelector(".info");
        infoDiv.innerHTML = `
            <strong>KundID:</strong> ${customerId}
            <strong>OrderID:</strong> ${orderId}
            <strong>Orderdatum:</strong> ${orderDate}
        `;

        const productsTable = document.getElementById("edit-orderproducts-table").querySelector("tbody");
        productsTable.innerHTML = "";

        const products = JSON.parse(button.getAttribute("data-products"));
        products.forEach(product => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${product.ProductName}</td>
                <td>${product.Price}</td>
                <td>
                    <input type="number" name="ProductQuantities[${product.ProductId}]" value="${product.ProductQuantity}" min="0" />
                </td>
            `;
            productsTable.appendChild(row);
        });

        orderDetails.style.display = "block";
        orderPlaceholder.style.display = "none";
    }

    manageButtons.forEach(button => {
        button.addEventListener("click", function () {
            populateOrderDetails(this);
        });
    });

    cancelButton.addEventListener("click", function () {
        orderDetails.style.display = "none";
        orderPlaceholder.style.display = "block";
    });

    deleteButton.addEventListener("click", function () {
        const orderId = document.getElementById("edit-order-id").value;
        if (confirm("Är du säker på att du vill ta bort ordern?")) {
            fetch(`/Order/Delete/${orderId}`, { method: "POST" })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert(data.message);
                        location.reload();
                    } else {
                        alert("Misslyckades med att ta bort ordern.");
                    }
                })
                .catch(error => console.error("Fel:", error));
        }
    });
});
