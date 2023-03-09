const addToCart = (productId, username) => {
    if (!username) {
        console.log(window.location.host)
        window.location.href = `https://${window.location.host}/Account/Login`
        return;
    }
    const form = new FormData()
    form.append("ProductId", productId)
    form.append("Username", username)
    fetch(`https://${window.location.host}/add-to-cart`, {
        method: 'post',
        body: form,
    }).then(response => {
        return response.text();
    }).then(data => {
        if (data.toString() === 'true') {
            notifySuccess("You've added 1 item to cart.")
            totalItemUpdate(username)
        } else {
            notifyError("This product is not available now, try others.")
        }
    }).catch(e => {
        notifyError("This product is not available now, try others.")
    })
}

const hideUnusabledElement = () => {
    $(".nice-select").hide()
}
const onLoadedHook = (message, isSuccess, username) => {
    //hideUnusabledElement();
    if (message && isSuccess != null) {
        if (isSuccess === 'true') {
            notifySuccess(message)
        } else {
            notifyError(message)
        }
    }
    if (username) {
        totalItemUpdate(username)
    }
}

const totalItemUpdate = (username) => {
    const form = new FormData()
    form.append("Username", username)
    fetch(`https://${window.location.host}/Cart/TotalItems`, {
        method: 'post',
        body: form,
    })
        .then(response => response.text())
        .then(data => changeNumOfItems(data))
}

const changeNumOfItems = (newData) => {
    try {
        parseInt(newData)
        $("#numOfItemsInCart").html(newData)
    } catch (Exception) {

    }
}

const addQuantity = (itemId) => {
    const selectorId = "#" + itemId + "-quantity"
    let currentVal = $(selectorId).val()
    $(selectorId).val(parseInt(currentVal) + 1)
}

const removeQuantity = (itemId) => {
    const selectorId = "#" + itemId + "-quantity"
    let currentVal = $(selectorId).val()
    let newVal = parseInt(currentVal) - 1
    if (newVal < 1) {
        newVal = 1
    }
    $(selectorId).val(newVal)
}