document.addEventListener("DOMContentLoaded", function () {
    const orderDetails = document.getElementById("edit-order-details");
    const orderPlaceholder = document.getElementById("edit-order-placeholder");
    const manageButtons = document.querySelectorAll(".manage-order-btn");
    const cancelButton = document.getElementById("cancel-order-edit");
    const deleteButton = document.getElementById("delete-order");
    const saveButton = document.getElementById("save-order-edit");

    function populateOrderDetails(button) {
        const orderId = button.getAttribute("data-order-id");
        const customerId = button.getAttribute("data-customer-id");
        const orderDate = button.getAttribute("data-order-date");

        const infoDiv = orderDetails.querySelector(".info");
        infoDiv.innerHTML = `
            <input type="hidden" id="edit-order-id" value="${orderId}" />
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
                <td>${product.productName}</td>
                <td>${product.price}</td>
                <td>
                    <input type="number" name="ProductQuantities[${product.productId}]" value="${product.productQuantity}" min="0" />
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
        const orderIdInput = document.getElementById("edit-order-id");
        if (!orderIdInput) {
            console.error("Order-ID kunde inte hittas!");
            return;
        }
        const orderId = orderIdInput.value;
        if (confirm("Är du säker på att du vill ta bort ordern?")) {
            fetch('/Order/Delete', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ OrderId: parseInt(orderId) })
            })
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

    saveButton.addEventListener("click", function (event) {
        event.preventDefault();
        const orderIdInput = document.getElementById("edit-order-id");
        if (!orderIdInput) {
            console.error("Order-ID hittades inte!");
            return;
        }
        const orderId = orderIdInput.value;

        let updatedOrder = {
            OrderId: orderId,
            OrderProducts: []
        };

        document.querySelectorAll("#edit-orderproducts-table input[type='number']").forEach(input => {
            const matches = input.name.match(/\[(\d+)\]/);
            if (matches) {
                const productId = parseInt(matches[1]);
                const productQuantity = parseInt(input.value);
                updatedOrder.OrderProducts.push({
                    ProductId: productId,
                    ProductQuantity: productQuantity
                });
            }
        });

        fetch('/Order/Edit', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedOrder)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    location.reload();
                } else {
                    alert("Misslyckades med att spara ändringar.");
                }
            })
            .catch(error => console.error("Fel:", error));
    });
});
