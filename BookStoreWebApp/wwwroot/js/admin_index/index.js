let deleteId = 0;

const deleteProduct = (productId) => {
    let modalElement = document.getElementById("modal");
    modalElement.style.display = "flex";
    deleteId = productId;
}

const hideModal = () => {
    let modalElement = document.getElementById("modal");
    modalElement.style.display = "none";
}

const confirmDeleteProduct = () => {
    window.location.href += "/DeleteProduct/" + deleteId;
}