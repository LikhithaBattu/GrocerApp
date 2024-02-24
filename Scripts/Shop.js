document.addEventListener("DOMContentLoaded", function () {
    const addToCartButtons = document.querySelectorAll(".btn-add-to-cart");
    const cartCountElement = document.getElementById("cart-count");
    const cart = []; // Simulated cart data

    function updateCartCount() {
        cartCountElement.innerText = cart.length;
    }

    function addToCart(productId, quantity) {
        // Check if the product is already in the cart
        const existingProduct = cart.find(item => item.productId === productId);

        if (existingProduct) {
            // Update the quantity of the existing product
            existingProduct.quantity += quantity;
        } else {
            // Add a new product to the cart
            cart.push({ productId, quantity });
        }

        // Update the cart count element
        updateCartCount();
    }

    function updateCartCount() {
        const cartCountElement = document.getElementById("cart-count");
        if (cartCountElement) {
            // Calculate the total quantity of items in the cart
            const totalQuantity = cart.reduce((total, item) => total + item.quantity, 0);
            cartCountElement.innerText = totalQuantity;
        }
    }
    // Function to display the cart summary
    function displayCart() {
        const cartSummaryElement = document.getElementById("cart-summary");
        if (cartSummaryElement) {
            // Clear the existing cart summary
            cartSummaryElement.innerHTML = "";

            // Check if the cart is empty
            if (cart.length === 0) {
                cartSummaryElement.innerHTML = "Your cart is empty.";
                return;
            }

            // Create an HTML table to display the cart contents
            const table = document.createElement("table");
            table.classList.add("cart-table");

            // Create table headers
            const thead = document.createElement("thead");
            const headerRow = document.createElement("tr");
            const headers = ["Product", "Quantity", "Price", "Total"];
            headers.forEach(headerText => {
                const th = document.createElement("th");
                th.innerText = headerText;
                headerRow.appendChild(th);
            });
            thead.appendChild(headerRow);
            table.appendChild(thead);

            // Create table body with cart items
            const tbody = document.createElement("tbody");
            cart.forEach(item => {
                const product = getProductById(item.productId); // Implement a function to get product details
                if (product) {
                    const row = document.createElement("tr");
                    const nameCell = document.createElement("td");
                    nameCell.innerText = product.name;
                    const quantityCell = document.createElement("td");
                    quantityCell.innerText = item.quantity;
                    const priceCell = document.createElement("td");
                    priceCell.innerText = `$${product.price}`;
                    const totalCell = document.createElement("td");
                    totalCell.innerText = `$${product.price * item.quantity}`;

                    row.appendChild(nameCell);
                    row.appendChild(quantityCell);
                    row.appendChild(priceCell);
                    row.appendChild(totalCell);
                    tbody.appendChild(row);
                }
            });

            table.appendChild(tbody);
            cartSummaryElement.appendChild(table);
        }
    }

    // Example function to retrieve product details by ID (you need to implement this)
    function getProductById(productId) {
        // Implement code to fetch product details from your data source (e.g., an array or server)
        // Return the product object or null if not found
        // Replace this example with your actual product retrieval logic
        return products.find(product => product.id === productId);
    }




    const addToCartButtons = document.querySelectorAll(".btn-add-to-cart");
    addToCartButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            const productId = button.getAttribute("data-productid");
            const quantity = parseInt(button.getAttribute("data-quantity"), 10);
            addToCart(productId, quantity);
            displayCart(); // Optional: Display the cart after adding a product
        });
    });

   
});
