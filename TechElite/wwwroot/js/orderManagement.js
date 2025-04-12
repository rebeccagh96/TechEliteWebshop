document.addEventListener("DOMContentLoaded", function () {
    const orderForm = document.getElementById("edit-order-form");
    const orderPlaceholder = document.getElementById("edit-order-placeholder");
    const manageButtons = document.querySelectorAll(".manage-order-btn");
    const cancelButton = document.getElementById("cancel-order-edit");
    const deleteButton = document.getElementById("delete-order");

    function populateOrderForm(button) {
        const orderId = button.getAttribute("data-order-id");
        const customerId = button.getAttribute("data-customer-id");
        const orderDate = button.getAttribute("data-order-date");
        const totalPrice = button.getAttribute("data-total-price");

        document.getElementById("edit-order-id").value = orderId;
        document.getElementById("edit-customer-id").value = customerId;
        document.getElementById("edit-order-date").value = orderDate;
        document.getElementById("edit-total-price").value = totalPrice;

        const productsTable = document.getElementById("edit-products-table").querySelector("tbody");
        productsTable.innerHTML = ""; 

        const products = JSON.parse(button.getAttribute("data-products"));
        products.forEach(product => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${product.ProductName}</td>
                <td>${product.Price}</td>
                <td>
                    <input type="number" name="Products[${product.ProductId}].Quantity" value="${product.Quantity}" />
                </td>
                <td>
                    <button type="button" class="remove-product-btn" data-product-id="${product.ProductId}">Ta bort</button>
                </td>
            `;
            productsTable.appendChild(row);
        });

        orderForm.style.display = "block";
        orderPlaceholder.style.display = "none";
    }

    manageButtons.forEach(button => {
        button.addEventListener("click", function () {
            populateOrderForm(this);
        });
    });

    cancelButton.addEventListener("click", function () {
        orderForm.style.display = "none";
        orderPlaceholder.style.display = "block";
    });

    deleteButton.addEventListener("click", function () {
        const orderId = document.getElementById("edit-order-id").value;
        if (confirm("Är du säker på att du vill avbryta ordern?")) {
            fetch(`/Order/Delete/${orderId}`, { method: "POST" })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert(data.message);
                        location.reload();
                    } else {
                        alert("Misslyckades med att avbryta ordern.");
                    }
                })
                .catch(error => console.error("Fel:", error));
        }
    });
});
